using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class TheBestEventHub
    {
        private static readonly Lazy<TheBestEventHub> lazy = new Lazy<TheBestEventHub> (() => new TheBestEventHub());

        private TheBestEventHub()
        {
        }

        public static TheBestEventHub Instance { get { return lazy.Value; } }

        public async Task Publish(string channel, string message)
        {
            await RedisPubSub.Instance.Publish(new PubSubChannel(channel), new PubSubMessage(message));
        }
    }
}
