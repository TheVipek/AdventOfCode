using System;
using System.Linq;
namespace AoC2022.Day4 
{
    public class Day : BaseDay
    {
        public override string FirstPart()
        {
            int FullContainPairs = 0;
            foreach (var pairs in inputContent)
            {
                int[] _pairs = pairs.Split(',', '-').Select(int.Parse).ToArray();
                (int, int) firstPair = (_pairs[0], _pairs[1]);
                (int, int) secondPair = (_pairs[2], _pairs[3]);
                //if they're the same then other is containing other
                if (firstPair == secondPair) FullContainPairs += 1;
                // firstPair starts before or at the same as second and ends after or at the same as second, which means that its fully containing second
                else if (firstPair.Item1 <= secondPair.Item1 && firstPair.Item2 >= secondPair.Item2) FullContainPairs += 1;
                //Like in previous IF statement but reversed
                else if (secondPair.Item1 <= firstPair.Item1 && secondPair.Item2 >= firstPair.Item2) FullContainPairs += 1;
            }
            return FullContainPairs.ToString();
        }
        public override string SecondPart()
        {
            int OverlappingPairs = 0;
            foreach (var pairs in inputContent)
            {
                int[] _pairs = pairs.Split(',', '-').Select(int.Parse).ToArray();
                (int, int) firstPair = (_pairs[0], _pairs[1]);
                (int, int) secondPair = (_pairs[2], _pairs[3]);
                //they're overlapping each other
                if (firstPair == secondPair) OverlappingPairs += 1;
                // if starting firstPair item1 is lower or equal to second pair item1
                // and firstPair item2 is higher or equal to secondPair item1 means that they're overlapping
                else if (firstPair.Item1 <= secondPair.Item1 && firstPair.Item2 >= secondPair.Item1) OverlappingPairs += 1;
                //Like in previous IF statement but reversed
                else if (secondPair.Item1 <= firstPair.Item1 && secondPair.Item2 >= firstPair.Item1) OverlappingPairs += 1;
            }
            return OverlappingPairs.ToString();
        }
    }

}
