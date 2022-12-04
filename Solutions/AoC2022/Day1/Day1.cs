using System;
using System.Collections.Generic;
using System.Linq;


namespace AoC2022.Day1
{
    public class Day1 : BaseDay
    {

        public override string FirstPart()
        {
            int mostCaloriesElf = 0;
            int currentCaloriesElf = 0;

            foreach (string line in inputContent)
            {


                if (line == string.Empty)
                {
                    //First part of puzzle
                    if (currentCaloriesElf > mostCaloriesElf) mostCaloriesElf = currentCaloriesElf;
                    currentCaloriesElf = 0;
                    //skipping rest loop
                    continue;
                }

                currentCaloriesElf += int.Parse(line);
            }

            return mostCaloriesElf.ToString();
        }
        public override string SecondPart()
        {
            int currentCaloriesElf = 0;
            //Storing three elves with most calories
            List<int> topThree = new List<int>();
            int topCalories = 0;


            foreach (string line in inputContent)
            {


                if (line == string.Empty)
                {
                    //Second part of puzzle
                    if (topThree.Count == 3)
                    {
                        int minIndex = topThree.Min(x => x);
                        if (currentCaloriesElf > minIndex)
                        {
                            topThree.Remove(minIndex);
                            topThree.Add(currentCaloriesElf);

                        }
                    }
                    else
                    {
                        topThree.Add(currentCaloriesElf);
                    }
                    currentCaloriesElf = 0;
                    //skipping rest loop
                    continue;
                }

                currentCaloriesElf += int.Parse(line);
            }

            foreach (int item in topThree) topCalories += item;
            return topCalories.ToString();
        }
    }
   

}
