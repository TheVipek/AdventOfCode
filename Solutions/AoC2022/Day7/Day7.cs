using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day7
{
    public class Day : BaseDay
    {

        public override string FirstPart()
        {
            List<KeyValuePair<string,int>>directoryPath = new List<KeyValuePair<string, int>>();
            //max size of directory
            int maxSize = 100000;
            //All directories size
            int DirsSize = 0;
            foreach (var input in inputContent)
            {
                string[] _input = input.Split(' ');
                if(_input[0] == "$")
                {

                    if (_input[1] == "cd")
                        //No need to do anything,just skip rest of loop
                        if (_input[2] == "/") continue;
                        //We're moving out ,so there's need to remove element at last index
                        //and skipping rest of loop
                        else if (_input[2] == "..")
                        {
                            if (directoryPath.Count > 0)
                            {
                                //Check if its not bigger than defined maxSize
                                if (directoryPath[directoryPath.Count() - 1].Value <= maxSize) DirsSize += directoryPath[directoryPath.Count() - 1].Value;
                                directoryPath.RemoveAt(directoryPath.Count() - 1);
                            }
                            continue;
                        }
                        //If its not cd or / or .. ,then it must be moving to X directory 
                        else
                        {
                            directoryPath.Add(new KeyValuePair<string, int>(_input[2], 0));
                            continue;
                        }
                    //No need to do anything,just skip rest of loop
                    else if (_input[1] == "ls") continue;
                }
                //If it's not starting with $,then it must be displaying informations after usage of ls command
                else 
                {
                    if (_input[0][0] >= '0' && _input[0][0] <= '9')
                    {
                        for(int i=0;i<directoryPath.Count;i++)
                        {
                            directoryPath[i] = new KeyValuePair<string,int>(directoryPath[i].Key,directoryPath[i].Value+int.Parse(_input[0]));
                        }
                    }
                }

                
               
            }
            // --IF we didn't end in root directory,then there should be one element left in directoryPath
            foreach (var k in directoryPath)
            {
                if (k.Value <= maxSize) DirsSize += k.Value;
            }
            Console.WriteLine($"Dirs Size:{DirsSize}");
            return DirsSize.ToString();
        }
        public override string SecondPart()
        {
            List<KeyValuePair<string, int>> directoryPath = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> allDirectories = new List<KeyValuePair<string, int>>();
            //All directories size
            int totalDiskSpace = 70000000;
            int totalSize = 0;
            foreach (var input in inputContent)
            {
                string[] _input = input.Split(' ');
                if (_input[0] == "$")
                {

                    if (_input[1] == "cd")
                        //No need to do anything,just skip rest of loop
                        if (_input[2] == "/") continue;
                        //We're moving out ,so there's need to remove element at last index
                        //and skipping rest of loop
                        else if (_input[2] == "..")
                        {
                            if (directoryPath.Count > 0)
                            {
                                //Check if its not bigger than defined maxSize
                                allDirectories.Add(directoryPath[directoryPath.Count() - 1]);
                                directoryPath.RemoveAt(directoryPath.Count() - 1);
                            }
                            continue;
                        }
                        //If its not cd or / or .. ,then it must be moving to X directory 
                        else
                        {
                            directoryPath.Add(new KeyValuePair<string, int>(_input[2], 0));
                            continue;
                        }
                    //No need to do anything,just skip rest of loop
                    else if (_input[1] == "ls") continue;
                }
                //If it's not starting with $,then it must be displaying informations after usage of ls command
                else
                {
                    if (_input[0][0] >= '0' && _input[0][0] <= '9')
                    {
                        for (int i = 0; i < directoryPath.Count; i++)
                        {
                            directoryPath[i] = new KeyValuePair<string, int>(directoryPath[i].Key, directoryPath[i].Value + int.Parse(_input[0]));
                        }
                    }
                }



            }
            // --IF we didn't end in root directory,then there should be one element left in directoryPath
            foreach (var k in directoryPath)
            {
                allDirectories.Add(k);
            }
            foreach (var k in allDirectories)
            {
                Console.WriteLine(k);
                totalSize += k.Value;
            }
            foreach (var k in allDirectories)
            {
                //if(k.Value > )
            }
            Console.WriteLine($"Space remaining:{totalDiskSpace - totalSize}");
            Console.WriteLine($"Total Size:{totalSize}");
            return totalSize.ToString();
        }

    }
}
