using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day3
{
    public class Day : BaseDay
    {
        public int getItemValue(char item) 
        {
            if (item >= 65 && item <= 90)
            {
                // Priorities are going from 1 to x ,instead of from 0 ,so im adding 1 as it were list count
                return (Math.Abs(65 - item) + 1) + 26; // increase charValue by 26 ,beacuse of it's priority going from 26 to 52
                                                       //Console.WriteLine($"Char {lookingFor} value is equal to {charValue}");

            }
            return Math.Abs(97 - item) + 1;
        }
        public override string FirstPart()
        {
            int sum = 0;
            foreach (string rucksack in inputContent)
            {
                char[] firstCompChar = rucksack.Substring(0, (rucksack.Length / 2)).Distinct().ToArray();
                char[] secondCompChar = rucksack.Substring((rucksack.Length / 2), (rucksack.Length / 2)).Distinct().ToArray();
                char item = firstCompChar.Intersect(secondCompChar).First();
                sum += getItemValue(item);   
            }
            return sum.ToString();

        }
        public override string SecondPart()
        {
            int sum = 0;
            for (int i = 0; i < inputContent.Length; i += 3)
            {

                char[] lookingForItems = inputContent[i].Distinct().ToArray();
                                                        
                foreach (var item in lookingForItems)
                {
                    if (inputContent[i + 1].ToCharArray().Where(ctx => ctx == item).Any() && inputContent[i + 2].ToCharArray().Where(ctx => ctx == item).Any())
                    {
                        sum += getItemValue(item);
                        break;
                    }

                }
            }
            return sum.ToString();
        }
    }
}
