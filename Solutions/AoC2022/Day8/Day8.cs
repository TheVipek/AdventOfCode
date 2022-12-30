using System;

namespace AoC2022.Day8
{
    public class Day : BaseDay
    {
        public override string FirstPart()
        {
            int visibleTrees = 0;


            for (int i = 0; i < inputContent.Length; i++)
            {
                if (i == 0 || i == inputContent.Length - 1)
                {
                    visibleTrees += inputContent[i].Length;
                    continue;
                }
                else
                {
                    visibleTrees += 2;
                }

                //we dont want do include first and last index,beacuse they're already counted to visible trees
                for (int j = 1; j < inputContent[i].Length - 1; j++)
                {
                    //Console.WriteLine($"Checking {inputContent[i][j]}");
                    //if bigger than on left / right / top / down 
                    int currentLeft = j - 1;
                    int currentRight = j + 1;
                    int currentTop = i - 1;
                    int currentBottom = i + 1;
                    if (inputContent[i][j] > inputContent[i][currentLeft])
                    {
                        //we assume that tree is visible
                        bool visible = true;
                        while (currentLeft >= 0)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} leftSide:{inputContent[i][currentLeft]}");
                            if (inputContent[i][j] <= inputContent[i][currentLeft])
                            {
                                visible = false;
                                break;
                            }
                            currentLeft -= 1;
                        }
                        if (visible)
                        {
                            visibleTrees += 1;
                            continue;
                        }
                    }
                    
                    if(inputContent[i][j] > inputContent[i][currentRight])
                    {
                        //we assume that tree is visible
                        bool visible = true;
                        //Console.WriteLine($"Checking {inputContent[i][j]} rightSide:{inputContent[i][currentRight]}");

                        while (currentRight <= inputContent[i].Length - 1)
                        {
                            if (inputContent[i][j] <= inputContent[i][currentRight])
                            {
                                visible = false;
                                break;
                            }
                            currentRight += 1;
                        }
                        if(visible)
                        {
                            visibleTrees += 1;
                            continue;
                        }
                    }

                    if(inputContent[i][j] > inputContent[currentTop][j])
                    {
                        //we assume that tree is visible
                        bool visible = true;
                        while (currentTop >= 0)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} topSide:{inputContent[currentTop][j]}");
                            if (inputContent[i][j] <= inputContent[currentTop][j])
                            {
                                visible = false;
                                break;
                            }
                            currentTop -= 1;
                        }
                        if(visible)
                        {
                            visibleTrees += 1;
                            continue;
                        }
                    }

                    if(inputContent[i][j] > inputContent[currentBottom][j])
                    {
                        //we assume that tree is visible
                        bool visible = true;
                        while (currentBottom <= inputContent[i].Length - 1)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} bottomSide:{inputContent[currentBottom][j]}");
                            if (inputContent[i][j] <= inputContent[currentBottom][j])
                            {
                                visible = false;
                                break;
                            }
                            currentBottom += 1;
                        }
                        if (visible)
                        {
                            visibleTrees += 1;
                            continue;
                        }
                    }
                }
                
            }
            //Console.WriteLine(visibleTrees);
            return visibleTrees.ToString();

        }
        public override string SecondPart()
        {
            int maxVisible = 0;

            for (int i = 1; i < inputContent.Length - 1; i++)
            {
                
                for (int j = 1; j < inputContent[i].Length - 1; j++)
                {
                    //Console.WriteLine($"Checking {inputContent[i][j]}");
                    //if bigger than on left / right / top / down 
                    int currentLeft = j - 1;
                    int leftVisible = 1;
                    int currentRight = j + 1;
                    int rightVisible = 1;
                    int currentTop = i - 1;
                    int topVisible = 1;
                    int currentBottom = i + 1;
                    int bottomVisible = 1;

                    int currentVisible = 0;


                    if (inputContent[i][j] > inputContent[i][currentLeft])
                    {
                        while (currentLeft > 0)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} leftSide:{inputContent[i][currentLeft]}");
                            if (inputContent[i][j] <= inputContent[i][currentLeft])
                            {
                                break;
                            }
                            currentLeft -= 1;
                            leftVisible += 1;
                        }
                    }

                    if (inputContent[i][j] > inputContent[i][currentRight])
                    {

                        //Console.WriteLine($"Checking {inputContent[i][j]} rightSide:{inputContent[i][currentRight]}");

                        while (currentRight < inputContent[i].Length - 1)
                        {
                            if (inputContent[i][j] <= inputContent[i][currentRight])
                            {
                                break;
                            }
                            currentRight += 1;
                            rightVisible += 1;
                        }
                    }

                    if (inputContent[i][j] > inputContent[currentTop][j])
                    {

                        while (currentTop > 0)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} topSide:{inputContent[currentTop][j]}");
                            if (inputContent[i][j] <= inputContent[currentTop][j])
                            {
                                break;
                            }
                            currentTop -= 1;
                            topVisible += 1;
                        }
                    }

                    if (inputContent[i][j] > inputContent[currentBottom][j])
                    {
                        while (currentBottom < inputContent[i].Length - 1)
                        {
                            //Console.WriteLine($"Checking {inputContent[i][j]} bottomSide:{inputContent[currentBottom][j]}");
                            if (inputContent[i][j] <= inputContent[currentBottom][j])
                            {
                                break;
                            }
                            currentBottom += 1;
                            bottomVisible += 1;
                        }
                    }
                    //Console.WriteLine();
                    //Console.WriteLine($"row:{i+1} column:{j+1} - {inputContent[i][j]}");
                    //Console.WriteLine($"leftVisible: {leftVisible} rightVisible:{rightVisible} topVisible:{topVisible} bottomVisible:{bottomVisible}");
                    currentVisible = leftVisible * rightVisible * topVisible * bottomVisible;
                    //Console.WriteLine(currentVisible);
                    if (currentVisible > maxVisible) maxVisible = currentVisible;
                }
                
            }
            //Console.WriteLine($"MaxVisible:{maxVisible}");
            return maxVisible.ToString();
        }
    }
}
