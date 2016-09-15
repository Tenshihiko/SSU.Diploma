using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawer
{
    public class PlanarBuilder : GraphBuilder
    {
        public override Graph Create(int[] degreeSet)
        {
            if (!CheckDegreeSet(degreeSet))
            {
                return null;
            }
            int n = (degreeSet.Length - 1) * 2 + (degreeSet.Sum() - degreeSet[0]) * degreeSet[0];

            Graph g = new Graph(degreeSet, n);
            FillThePlanar(g);
            return g;
        }

        private void FillThePlanar(Graph g)
        {
            throw new NotImplementedException();
        }

        private bool CheckDegreeSet(int[] degreeSet)
        {
            throw new NotImplementedException();
        }
    }
}
