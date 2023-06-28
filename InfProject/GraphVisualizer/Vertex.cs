using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer
{
    public class Vertex
    {
        public int Number { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Vertex(int num, float x, float y)
        { 
            Number = num;
            X = x;
            Y = y;
        }
    }
}
