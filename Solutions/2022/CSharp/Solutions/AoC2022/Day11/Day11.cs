using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
namespace AoC2022.Day11
{
    public class Monkey
    {
        public Queue<long> Items { get; set; }
        private long currentItem;
        public string[] Operation { get; set; }
        public long DivisibleBy { get; set; }
        public bool IsWorryLevelDivided { get; }
        public int TestPassedMonkey { get; set; }
        public int TestFailedMonkey { get; set; }
        public int InspectedTimes { get; set; }
        public Monkey(bool _IsWorryLevelDivided) 
        {
            IsWorryLevelDivided = _IsWorryLevelDivided;
        }
        public void CurrentItemInspect()
        {
            InspectedTimes += 1;
            //Using first one ,and pushing it to end of list
            currentItem = Items.Dequeue();
            Items.Enqueue(currentItem);
            Console.WriteLine($"Monkey inspects an item with a worry level of {currentItem}");
            switch (Operation[0])
            {
                case "+":
                    if (Operation[1] == "old") currentItem += currentItem;
                    else currentItem += long.Parse(Operation[1]);
                    Console.WriteLine($"Worry level is increased by {Operation[1]} to {currentItem}.");
                    break;
                case "*":
                    if (Operation[1] == "old") currentItem *= currentItem;
                    else currentItem *= long.Parse(Operation[1]);
                    Console.WriteLine($"Worry level is multipied by {Operation[1]} to {currentItem}.");
                    break;
                default:
                    break;
            }
            if (IsWorryLevelDivided) currentItem /= 3;
           Console.WriteLine($"Monkey gets bored with item. Worry level is divided by 3 to {currentItem}.");
        }
        /// <summary>
        /// Returns 1 if condition is reached and 0 if not
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public void PassToDecision(out Tuple<int,long> decisionTuple) 
        {
            CurrentItemInspect();
            if (currentItem % DivisibleBy == 0)
            {
                Console.WriteLine($"Current worry level is divisible by {DivisibleBy}.");
                Console.WriteLine($"Item with worry level {currentItem} is thrown to monkey {TestPassedMonkey}.");
                decisionTuple = new Tuple<int, long>(TestPassedMonkey, currentItem);
            }
            else
            {
                Console.WriteLine($"Current worry level is not divisible by {DivisibleBy}.");
                Console.WriteLine($"Item with worry level {currentItem} is thrown to monkey {TestFailedMonkey}.");
                decisionTuple = new Tuple<int, long>(TestFailedMonkey, currentItem);
            }
        } 

        public void CatchItem(long item)
        {
            Items.Enqueue(item);
        }
    }
        public class Day : BaseDay
        {
        public override string FirstPart()
        {
            //List<Monkey> monkeys = GetMonkeys(true);
            //PlayRounds(ref monkeys, 20);
            //long[] mostActive = GetMostActive(monkeys);
            ////Console.WriteLine($"Most active : {mostActive[0]} {mostActive[1]}");
            //long monkeyBusiness = mostActive.Aggregate((long)1, (x, y) => (x * y));
            ////Console.WriteLine($"MonkeyBusiness: {monkeyBusiness}");
            //return monkeyBusiness.ToString();
            return "";
        }

        public override string SecondPart()
        {
            List<Monkey> monkeys = GetMonkeys(false);
            PlayRounds(ref monkeys, 1000);
            int[] mostActive = GetMostActive(monkeys);
            Console.WriteLine($"Most active : {mostActive[0]} {mostActive[1]}");
            long monkeyBusiness = mostActive.Aggregate((long)1, (x, y) => (x * y));
            Console.WriteLine($"MonkeyBusiness: {monkeyBusiness}");
            return monkeyBusiness.ToString();
            throw new NotImplementedException();
        }

        public List<Monkey> GetMonkeys(bool areDivided)
        {
            List<Monkey> monkeys = new List<Monkey>();
            Monkey monkey = null;
            for (int i = 0; i < inputContent.Length; i++)
            {
                string[] item = Regex.Replace(inputContent[i], "[^0-9,*+-/]", "").Split(',');
                if (inputContent[i].Contains("Monkey")) monkey = new Monkey(areDivided);
                else if (inputContent[i].Contains("Starting items:")) monkey.Items = new Queue<long>(item.Select(long.Parse));
                else if (inputContent[i].Contains("Operation:"))
                { 
                    if(item[0][1..] == "") monkey.Operation = new string[2] { item[0][0].ToString(), "old" };
                    else monkey.Operation = new string[2] { item[0][0].ToString(), item[0][1..].ToString() };
                } 
                else if (inputContent[i].Contains("Test:")) monkey.DivisibleBy = long.Parse(item[0]);
                else if (inputContent[i].Contains("If true:")) monkey.TestPassedMonkey = int.Parse(item[0]);
                else if (inputContent[i].Contains("If false:"))
                {
                    monkey.TestFailedMonkey = int.Parse(item[0]);
                    monkeys.Add(monkey);
                } 
            }
            return monkeys;
        }
        public void PlayRounds(ref List<Monkey> monkeys, int amount)
        {
            for (int i = 1;i<=amount;i++)
            {
                Console.WriteLine($"Starting Round {i}");
                for (int j = 0; j < monkeys.Count; j++)
                {
                    Console.WriteLine($"Monkey {j}:");

                    for (int k = 0; k < monkeys[j].Items.Count; k++)
                    {
                        Tuple<int, long> monkeyDecision;
                        monkeys[j].PassToDecision(out monkeyDecision);
                        if(monkeyDecision != null) monkeys[monkeyDecision.Item1].CatchItem(monkeyDecision.Item2);
                    }
                    monkeys[j].Items.Clear();
                }
                foreach(var monkey in monkeys)
                {
                    Console.WriteLine($"Inspected Times:{monkey.InspectedTimes}"); 
                }
                Console.WriteLine($"Ending Round {i}");


            }
        }
        public int[] GetMostActive(List<Monkey> monkeys) => monkeys.Select(x => x.InspectedTimes).OrderByDescending(x => x).Take(2).ToArray();

    }
        

    }

