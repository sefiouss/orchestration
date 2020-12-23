using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public class PubSubMessage
    {
        public PubSubMessage(string messageContent)
        {
            this.Content = messageContent;
        }

        public string Content { get; private set; }//payload { "id"="", ... }
    }
}
