using Contract;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace PrimAlgoritm
{
    public class PrimGraphSolver : IGraphSolver
    {
        private int _numOfVertices;
        public int[,] SolveGraph(int[,] adjacencyMatrix)
        {
            //для демонстрации асинхронности
            Thread.Sleep(5000);
            _numOfVertices = adjacencyMatrix.GetLength(0);
            var mst = FindMST(GetEdgesListWithMatrix(adjacencyMatrix));
            return mst;

        }

        private int[,] FindMST(List<Edge> edges)
        {
            var mst = new List<Edge>();
            var usedV = new HashSet<int>();
            var notUsedE = new List<Edge>(edges);

            // сортируем неиспользованные ребра по весу
            ParallelQuicksort(notUsedE, 0, notUsedE.Count - 1);


            // добавляем минимальное ребро в дерево, а инцендентные ему вершины в список исп. вершин
            var startEdge = notUsedE[0];
            mst.Add(startEdge);
            usedV.Add(startEdge.V1);
            usedV.Add(startEdge.V2);
            notUsedE.Remove(startEdge);

            //добавляем ребра пока количество вершин у дерева не станет таким же как у начального графа
            while (usedV.Count != _numOfVertices)
            {
                mst.Add(GetMinWeightedEdge(notUsedE, usedV));
            }

            return GetMatrixWithEdgesList(mst);
        }
        private Edge GetMinWeightedEdge(List<Edge> notUsedE, HashSet<int> usedV)
        {
            // выбираем первое попавшееся ребро которое инцендентно уже построенному дереву
            foreach (var edge in notUsedE)
            {
                if (usedV.Contains(edge.V1))
                {
                    usedV.Add(edge.V2);
                    notUsedE.Remove(edge);
                    return edge;
                }
                else if (usedV.Contains(edge.V2))
                {
                    usedV.Add(edge.V1);
                    notUsedE.Remove(edge);
                    return edge;
                }
            }
            return null;
        }
        private List<Edge> GetEdgesListWithMatrix(int[,] matrix)
        {
            var edges = new HashSet<Edge>();

            for(int i = 0; i < matrix.GetLength(0) - 1 ; i++)
                for (int j = i + 1; j < matrix.GetLength(1); j++)
                {
                    if (i != j && matrix[i, j] != 0)
                    {
                         edges.Add(new Edge(i, j, matrix[i, j]));
                    }
                }
            

            return edges.ToList();
        }
        private int[,] GetMatrixWithEdgesList(List<Edge> edges)
        {
            var matrix = new int[_numOfVertices, _numOfVertices];

            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 0;
                }
            }

            foreach(var edge in edges)
            {
                matrix[edge.V1, edge.V2] = edge.Weight;
                matrix[edge.V2, edge.V1] = edge.Weight;
            }

            return matrix;
        }
        public void ParallelQuicksort(List<Edge> list, int left, int right)
        {
            if (left < right)
            {
                var pivot = Partition(list, left, right);

                var leftTask = Task.Run(() => ParallelQuicksort(list, left, pivot - 1));
                var rightTask = Task.Run(() => ParallelQuicksort(list, pivot + 1, right));

                Task.WaitAll(leftTask, rightTask);
            }
        }
        private int Partition(List<Edge> list, int left, int right)
        {
            var pivot = list[left].Weight;
            var i = left;
            var j = right;

            while (i < j)
            {
                while (list[j].Weight >= pivot && i < j) j--;
                while (list[i].Weight <= pivot && i < j) i++;
                Swap(list, i, j);
            }

            list[left] = list[i];
            list[i] = list[left];
            return i;
        }
        private void Swap(List<Edge> list, int i, int j)
        {

            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
    
