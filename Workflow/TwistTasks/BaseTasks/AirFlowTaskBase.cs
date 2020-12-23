using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POC
{
    public abstract class AirFlowTaskBase : TwistTaskBase
    {
        public AirFlowTaskBase(Guid id) : base(id)
        {
        }
    }
}
