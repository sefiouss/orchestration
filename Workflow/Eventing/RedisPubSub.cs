using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class RedisPubSub : IPubSub
    {
        private static readonly Lazy<RedisPubSub> lazy = new Lazy<RedisPubSub>(() => new RedisPubSub());

        private const string RedisConnectionString = "localhost";
        private readonly ConnectionMultiplexer connection;
        private readonly ISubscriber pubsub;

        private RedisPubSub()
        {
            this.connection = ConnectionMultiplexer.Connect(RedisConnectionString);

            this.pubsub = this.connection.GetSubscriber();
        }

        public static RedisPubSub Instance { get { return lazy.Value; } }

        public async Task Publish(PubSubChannel channel, PubSubMessage message)
        {
            var redisChannel = new RedisChannel(channel.Channel, RedisChannel.PatternMode.Auto);
            var redisMessage = new RedisValue(message.Content);

            await this.pubsub.PublishAsync(redisChannel, redisMessage);
        }

        public async Task Subscribe(PubSubChannel channel, Action<PubSubChannel, PubSubMessage> handler)
        {
            var redisChannel = new RedisChannel(channel.Channel, RedisChannel.PatternMode.Auto);
            var redisHandler = new Action<RedisChannel, RedisValue>((c, v) =>
            {
                var pubSubChannel = new PubSubChannel(c.ToString());
                var pubSubMessage = new PubSubMessage(v.ToString());

                handler(pubSubChannel, pubSubMessage);
            });

            await this.pubsub.SubscribeAsync(redisChannel, redisHandler);
        }

        public async Task Unsubscribe(PubSubChannel channel, Action<PubSubChannel, PubSubMessage> handler)
        {
            var redisChannel = new RedisChannel(channel.Channel, RedisChannel.PatternMode.Auto);
            var redisHandler = new Action<RedisChannel, RedisValue>((c, v) =>
            {
                var pubSubChannel = new PubSubChannel(c.ToString());
                var pubSubMessage = new PubSubMessage(v.ToString());

                handler(pubSubChannel, pubSubMessage);
            });

            await this.pubsub.UnsubscribeAsync(redisChannel, redisHandler);
        }
    }
}
