using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawer
{
    public class Graph
    {
        public int[] DegreeSet { get; }

        public bool[,] AdjacencyMatrix { get; }

        public Vertex[] Verteces { get; }

        public Graph(int[] degreeSet, int n)
        {
            DegreeSet = degreeSet;
            AdjacencyMatrix = new bool[n, n];
            Verteces = new Vertex[n];
        }
    }
}
