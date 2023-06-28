using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimAlgoritm
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            int[,] adjacencyMatrix = new int[,]
        {
            { 0, 1, 1, 0},
            { 1, 0, 0, 10},
            { 1, 0, 0, 1},
            { 0, 10, 1, 0}
        };
            var solver = new PrimGraphSolver();
            var mst = solver.SolveGraph(adjacencyMatrix);
            for (int i = 0; i < mst.GetLength(0); i++)
            {
                for (int j = 0; j < mst.GetLength(1); j++)
                {
                    Console.Write(mst[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
