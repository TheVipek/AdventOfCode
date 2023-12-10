using System;
using System.IO;

namespace AoC
{
    public class FileReader
    {
        public string[] GetInput()
        {
            var type = GetType().Name;
            Console.WriteLine(type);
            string projectDir = AppDomain.CurrentDomain.BaseDirectory;
            string parentDir = Directory.GetParent(projectDir).Parent.Parent.Parent.FullName;
            Console.WriteLine(parentDir);
            //string pathToFile = $"{parentDir}\\{fileName}";
            return File.ReadAllLines(type);
        }
    }
}
