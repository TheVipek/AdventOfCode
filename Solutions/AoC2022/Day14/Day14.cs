using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


namespace AoC2022.Day14
{
    public class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Position(int x,int y)
        {
            X = x;
            Y = y;
        }
        public void SetPositionX(int x) => X = x;
        public void SetPositionY(int y) => Y = y;
        public void SetPosition(Position position)
        {
            X = position.X;
            Y = position.Y;
        }
    }
    
    public class Sand
    {
        public Position Position{ get; private set; } 
        public Position Down { get { return new Position(Position.X, Position.Y + 1); } } 
        public Position DownLeft { get { return new Position(Position.X - 1, Position.Y + 1); } }
        public Position DownRight { get { return new Position(Position.X + 1, Position.Y + 1); } }

        public bool IsRested { get; set; } = false;
        
        public Sand(Position position)
        {
            Position = position;
        }
        
    }
    public class MapStructure
    {
        public Dictionary<string, Position> elementsOnMap = new Dictionary<string, Position>();
        public HashSet<Sand> sandOnMap = new HashSet<Sand>();

        public Position OriginSand { get; private set; } = new Position(500, 0);
        public Position StructureRangeX { get; private set; }
        public int MinimumYPoint { get; private set; }

        public MapStructure() { }
        public void GeneratingProcess(Position pos)
        {
            if (CanGenerate(pos)) 
            { 
                elementsOnMap.Add($"{pos.X}:{pos.Y}",pos);
               // Console.WriteLine($"Generation at: {elementPosition[0]}:{elementPosition[1]}");
            } 
        }
        /// <summary>
        /// Return true if there's no xPos and yPos element in elementsOnMap
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public bool CanGenerate(Position pos)
        {

            if (!elementsOnMap.ContainsKey($"{pos.X}:{pos.Y}"))
            {
                return true;
            }
            return false;
          
        }
        /// <summary>
        /// Smaller x position and the biggest x
        /// </summary>
        public void SetAbyssPositions(int minRangeX = 0 ,int maxRangeX = 0)
        {
            if(minRangeX != 0 && maxRangeX != 0)
            {
                StructureRangeX = new Position(minRangeX,maxRangeX);
                return;
            }
            StructureRangeX = new Position(elementsOnMap.Min(pos => pos.Value.X), elementsOnMap.Max(pos => pos.Value.X));
        }
        /// <summary>
        /// Set minimum Y point for structure
        /// We're using Max method,beacuse the lower we go ,the lower value gets
        /// </summary>
        /// <param name="additionalDown">How much lower we want go from minimum Y structure point</param>
        public void SetMinimumYPoint(int additionalDown = 0) 
        { 
            
            MinimumYPoint = elementsOnMap.Max(element => element.Value.Y) + additionalDown ;
            //we need to generate additional layer
            if(additionalDown > 0)
            {
                FillBeetwen(new Position(StructureRangeX.X, MinimumYPoint),new Position(StructureRangeX.Y, MinimumYPoint));
            }
            
        }
        
            
        /// <summary>
        /// Get line of scan and applies it to mapStructure
        /// </summary>
        /// <param name="scan"></param>
        public void SetPartOfStructure(string[] scan)
        {
            Position lastPosition = null;
            //i+=2 beacuse we want to catch x and y and they're on different indexes
            for (int i = 0; i < scan.Length; i += 2)
            {
                Position currentPosition = new Position(
                    int.Parse(scan[i]),
                    int.Parse(scan[i + 1])
                    );

                if (lastPosition == null)
                {

                    lastPosition = currentPosition;
                    //Console.Write($"{lastPosition[0]}:{lastPosition[1]} - ...\n");
                    GeneratingProcess(lastPosition);
                    continue;
                }
                //Console.Write($"lastPosition:{lastPosition[0]}:{lastPosition[1]} - {currentPosition[0]}:{currentPosition[1]} \n");

                FillBeetwen(lastPosition, currentPosition);
                lastPosition = currentPosition;
            }

        }
        /// <summary>
        /// Fills elements between lastPosition and currentPosition
        /// </summary>
        /// <param name="lastPosition"></param>
        /// <param name="currentPosition"></param>
        public void FillBeetwen(Position lastPosition, Position currentPosition)
        {

            //difference between last and current pos
            int differenceInPositionX = lastPosition.X - currentPosition.X;
            int differenceInPositionY = lastPosition.Y - currentPosition.Y;
            //Console.WriteLine($"DifferenceX:{differenceInPositionX} DifferenceY:{differenceInPositionY}");


            //We have to go left
            if (differenceInPositionX > 0)
            {
                while (differenceInPositionX > 0)
                {
                    GeneratingProcess(new Position(lastPosition.X - differenceInPositionX, lastPosition.Y));
                    differenceInPositionX--;
                }
            }
            //We have to go right
            else if (differenceInPositionX < 0)
            {
                while (differenceInPositionX < 0)
                {
                    GeneratingProcess(new Position(lastPosition.X - differenceInPositionX, lastPosition.Y));
                    differenceInPositionX++;

                }
            }
            //We have to go down
            if (differenceInPositionY > 0)
            {
                while (differenceInPositionY > 0)
                {
                    GeneratingProcess(new Position(lastPosition.X, lastPosition.Y - differenceInPositionY));
                    differenceInPositionY--;

                }
            }
            //We have to go up
            else if (differenceInPositionY < 0)
            {
                while (differenceInPositionY < 0)
                {
                    GeneratingProcess(new Position(lastPosition.X, lastPosition.Y - differenceInPositionY));
                    differenceInPositionY++;

                }
            }


        }
        //
        public Sand SpawnSand()
        {
            Sand sand = new(new Position(OriginSand.X,OriginSand.Y));
            //Console.WriteLine($"Sand base position:{sand.Position[0]}:{sand.Position[1]}");
            MoveSand(ref sand);
            //Which means that its in rest state
            if (sand != null && sand.IsRested == true)
            {
                //Console.WriteLine($"Sand rested position:[{sand.Position[0]}:{sand.Position[1]}");
               // Console.WriteLine("Sand is rest-stated");
                sandOnMap.Add(sand);
                elementsOnMap.Add($"{sand.Position.X}:{sand.Position.Y}",sand.Position);

                return sand;

            }
            return null;
        }

        public object MoveSand(ref Sand sand)
        {
            //Console.WriteLine($"Sand is trying to achieve new position:[{sand.Position.X}:{sand.Position.Y}");

            //Check if sand position isn't out of 'structure bounds'
            if ( (sand.Position.X < StructureRangeX.X || sand.Position.X > StructureRangeX.Y))
            {
                //Console.WriteLine($"Sand is falling into abyss due to new position [{sand.Position[0]}:{sand.Position[1]}]" +
                //    $"Abyss range: lower than: {StructureRangeX[0]},higher than:{StructureRangeX[1]}");
                sand = null;
                return null;
            }
            //check if we can move down
            if (CanGenerate(sand.Down))
            {
                //Console.WriteLine($"Down {sand.Down.X}:{sand.Down.Y}");
                sand.Position.SetPosition(sand.Down);
                return MoveSand(ref sand);
            }
            //check if we can move down left
            if (CanGenerate(sand.DownLeft))
            {
                //Console.WriteLine($"DownLeft {sand.DownLeft.X}:{sand.DownLeft.Y}");

                sand.Position.SetPosition(sand.DownLeft);
                return MoveSand(ref sand);
            }
            //check if we can move down right
            if (CanGenerate(sand.DownRight))
            {
                //Console.WriteLine($"DownRight {sand.DownRight.X}:{sand.DownRight.Y}");

                sand.Position.SetPosition(sand.DownRight);
                return MoveSand(ref sand);
            }
            sand.IsRested = true;
            return null;

        }
    }
    public class Day : BaseDay
    {
        public override string FirstPart()
        {
            MapStructure mapStructure = new MapStructure();  
            for(int i=0;i<inputContent.Length;i++) 
            {
                string[] scan = inputContent[i].Split(new char[] { ',', '-', '>', ' ' },StringSplitOptions.RemoveEmptyEntries);
                mapStructure.SetPartOfStructure(scan);
            }
            mapStructure.SetAbyssPositions();
            mapStructure.SetMinimumYPoint();
            while (true)
            {
                Sand sandSpawned = mapStructure.SpawnSand();
                if (sandSpawned == null || sandSpawned.IsRested == false)
                {
                    break;
                }
            }
            //foreach (var item in mapStructure.sandOnMap)
            //{
            //    Console.WriteLine($"{item.Position[0]}:{item.Position[1]}");
            //}
            // Console.WriteLine(mapStructure.sandOnMap.Count);

            return mapStructure.sandOnMap.Count.ToString();

        }
        public override string SecondPart() 
        {
            MapStructure mapStructure = new MapStructure();
            for (int i = 0; i < inputContent.Length; i++)
            {
                string[] scan = inputContent[i].Split(new char[] { ',', '-', '>', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                mapStructure.SetPartOfStructure(scan);
            }
            mapStructure.SetAbyssPositions(-1000, 1000);
            mapStructure.SetMinimumYPoint(2);
            while (true)
            {
                Sand sandSpawned = mapStructure.SpawnSand();
                if(sandSpawned.IsRested == true)
                {
                    if (sandSpawned.Position.X == 500 && sandSpawned.Position.Y == 0)
                    {
                        break;
                    }

                }
            }
            return mapStructure.sandOnMap.Count.ToString();
            throw new NotImplementedException();
        }
       
        
    }

}
