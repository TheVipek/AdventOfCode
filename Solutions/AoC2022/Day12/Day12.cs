using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Xml.Linq;

namespace AoC2022.Day12
{
    public class Node
    {
        public char Data { get; private set; }
        public List<Node> Neighbours { get; private set; } = new List<Node>();
        //public int[] Position { get; private set; }
        public Node(char data)
        {
            Data = data;
        }
        public Node GetNode() => this;
        //public ArrayList GetNeighbours()
        //{
        //    return new ArrayList() {
        //    new int[2]{ Position[0] - 1, Position[1] }, // x-1
        //    new int[2]{ Position[0] + 1, Position[1] }, // x+1
        //    new int[2]{ Position[0], Position[1] - 1 }, // y-1
        //    new int[2]{ Position[0], Position[1] + 1 }  // y+1
        //    };
        //}
        //public void Set(char data, int[] position)
        //{
        //    this.Data = data;
        //    this.Position = position;
        //}
        public void Set(char data) => this.Data = data;
        public void AddNeighbour(Node neighbour) => Neighbours.Add(neighbour);
        //public void Set(int[] position) => this.Position = position;
    }
    public class Heightmap
    {
        // public List<Node> Verticies { get; } = new List<Node>();
        public Node[][] Verticies { get; private set; }
        public Node StartPoint { get; private set; }
        public Node EndPoint { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringMap"></param>
        /// <param name="startPosition">If custom StartingPosition is needed</param>
        public Heightmap(string[] stringMap)
        {
            CreateHeightmap(stringMap);

            //StartPoint = FindPointWithName('S');
            //EndPoint = FindPointWithName('E');
            StartPoint.Set('a');
            EndPoint.Set('z');
            SetAllNeighbours();
        }
        public void SetStart(Node newStartPoint)
        {
            if (newStartPoint == StartPoint) return;
            StartPoint = newStartPoint;
        }
        public void CreateHeightmap(string[] stringMap)
        {
            //Initializing heightmap 
            Verticies = new Node[stringMap.Length][];
            for (int i = 0; i < stringMap.Length; i++)
            {
                Verticies[i] = new Node[stringMap[i].Length];
                for (int j = 0; j < stringMap[i].Length; j++)
                {
                    //Filling heightmap
                    // AddVertex(new Node(stringMap[i][j], new int[2] { i, j }));
                    AddVertex(i, j, new Node(stringMap[i][j]));
                    if (stringMap[i][j] == 'S') StartPoint = Verticies[i][j];
                    else if (stringMap[i][j] == 'E') EndPoint = Verticies[i][j];
                }
            }
        }
        /// <summary>
        /// Returns first occured point
        /// </summary>
        /// <param name="data">Name of point</param>
        /// <returns></returns>
       // public Node FindPointWithName(char data) => Verticies.Where(x => x == data).FirstOrDefault(defaultValue: null);
        /// <summary>
        /// Returns point at exact position
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        //public Node FindPointWithPosition(int[] position) => Verticies.Where(x => x.Position.SequenceEqual(second: position)).FirstOrDefault(defaultValue: null);
        private void SetAllNeighbours()
        {
            for (int i = 0; i < Verticies.Length; i++)
            {
                for (int j = 0; j < Verticies[i].Length; j++)
                {
                    if (i > 0) Verticies[i][j].AddNeighbour(Verticies[i - 1][j]);
                    if (i < Verticies.Length - 1) Verticies[i][j].AddNeighbour(Verticies[i + 1][j]);
                    if (j > 0) Verticies[i][j].AddNeighbour(Verticies[i][j - 1]);
                    if (j < Verticies[i].Length - 1) Verticies[i][j].AddNeighbour(Verticies[i][j + 1]);

                }
            }
        }

        /// <summary>
        /// Adds vertex to Verticies
        /// </summary>
        /// <param name="verticle">Node that we want to add</param>
        private void AddVertex(

            int x, int y,
            Node verticle
            ) =>
            Verticies[x][y] = verticle;
        // Verticies.Add(verticle);

    }

