using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Day1
{
    class Program
    {
        const string fileName = "Day1.txt";
        static void Main(string[] args)
        {
            Program programInstance = new Program();
            string[] fileText = programInstance.GetFile(fileName);
            int mostCaloriesElf = 0;
            int currentCaloriesElf = 0;
            //Storing three elves with most calories
            List<int> topThree = new List<int>();
            int topCalories = 0;


            foreach (string line in fileText)
            {


                if (line == string.Empty)
                {
                    //First part of puzzle
                    if (currentCaloriesElf > mostCaloriesElf)
                    {
                        mostCaloriesElf = currentCaloriesElf;

                    }


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

            Console.WriteLine("Most calories: " + mostCaloriesElf);
            foreach (int item in topThree)
            {
                topCalories += item;

            }
            Console.WriteLine("Top three total calories: " + topCalories);

        }
        public string[] GetFile(string fileName)
        {
            string projectDir = Environment.CurrentDirectory;
            string parentDir = Directory.GetParent(projectDir).Parent.Parent.Parent.FullName;
            string pathToFile = $"{parentDir}\\{fileName}";
            return File.ReadAllLines(pathToFile);
        }

    }

}