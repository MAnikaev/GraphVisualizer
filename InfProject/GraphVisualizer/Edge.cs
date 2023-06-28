using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer
{
    public class Edge
    {
        public Vertex V1 { get; set; }
        public Vertex V2 { get; set; }
        public int Weight { get; set; }
        public Edge(Vertex v1, Vertex v2, int weight)
        {
            V1 = v1;
            V2 = v2;
            Weight = weight;
        }
    }
}
