using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;

namespace AoC2022.Day12
{
    public class Node
    {
        public char Data { get; private set; }
        public List<Node> Neighbours { get; private set; } = new List<Node>();
        public Node(char data)
        {
            Data = data;
        }
        public Node GetNode() => this;
        public void Set(char data) => this.Data = data;

        public void AddNeighbour(Node neighbour) => Neighbours.Add(neighbour);
    }
    public class Heightmap
    {
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

            StartPoint = FindPointWithName('S');
            EndPoint = FindPointWithName('E');
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
                    AddVertex(i, j, new Node(stringMap[i][j]));
                }
            }
        }
        /// <summary>
        /// Returns first occured point
        /// </summary>
        /// <param name="data">Name of point</param>
        /// <returns></returns>
        public Node FindPointWithName(char data)
        {
            for (int i = 0; i < Verticies.Length; i++)
            { 
                for(int j = 0; j < Verticies[i].Length;j++)
                {
                    if (Verticies[i][j].Data == data) return Verticies[i][j];
                }
            }
            return null;
        }
        /// <summary>
        /// Return list of points
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Node> FindPointsWithName(char data)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < Verticies.Length; i++)
            {
                for (int j = 0; j < Verticies[i].Length; j++)
                {
                    if (Verticies[i][j].Data == data) nodes.Add(Verticies[i][j]);
                }
            }
            return nodes;
        }
        /// <summary>
        /// Set neighbours for all nodes
        /// </summary>
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
            List<Node> wantedPoints = heightmap.FindPointsWithName('a');
            foreach (Node node in wantedPoints)
            {
                paths = Dijkstra(heightmap, node);
                if (paths[heightmap.EndPoint] < shortestRouteX) shortestRouteX = paths[heightmap.EndPoint];
            }
            return shortestRouteX.ToString();

        }
        public Dictionary<Node,int> Dijkstra(Heightmap heightmap, Node start)
        {
            //if start parameter is different,we want change it for heightmap
            if (start != heightmap.StartPoint) heightmap.SetStart(start);
            //queue for getting element with smallest priority
            PriorityQueue<Node, int> priorityQueue = new PriorityQueue<Node, int>();
            //tracking visited elements
            //List<Node> visitedElements = new List<Node>();
            Dictionary<Node, int> distance = new Dictionary<Node, int>();
            Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
            distance.Add(start, 0);
            priorityQueue.Enqueue(start, 0);
            //visitedElements.Add(start);

            for (int i = 0; i < heightmap.Verticies.Length; i++)
            {
                for (int j = 0; j < heightmap.Verticies[i].Length; j++)
                {
                    if (heightmap.Verticies[i][j] != start) distance.Add(heightmap.Verticies[i][j], int.MaxValue);
                }
            }
            while (priorityQueue.Count > 0)
            {
                Node smallestElement = priorityQueue.Dequeue();

                foreach(Node neighbour in smallestElement.Neighbours)
                {
                    int higherSteps;
                    higherSteps = (int)neighbour.Data - (int)smallestElement.Data;
                    //Which means that it wasn't explored yet and its at the same step or one higher ,which allow us to move 
                    if (
                        //!visitedElements.Contains(neighbour) && 
                        higherSteps <= 1)
                    {
                        int altDistance = distance[smallestElement] + 1;
                      //  visitedElements.Add(neighbour);
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
