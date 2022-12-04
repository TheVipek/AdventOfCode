using System;
using System.Text;

namespace AoC2022.Day2

{
    class Day2 : BaseDay
    {
        // WIN - DRAW - LOSE values
        int[] gameRewards = new int[3] { 6, 3, 0 };
        // SCISSORS - PAPER - ROCK values
        int[] shapeRewards = new int[3] { 3, 2, 1 };
        // A B C enemyPlay
        // X Y Z myPlay

        public override string FirstPart()
        {
            int score = 0;
            foreach (string line in inputContent)
            {
                int reward = 0;
                string lineContent = line.Replace(" ", "");
                int opponentPlay = lineContent[0] + 23; //get what play it would be if opponent would play Y X Z
                int myPlay = lineContent[1];

                //Play shape
                if (myPlay == 90) reward += shapeRewards[0]; // SCISSORS
                else if (myPlay == 89) reward += shapeRewards[1]; // PAPER
                else reward += shapeRewards[2]; // ROCK

                //Game
                if (myPlay == opponentPlay) reward += gameRewards[1]; // DRAW 

                else if (myPlay > opponentPlay)
                {
                    if (myPlay - opponentPlay == 1) reward += gameRewards[0]; // WIN
                }
                else if (myPlay < opponentPlay)
                {
                    if (myPlay - opponentPlay == -2) reward += gameRewards[0]; // WIN
                }
                score += reward;
            }

            return score.ToString();
        }

        public override string SecondPart()
        {
            int score = 0;
            foreach (string line in inputContent)
            {
                int reward = 0;
                string lineContent = line.Replace(" ", "");
                int opponentPlay = lineContent[0] + 23; //get what play it would be if opponent would play Y X Z
                int myPlay = lineContent[1];

                //intention           V
                if (myPlay == 90) // WIN
                {
                    reward += gameRewards[0];                               // => = lose vs
                    if (opponentPlay == 90) reward += shapeRewards[2]; // Scissors => Rock
                    else if (opponentPlay == 89) reward += shapeRewards[0]; // Paper => Scissors
                    else reward += shapeRewards[1]; // Rock => Paper
                }
                //intention                V
                else if (myPlay == 89) // DRAW
                {
                    reward += gameRewards[1];   // Keep same as opponent ,beacuse its DRAW
                    if (opponentPlay == 90) reward += shapeRewards[0];
                    else if (opponentPlay == 89) reward += shapeRewards[1];
                    else reward += shapeRewards[2];
                }
                //intention  V
                else     // LOSE
                {
                    reward += gameRewards[2];                           // => = win vs
                    if (opponentPlay == 90) reward += shapeRewards[1]; // Scissors => Paper
                    else if (opponentPlay == 89) reward += shapeRewards[2]; // Paper => Rock
                    else reward += shapeRewards[0]; // Rock => Scissors
                }
                score += reward;

            }

            return score.ToString();
        }
    }
}
