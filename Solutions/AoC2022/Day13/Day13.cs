using System;
using System.Collections;
using System.Collections.Generic;

namespace AoC2022.Day13

{
public class Day : BaseDay
{
        public override string FirstPart()
        {
            for(int i =0; i < inputContent.Length; i+=3)
            {
                List<object> list = ParseToData(inputContent[i]);
                Console.WriteLine("Gathered packet...");
                Console.Write('[');
                foreach (var item in list)
                {
                    Console.Write(item);
                }
                Console.WriteLine(']');

            }
            throw new NotImplementedException();

        }
        public override string SecondPart()
        {
            throw new NotImplementedException();
        }
        public List<object> ParseToData(string dataLine) 
        {
            List<object> packets = new List<object>();
            for (int i = 1; i < dataLine.Length - 1;i++)
            {
                if(dataLine[i] != ',')
                {
                    packets.Add(dataLine[i]);
                    Console.WriteLine(dataLine[i]);
                }
            }

            return packets;
        }
        //public int Comparer(List<object> firstList,List<object> secondList) 
        //{
            
        //}

    }
}
