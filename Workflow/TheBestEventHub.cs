using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class TheBestEventHub
    {
        public event EventHandler<TheBestEventHubEventArgs> RaiseEvent;

        private static readonly Lazy<TheBestEventHub> lazy = new Lazy<TheBestEventHub> (() => new TheBestEventHub());

        private TheBestEventHub()
        {
        }

        public static TheBestEventHub Instance { get { return lazy.Value; } }

        public async Task Publish(string channel, string message)
        {
            await Task.Delay(3000);
            RaiseEvent(this, new TheBestEventHubEventArgs(channel, message));
        }
    }

    public class TheBestEventHubEventArgs : EventArgs
    {
        public TheBestEventHubEventArgs(string channel, string message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public string Channel { get; private set; }
        public string Message { get; private set; }
    }
}
