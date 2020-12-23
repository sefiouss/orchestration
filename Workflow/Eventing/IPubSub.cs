using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public interface IPubSub
    {
        Task Publish(PubSubChannel channel, PubSubMessage message);

        Task Subscribe(PubSubChannel channel, Action<PubSubChannel, PubSubMessage> handler);

        Task Unsubscribe(PubSubChannel channel, Action<PubSubChannel, PubSubMessage> handler);
    }
}
