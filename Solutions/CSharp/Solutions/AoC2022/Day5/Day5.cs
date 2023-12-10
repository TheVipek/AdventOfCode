using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day5
{
	public class Day : BaseDay
	{
        public Dictionary<int, List<char>> generateStacks(ref int startingIndex)
        {
            Dictionary<int, List<char>> stacks = new Dictionary<int, List<char>>();
            for (int i = 0; i < inputContent.Length; i++)
            {
                int stackForValue = 0;
                //Checks whether its number in ascii
                if (inputContent[i][1] >= 48 && inputContent[i][1] <= 57)
                {
                    // At i index appeared number ,so next would be one that interests us
                    startingIndex = i+1;
                    break;
                }
                for (int j = 1; j < inputContent[i].Length; j += 4)
                {
                    if (inputContent[i][j] != ' ')
                    {
                        // if key doesnt exist lets create new
                        if (stacks.ContainsKey(stackForValue)) stacks[stackForValue].Insert(0,inputContent[i][j]);
                        else stacks.Add(stackForValue, new List<char>() { inputContent[i][j] });
                        //Console.WriteLine($"Added {inputContent[i][j]} to {stacks[stackForValue].Count}");

                    }
                    stackForValue += 1;
                }
            }
            return stacks;
        }
        public void ShowStacks(Dictionary<int, List<char>> stacks)
        {

            int maxAmount = stacks.OrderByDescending(x => x.Value.Count).First().Value.Count;
            Console.WriteLine(maxAmount);
            while (true)
            {
                for(int i = 0; i < stacks.Count; i++)
                {
                    if (stacks[i].ElementAtOrDefault(maxAmount - 1) != default)
                    {

                        Console.Write("{0,3}", $"[{stacks[i][maxAmount - 1]}]");
                    }
                    else
                    {
                        Console.Write($"{new String(' ',3)}");
                    }

                }
                maxAmount -= 1;
                Console.Write("\n");
                if (maxAmount == 0)
                {
                    
                    for(int i = 0; i<stacks.Count; i++)
                    {
                        Console.Write($" {i+1} ");
                    }
                    break;
                }
            }
            Console.WriteLine();

        }
        public void OrderCheck(Dictionary<int, List<char>> stacks, int startingIndex = 0, bool keepOrder = false)
        {
           // Console.WriteLine($"startinIndex: {startingIndex} : inputContent Amount: {inputContent.Length}");
            for(int i = startingIndex; i < inputContent.Length; i++)
            {
                List<int> orders = new List<int>();
                string[] separators = new string[] { "move", "from", "to"};
                string[] inputContentOrder = inputContent[i].Split(separators,StringSplitOptions.None);
                foreach (var item in inputContentOrder)
                {
                    if(item != string.Empty)
                    {
                        int value = int.Parse(item.ToString());
                        orders.Add(value);

                    }
                }
                if (orders.Count == 3)
                {
                    //Console.WriteLine($"elementsAmount:{orders[0]} fromStackIdx:{orders[1]} toStackIdx:{orders[2]}");
                    if (keepOrder == false) ElementMoveFTTB(stacks, orders[0], orders[1], orders[2]);
                    else ElementMoveFTTT(stacks, orders[0], orders[1], orders[2]);
                }

            }
        }
        /// <summary>
        /// Element Move From Top To Bottom is moving elements from top to bottom "disordering" them
        /// </summary>
        /// <param name="stacks">Dict containing all stacks</param>
        /// <param name="elementsAmount">how much elements from stack</param>
        /// <param name="fromStackIdx">from stack</param>
        /// <param name="toStackIdx">to stack</param>
        public void ElementMoveFTTB(Dictionary<int, List<char>> stacks,int elementsAmount,int fromStackIdx,int toStackIdx)
        {

            //Reducing them by one ,beacuse in file they're not written from 0 like indexes
            fromStackIdx -= 1;
            toStackIdx -= 1;

            while(elementsAmount > 0)
            {
                int elementIdx = stacks[fromStackIdx].Count - 1;
                char element = stacks[fromStackIdx][elementIdx];
                stacks[fromStackIdx].RemoveAt(elementIdx);
                stacks[toStackIdx].Add(element);
                elementsAmount -= 1;
                
            }
            //ShowStacks(stacks);


        }
        /// <summary>
        /// Element Move From Top To Top is moving elements from top to top ,leaving them in the same order
        /// </summary>
        /// <param name="stacks"></param>
        /// <param name="elementsAmount"></param>
        /// <param name="fromStackIdx"></param>
        /// <param name="toStackIdx"></param>
        public void ElementMoveFTTT(Dictionary<int, List<char>> stacks, int elementsAmount, int fromStackIdx, int toStackIdx)
        {
            fromStackIdx -= 1;
            toStackIdx -= 1;
            Stack<char> elements = new Stack<char>();
            while(elementsAmount > 0)
            {
                int elementIdx = stacks[fromStackIdx].Count - 1;
                elements.Push(stacks[fromStackIdx][elementIdx]);
                stacks[fromStackIdx].RemoveAt(elementIdx);
                elementsAmount -= 1;

            }
            while (elements.Count > 0)
            {
                stacks[toStackIdx].Add(elements.Pop());

            }
            //ShowStacks(stacks);
        }
        public string TopElements(Dictionary<int, List<char>> stacks)
        {
            string topElements = string.Empty;
            for (int i = 0; i < stacks.Count; i++)
            {
                topElements += stacks[i][stacks[i].Count - 1];

            }
            return topElements;
        }
        public override string FirstPart()
        {
            int startingIndex = 0;
            Dictionary<int, List<char>> stacks = generateStacks(ref startingIndex);
            //ShowStacks(stacks);
            OrderCheck(stacks, startingIndex);
            //ShowStacks(stacks);
            return TopElements(stacks);

        }
        public override string SecondPart()
        {
            int startingIndex = 0;
            Dictionary<int, List<char>> stacks = generateStacks(ref startingIndex);
            //ShowStacks(stacks);
            OrderCheck(stacks, startingIndex, true);
            //ShowStacks(stacks);
            return TopElements(stacks);
        }
    }
}
