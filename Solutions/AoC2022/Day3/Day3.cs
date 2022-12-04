using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day3
{
    public class Day3 : BaseDay
    {
        public override string FirstPart()
        {
            int sum = 0;
            foreach (string rucksack in inputContent)
            {
                string firstCompString = rucksack.Substring(0, (rucksack.Length / 2));// UP TO HALF
                List<char> firstCompList = new List<char>();
                firstCompList.AddRange(firstCompString);

                
                string secondCompString = rucksack.Substring((rucksack.Length / 2), (rucksack.Length / 2)); // FROM HALF TO END
                List<char> secondCompList = new List<char>();
                secondCompList.AddRange(secondCompString);


                char lookingFor;
                while (true)
                {
                    lookingFor = firstCompList[0];
                    IEnumerable<char> firstCompartmentCount = firstCompList.Where(ctx => ctx == lookingFor);
                    IEnumerable<char> secondCompartmentCount = secondCompList.Where(ctx => ctx == lookingFor);
                    if (firstCompartmentCount.Count() >= 1 && secondCompartmentCount.Count() >= 1)
                    {
                        int charValue = lookingFor;
                        if(charValue >= 65 && charValue <= 90)
                        {
                            // Priorities are going from 1 to x ,instead of from 0 ,so im adding 1 as it were list count
                            charValue += 26; // increase charValue by 26 ,beacuse of it's priority going from 26 to 52
                            charValue = (Math.Abs(65 - charValue) + 1); 
                            //Console.WriteLine($"Char {lookingFor} value is equal to {charValue}");
                            sum += charValue;
                        }
                        else
                        {
                            // Priorities are going from 1 to x ,instead of from 0 ,so im adding 1 as it were list count
                            charValue = Math.Abs(97 - charValue) +1;
                            //Console.WriteLine($"Char {lookingFor} value is equal to {charValue}");
                            sum += charValue;
                        }
                        
                        break;
                    }

                    if (firstCompartmentCount.Count() > 0) firstCompList.RemoveAll(ctx => ctx == lookingFor);
                    if (secondCompartmentCount.Count() > 0) secondCompList.RemoveAll(ctx => ctx == lookingFor);


                }

            }
            return sum.ToString();

        }
        public override string SecondPart()
        {
            throw new NotImplementedException();
        }
    }
}
