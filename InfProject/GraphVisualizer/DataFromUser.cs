
using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphVisualizer
{
    public static class DataFromUser
    {
        public static int[,] Graph;
        public static bool IsDarkTheme;
        public static int[,] SolvedGraph;
        public static IGraphSolver Solver;
    }
}
