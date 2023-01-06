using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace AoC2022.Day14
{
    public class Sand
    {
        public int[] Position { get; private set; } 
        public bool IsRested { get; set; } = false;
        
        public Sand(int[] position)
        {
            Position = position;
        }
        public void SetPositionX(int x) => Position[0] = x;
        public void SetPositionY(int y) => Position[1] = y;  
        public void SetPosition(int[] newPosition) => Position = newPosition;
    }
    public class MapStructure
    {
        public List<int[]> elementsOnMap = new List<int[]>();
        public List<Sand> sandOnMap = new List<Sand>();

        // [0] lowest range, [1] highest range
        public int originSandX { get; } = 500;
        public int originSandY { get; } = 0;
        public int[] StructureRangeX { get; private set; }

        public MapStructure() { }
        public void GeneratingProcess(int elementPositionX, int elementPositionY)
        {
            if (CanGenerate(elementPositionX,elementPositionY)) 
            { 
                int[] elementPosition = new int[2] { elementPositionX, elementPositionY };
                elementsOnMap.Add(elementPosition);
               // Console.WriteLine($"Generation at: {elementPosition[0]}:{elementPosition[1]}");
            } 
        }
        /// <summary>
        /// Return true if there's no xPos and yPos element in elementsOnMap
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <returns></returns>
        public bool CanGenerate(int xPos, int yPos) => !elementsOnMap.Any(x => x[0] == xPos && x[1] == yPos);
        /// <summary>
        /// Smaller x position and the biggest x
        /// </summary>
        public void SetAbyssPositions()
        {
            int[] abyss = new int[2] { elementsOnMap.Min(x => x[0]), elementsOnMap.Max(x => x[0]) };
            //Console.WriteLine($"Abyss at: {abyss[0]} , {abyss[1]}");
            StructureRangeX = abyss;
        }
        /// <summary>
        /// Get line of scan and applies it to mapStructure
        /// </summary>
        /// <param name="scan"></param>
        public void SetPartOfStructure(string[] scan)
        {
            int[] lastPosition = null;
            //i+=2 beacuse we want to catch x and y and they're on different indexes
            for (int i = 0; i < scan.Length; i += 2)
            {
                int[] currentPosition = new int[2]
                {
                    int.Parse(scan[i]),
                    int.Parse(scan[i+1])
                };
                if (lastPosition == null)
                {

                    lastPosition = currentPosition;
                    //Console.Write($"{lastPosition[0]}:{lastPosition[1]} - ...\n");
                    GeneratingProcess(lastPosition[0], lastPosition[1]);
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
        public void FillBeetwen(int[] lastPosition, int[] currentPosition)
        {

            //difference between last and current pos
            int differenceInPositionX = lastPosition[0] - currentPosition[0];
            int differenceInPositionY = lastPosition[1] - currentPosition[1];
            //Console.WriteLine($"DifferenceX:{differenceInPositionX} DifferenceY:{differenceInPositionY}");


            //We have to go left
            if (differenceInPositionX > 0)
            {
                while (differenceInPositionX > 0)
                {
                    GeneratingProcess(lastPosition[0] - differenceInPositionX, lastPosition[1]);
                    differenceInPositionX--;
                }
            }
            //We have to go right
            else if (differenceInPositionX < 0)
            {
                while (differenceInPositionX < 0)
                {
                    GeneratingProcess(lastPosition[0] - differenceInPositionX, lastPosition[1]);
                    differenceInPositionX++;

                }
            }
            //We have to go down
            if (differenceInPositionY > 0)
            {
                while (differenceInPositionY > 0)
                {
                    GeneratingProcess(lastPosition[0], lastPosition[1] - differenceInPositionY);
                    differenceInPositionY--;

                }
            }
            //We have to go up
            else if (differenceInPositionY < 0)
            {
                while (differenceInPositionY < 0)
                {
                    GeneratingProcess(lastPosition[0], lastPosition[1] - differenceInPositionY);
                    differenceInPositionY++;

                }
            }


        }
        //
        public Sand SpawnSand()
        {
            Sand sand = new(new int[2] {originSandX,originSandY});
            //Console.WriteLine($"Sand base position:{sand.Position[0]}:{sand.Position[1]}");
            MoveSand(ref sand);
            //Which means that its in rest state
            if (sand != null && sand.IsRested == true)
            {
               // Console.WriteLine("Sand is rest-stated");
                sandOnMap.Add(sand);
                elementsOnMap.Add(sand.Position);

                return sand;

            }
            return null;
        }

        public object MoveSand(ref Sand sand)
        {
            //Console.WriteLine($"Sand is trying to achieve new position:[{positionMoveTo[0]}:{positionMoveTo[1]}]");

            //Check if sand position isn't out of 'structure bounds'
            if (sand.Position[0] < StructureRangeX[0] || sand.Position[0] > StructureRangeX[1])
            {
                //Console.WriteLine($"Sand is falling into abyss due to new position [{sand.Position[0]}:{sand.Position[1]}]" +
                //    $"Abyss range: lower than: {StructureRangeX[0]},higher than:{StructureRangeX[1]}");
                sand = null;
                return null;
            }
            //check if we can move down
            if (CanGenerate(sand.Position[0], sand.Position[1] + 1))
            {
                sand.SetPositionY(sand.Position[1] + 1);
                return MoveSand(ref sand);
            }
            //check if we can move down left
            if (CanGenerate(sand.Position[0] - 1, sand.Position[1] + 1))
            {
                sand.SetPositionX(sand.Position[0] - 1);
                sand.SetPositionY(sand.Position[1] + 1);

                return MoveSand(ref sand);
            }
            //check if we can move down right
            if (CanGenerate(sand.Position[0] + 1, sand.Position[1] + 1))
            {
                sand.SetPositionX(sand.Position[0] + 1);
                sand.SetPositionY(sand.Position[1] + 1);

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
            while (true)
            {
                Sand sandSpawned = mapStructure.SpawnSand();
                if (sandSpawned == null || sandSpawned.IsRested == false) break;
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
            throw new NotImplementedException();
        }
       
        
    }

}
