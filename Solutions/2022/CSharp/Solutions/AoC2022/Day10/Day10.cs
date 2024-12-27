using System;
using System.Collections.Generic;
namespace AoC2022.Day10

{
    public class ClockCircuit
    {
        public int Cycle { get; set; }
        private int baseNextCycleChecker;
        public int NextCycleChecker { get; set; }
        public int CurrentCycleStrength { get; set; }
        public int TotalCyclesStrength { get; set; }
        public Dictionary<string,int> CpuSignals { get; } = new Dictionary<string, int>()
        {
            { "noop",1},
            { "addx",2 }
        };
        public ClockCircuit(int _nextCycleChecker)
        {
            Cycle = 0;
            CurrentCycleStrength = 1;
            TotalCyclesStrength = 0;
            baseNextCycleChecker = _nextCycleChecker;
            NextCycleChecker = baseNextCycleChecker;
        }
        public void Reset()
        {
            Cycle = 0;
            CurrentCycleStrength = 1;
            TotalCyclesStrength = 0;
            NextCycleChecker = baseNextCycleChecker;
        }
        public bool IsCycleCheckerTime() => _ = Cycle == NextCycleChecker;
        
        /// <summary>
        /// Calculate sent singal by CPU
        /// </summary>
        /// <param name="sentCPUSignal">String array that contains signal name and amount of cycles </param>
        public void CalculateSignalStrength(string[] sentCPUSignal)
        {
            Cycle += 1;
            if (IsCycleCheckerTime())
            {
                TotalCyclesStrength += (CurrentCycleStrength * Cycle);
                NextCycleChecker += 40;
            }
            //which means that its addx
            if (sentCPUSignal.Length == 2)
            {
                Cycle += 1;
                if (IsCycleCheckerTime())
                {
                    TotalCyclesStrength += (CurrentCycleStrength * Cycle);
                    NextCycleChecker += 40;
                }
                CurrentCycleStrength += int.Parse(sentCPUSignal[1]);
            }
        }

        /// <summary>
        /// Render image via signals sent by CPU
        /// </summary>
        /// <param name="sentCPUSignal">String array that contains signal name and amount of cycles</param>
        /// <param name="currentCRT">CRT index we want to update in crtsContainer</param>
        /// <param name="localCycle">Controlling position for CRT draw</param>
        /// <param name="litPixelsPosition"></param>
        /// <param name="crtsContainer"></param>
        public void RenderImage(string[] sentCPUSignal, ref int currentCRT, ref int localCycle, ref int litPixelsPosition, ref string[] crtsContainer)
        {
            Cycle += 1;
            localCycle += 1;
            //Console.WriteLine();
            //Console.WriteLine($"Start cycle {Cycle}: begin executing {sentCPUSignal[sentCPUSignal.Length-1]}");
            //Console.WriteLine($"During cycle {Cycle}: CRT draws pixel in position {localCycle - 1}");
            //Console.WriteLine($"Current CRT row: {crtsContainer[currentCRT]}");

            _ = ((localCycle - 1 >= litPixelsPosition) && (localCycle - 1 <= litPixelsPosition + 2)) ? crtsContainer[currentCRT] += "#" : crtsContainer[currentCRT] += ".";


            if (IsCycleCheckerTime())
            {
                currentCRT += 1;
                NextCycleChecker += 40;
                localCycle = 0;
            }

            //which means that its addx
            if (sentCPUSignal.Length == 2)
            {

                Cycle += 1;
                localCycle += 1;
                _ = ((localCycle - 1 >= litPixelsPosition) && (localCycle - 1 <= litPixelsPosition + 2)) ? crtsContainer[currentCRT] += "#" : crtsContainer[currentCRT] += ".";

                //Console.WriteLine();
                //Console.WriteLine($"Start cycle {Cycle}: begin executing {sentCPUSignal[sentCPUSignal.Length - 1]}");
                //Console.WriteLine($"During cycle {Cycle}: CRT draws pixel in position {localCycle - 1}");
                //Console.WriteLine($"Current CRT row: {crtsContainer[currentCRT]}");
                if (IsCycleCheckerTime())
                {
                    currentCRT += 1;
                    NextCycleChecker += 40;
                    localCycle = 0;
                }
                litPixelsPosition += int.Parse(sentCPUSignal[1]);
                //Console.WriteLine($"End of cycle  {Cycle}: finish executing {sentCPUSignal[1]} (Register X is now {litPixelsPosition})");
                //Console.Write($"Sprite position: ");
                //for (int i = 0; i < 40; i++)
                //{
                //    if (i >= litPixelsPosition && i <= litPixelsPosition + 2) Console.Write("#");
                //    else Console.Write(".");
                //}
                //Console.WriteLine();
            }
        }
        /// <summary>
        /// Print rendered image
        /// </summary>
        /// <param name="crtsContainer"></param>
        public void Draw(string[] crtsContainer)
        {
            for(int i=0;i<crtsContainer.Length;i++)
            {
                Console.WriteLine(crtsContainer[i]);
            }
        }
    }
	public class Day : BaseDay
	{
        
        public override string FirstPart()
        {
            ClockCircuit clockCircuit = new ClockCircuit(20);
            foreach (var input  in inputContent)
            {
                string[] sentCPUSignal = input.Split(' ');
                if (clockCircuit.CpuSignals.ContainsKey(sentCPUSignal[0]))
                {
                    clockCircuit.CalculateSignalStrength(sentCPUSignal);
                }
            }
            return clockCircuit.TotalCyclesStrength.ToString();

        }
        public override string SecondPart()
        {
            ClockCircuit clockCircuit = new ClockCircuit(40);

            // #(litPixel) are from 0 to 2 index
            int litPixelsPosition = 0;
            //current string that we're writing to
            int currentCRT = 0;
            // string container
            string[] crtsContainer = new string[6];
            // local cycle for position
            int localCycle = 0;
            foreach (var input in inputContent)
            {
                string[] sentCPUSignal = input.Split(' ');
                if (clockCircuit.CpuSignals.ContainsKey(sentCPUSignal[0]))
                {
                    clockCircuit.RenderImage(sentCPUSignal,ref currentCRT, ref localCycle, ref litPixelsPosition, ref crtsContainer);
                }

            }
            clockCircuit.Draw(crtsContainer);
            return "Generating done... (See above)";
        }
        
    }

}
