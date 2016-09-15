using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawer
{
    public class TreeBuilder : GraphBuilder
    {
        public override Graph Create(int[] degreeSet)
        {
            if (!CheckDegreeSet(degreeSet))
            {
                return null;
            }
            int n = degreeSet.Length - 1 + degreeSet.Sum() - 1;

            Graph g = new Graph(degreeSet, n);
            FillTheTree(g);
            return g;
        }

        private void FillTheTree(Graph g)
        {
            throw new NotImplementedException();
        }

        private bool CheckDegreeSet(int[] degreeSet)
        {
            throw new NotImplementedException();
        }
    }
}
