using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC
{
    public interface IPausableTask
    {
        void Pause();

        void Resume();
    }
}
