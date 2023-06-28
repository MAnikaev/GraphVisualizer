using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer
{
    public class GraphDrawer
    {
        private List<Vertex> _vertexes = new List<Vertex>();

        private void DrawVertexes(ICanvas canvas, int n)
        {
            
            for (int i = 0; i < n; i++)
            {
                var X = 275 + (float)Math.Cos(2 * i * Math.PI / n) * 200;
                var Y = 250 + (float)Math.Sin(2 * i * Math.PI / n) * 200;
                _vertexes.Add(new Vertex(i, X, Y));
                canvas.StrokeColor = Colors.Blue;
                canvas.FontColor = Colors.Blue;
                canvas.FillColor = Colors.White;
                canvas.DrawCircle(X, Y, 11);
                canvas.FillCircle(X, Y, 11);
                canvas.DrawString(i.ToString(), X, Y + 2, HorizontalAlignment.Justified);
            }
        }

        private void DrawEdge(ICanvas canvas, Edge edge)
        {
            if (edge.Weight <= 0)
                return;
            
            var xOfMiddle = (edge.V1.X + edge.V2.X) / 2;
            var yOfMiddle = (edge.V1.Y + edge.V2.Y) / 2;
            canvas.DrawLine(edge.V1.X, edge.V1.Y, edge.V2.X, edge.V2.Y);
            canvas.StrokeColor = Colors.Blue;
            canvas.FontColor = Colors.Blue;
            canvas.DrawString(edge.Weight.ToString(), xOfMiddle + 5, yOfMiddle - 5, HorizontalAlignment.Justified);
        }

        private List<Edge> GetEdgesByMatrix(int[,] graph)
        {
            var edges = new HashSet<Edge>();
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (i != j || graph[i, j] <= 0)
                    {
                        edges.Add(new Edge(_vertexes[i], _vertexes[j], graph[i, j]));
                    }
                }
            }
            return edges.ToList();
        }

        public void DrawGraph(int[,] graph, ICanvas canvas)
        {
            canvas.FillColor = Colors.White;
            canvas.FillCircle(275, 250, 220);
            DrawVertexes(canvas, graph.GetLength(0));
            foreach (var edge in GetEdgesByMatrix(graph))
            {
                DrawEdge(canvas, edge);
            }
            DrawVertexes(canvas, graph.GetLength(0));
            
        }
    }
}
