using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawer
{
    public abstract class GraphBuilder
    {
        public abstract Graph Create(int[] degreeSet);
    }
}