    public class Day : BaseDay
	{
        public override string FirstPart()
        {
            Heightmap heightmap = new Heightmap(inputContent);
            Dictionary<Node,int> paths = Dijkstra(heightmap, heightmap.StartPoint);
            return paths[heightmap.EndPoint].ToString();
        }
        public override string SecondPart()
        {
            Heightmap heightmap = new Heightmap(inputContent);
            Dictionary<Node, int> paths;
            int shortestRouteX = int.MaxValue;
            //Default start point
            shortestRouteX = int.MaxValue;

            //var _heightmap = heightmap.Verticies.Where(x => x.Data == 'a').Select(x => x);
            //foreach (var item in _heightmap)
            //{
            //    paths = Dijkstra(heightmap, item);
            //    if (paths[heightmap.EndPoint] < shortestRouteX) shortestRouteX = paths[heightmap.EndPoint];
            //}
            for (int i = 0; i < heightmap.Verticies.Length; i++)
            {
                for (int j = 0; j < heightmap.Verticies[i].Length; j++)
                {
                    if(heightmap.Verticies[i][j].Data == 'a')
                    {
                        paths = Dijkstra(heightmap, heightmap.Verticies[i][j]);
                        if (paths[heightmap.EndPoint] < shortestRouteX) shortestRouteX = paths[heightmap.EndPoint];
                    }
                }
            }
            //foreach (var item in heightmap.Verticies)
            //{
            //    if (item.Data == 'a')
            //    {
            //        paths = Dijkstra(heightmap, item);
            //        if (paths[heightmap.EndPoint] < shortestRouteX) shortestRouteX = paths[heightmap.EndPoint];
            //    }

            //}
            return shortestRouteX.ToString();

        }
        public Dictionary<Node,int> Dijkstra(Heightmap heightmap, Node start)
        {
            if (start != heightmap.StartPoint) heightmap.SetStart(start);
            PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();
            List<Node> visitedElements = new List<Node>();
            Dictionary<Node, int> distance = new Dictionary<Node, int>();
            Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
            distance.Add(start, 0);
            priorityQueue.Enqueue(start, 0);
            visitedElements.Add(start);

            for (int i = 0; i < heightmap.Verticies.Length; i++)
            {
                for (int j = 0; j < heightmap.Verticies[i].Length; j++)
                {
                    if (heightmap.Verticies[i][j] != start) distance.Add(heightmap.Verticies[i][j], int.MaxValue);
                }
            }
            //foreach (Node node in heightmap.Verticies)
            //{
            //    if(node != start) distance.Add(node, int.MaxValue);
            //}
            while (priorityQueue.Count > 0)
            {
                Node smallestElement = priorityQueue.Dequeue();

                //foreach (int[] neighbour in smallestElement.GetNeighbours())
                foreach(Node neighbour in smallestElement.Neighbours)
                {
                    //Node _neighbour = heightmap.FindPointWithPosition(neighbour);

                    //if (_neighbour == null) continue;
                    //int higherSteps;
                    //higherSteps = (int)_neighbour.Data - (int)smallestElement.Data;
                    ////Which means that it wasn't explored yet and its at the same step or one higher ,which allow us to move 
                    //if (!visitedElements.Contains(_neighbour) && higherSteps <= 1)
                    //{
                    //    int altDistance = distance[smallestElement] + 1;

                    //    if(altDistance < distance[_neighbour])
                    //    {
                    //        distance[_neighbour] = altDistance;
                    //        previousNodes[_neighbour] = smallestElement;
                    //        priorityQueue.Enqueue(_neighbour, altDistance);
                    //    }
                    //}
                    if (neighbour == null) continue;
                    int higherSteps;
                    higherSteps = (int)neighbour.Data - (int)smallestElement.Data;
                    //Which means that it wasn't explored yet and its at the same step or one higher ,which allow us to move 
                    if (!visitedElements.Contains(neighbour) && higherSteps <= 1)
                    {
                        int altDistance = distance[smallestElement] + 1;

                        if (altDistance < distance[neighbour])
                        {
                            distance[neighbour] = altDistance;
                            previousNodes[neighbour] = smallestElement;
                            priorityQueue.Enqueue(neighbour, altDistance);
                        }
                    }
                }
            }
            return distance;
        }
    }
}
