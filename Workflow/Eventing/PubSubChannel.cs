using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class PubSubChannel
    {
        public PubSubChannel(string channel)
        {
            this.Channel = channel;
        }

        public string Channel
        {
            get;
            private set;
        }
    }
}
