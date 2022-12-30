using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day9
{
    public class Knot
    {
        private int[] position;
        public int[] Position { get { return position; } }
        public int PositionX { get { return position[0]; } set { position[0] = value; } }
        public int PositionY { get { return position[1]; } set { position[1] = value; } }

        private int[] lastPosition;
        public int LastPositionX { get { return lastPosition[0]; } }
        public int LastPositionY { get { return lastPosition[1]; } }
        public Knot(int[] _position) 
        { 
            position = _position;
            lastPosition = position;
        } 
        public void Set(int[] _position)
        {
            lastPosition = position;
            position = _position;
        }
    }
    public class Rope
    {
        public List<Knot> knots = new List<Knot>();
        private Dictionary<string, int> tailVisited = new Dictionary<string, int>();
        public Dictionary<string, int> TailVisited { get { return tailVisited; } }
        //Directions in which head can move 
        private Dictionary<char, int[]> directions = new Dictionary<char, int[]>()
        {
            {'R',new int[2]{ 1, 0 }},
            {'L',new int[2]{ -1, 0 }},
            {'U',new int[2]{ 0, 1 }},
            {'D',new int[2]{ 0, -1 }}
        };
        public Rope(int tailLength)
        {
            // 0 is Head
            knots.Add(new Knot(new int[2] { 0, 0 }));
            for (int i = 0; i < tailLength; i++) knots.Add(new Knot(new int[2] { 0, 0 }));
            tailVisited.Add($"{0},{0}", 1);
        }
        public void Move(char motionDirection, int steps)
        {
            int[] direction = GetDirection(motionDirection);
            for(int i=0;i<steps;i++)
            {
                SetHeadPosition(direction[0], direction[1]);
            }
        }

        private int[] GetDirection(char motionDirection)
        {
            foreach (var item in directions)
            {
                if (item.Key == motionDirection) return item.Value;
            }
            return new int[2] { 0, 0 };
        }
        public void SetHeadPosition(int xDirection, int yDirection)
        {
            //if direction was different than those declared ,head won't move
            if (xDirection == 0 && yDirection == 0) return;
            //moving head
            knots[0].Set(new int[2] { knots[0].PositionX + xDirection, knots[0].PositionY + yDirection });
            //Console.WriteLine($"Head position:[{knots[0].PositionX},{knots[0].PositionY}]");
            for(int i=1;i<knots.Count;i++)
            {
                SetTailPosition(knots[i],knots[i-1]);

            }
        }
        private void SetTailPosition(Knot currentKnot,Knot previousKnot)
        {
            //if head moved into first knot, any knot position won't be changed

            int distanceX, distanceY;
            distanceX = Math.Abs(previousKnot.PositionX - currentKnot.PositionX);
            distanceY = Math.Abs(previousKnot.PositionY - currentKnot.PositionY);
            if (distanceX < 2 && distanceY < 2) return;

            if (previousKnot.PositionX > currentKnot.PositionX) currentKnot.PositionX += 1;
            if (previousKnot.PositionX < currentKnot.PositionX) currentKnot.PositionX -= 1;

            if (previousKnot.PositionY > currentKnot.PositionY) currentKnot.PositionY += 1;
            if(previousKnot.PositionY < currentKnot.PositionY) currentKnot.PositionY -= 1;

            //if ((distanceX == 2 && distanceY == 1) || (distanceY == 2 && distanceX == 1))
            //    {
            //        int xMove = ((knots[i - 1].PositionX - knots[i].PositionX) / (Math.Abs(knots[i - 1].PositionX - knots[i].PositionX)));
            //        int yMove = ((knots[i - 1].PositionY - knots[i].PositionY) / (Math.Abs(knots[i - 1].PositionY - knots[i].PositionY)));

            //    }
            //    knots[i].Set(new int[2] { knots[i].PositionX + xMove, knots[i].PositionY + yMove });
            //    else
            //    {
            //        knots[i].Set(new int[2] { knots[i - 1].LastPositionX, knots[i - 1].LastPositionY });


            //    }
                if(knots.IndexOf(currentKnot) == knots.Count-1)
                {
                   // Console.WriteLine($"End position:[{currentKnot.PositionX},{currentKnot.PositionY}]");
                if (!tailVisited.ContainsKey($"{currentKnot.PositionX},{currentKnot.PositionY}"))
                    {
  
                        tailVisited.Add($"{currentKnot.PositionX},{currentKnot.PositionY}", 1);
                    //        Console.WriteLine($" Added {knots[i].PositionX},{knots[i].PositionY}");
                }

            }
            }
        

        //Just for testing purposes
        //public void DrawMap()
        //{
        //    List<string[]> list = tailVisited.Keys.Select(x => x.Split(',')).ToList();

        //    int minXValue = tailVisited.Min(x => int.Parse(x.Key.Split(',')[0]));
        //    int maxXValue = tailVisited.Max(x => int.Parse(x.Key.Split(',')[0]));
        //    Console.WriteLine($"min:{minXValue} max:{maxXValue}");
        //    int minYValue = tailVisited.Min(x => int.Parse(x.Key.Split(',')[1]));
        //    int maxYValue = tailVisited.Max(x => int.Parse(x.Key.Split(',')[1]));
        //    Console.WriteLine($"min:{minYValue} max:{maxYValue}");
 

        //    for (int i = minXValue; i < maxXValue; i++)
        //    {
        //        for (int j = minYValue; j < maxYValue; j++)
        //        {
        //            string[] test = { i.ToString(), j.ToString() };
        //            bool isThere= list.Any(x => x.SequenceEqual(test));
        //            if (isThere)
        //            {
                       
        //                Console.Write("#");
                        
        //            }
        //            else Console.Write(".");
        //        }
        //        Console.WriteLine();
        //    }
        //}
        
    }
    public class Day : BaseDay
    {
        public override string FirstPart()
        {
            Rope rope = new Rope(1);
            foreach (var input in inputContent)
            {
                string[] _inputContent = input.Split(' ');
                rope.Move(char.Parse(_inputContent[0]), int.Parse(_inputContent[1]));
            }
            return rope.TailVisited.Count.ToString();
        }

        public override string SecondPart()
        {
            Rope rope = new Rope(9);
            foreach (var input in inputContent)
            {
                string[] _inputContent = input.Split(' ');
                rope.Move(char.Parse(_inputContent[0]), int.Parse(_inputContent[1]));
            }

            //testing
           // rope.DrawMap();
            return rope.TailVisited.Count.ToString();
        }
    }
}