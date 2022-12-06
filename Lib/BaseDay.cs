
using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
public abstract class BaseDay
{
    const string baseSolutionsDir = "Solutions";
    const string baseInputExt = ".txt";
    protected string[] baseInfo;
    protected string[] inputContent;
    Stopwatch firstPartExecutionTime;
    Stopwatch secondPartExecutionTime;

    public virtual string[] ReadInput(string Filename = "")
    {
        return File.ReadAllLines(GetInput(Filename));

    }
    protected virtual string GetInput(string Filename = "")
    {
        string inputPath;
        baseInfo = GetType().FullName.Split(".");
        string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        string dayDir = Path.Combine(baseDir, baseSolutionsDir, baseInfo[0], baseInfo[1]);
        if(Filename != string.Empty) inputPath = dayDir + $"\\{Filename}{baseInputExt}";
        else inputPath = dayDir + $"\\{baseInfo[1]}{baseInputExt}";
        return inputPath;

    }

    public virtual void Run(string Filename = "")
    {
        inputContent = ReadInput(Filename);
        Console.WriteLine($"Solution {baseInfo[0]} - {baseInfo[1]}");
        firstPartExecutionTime = new Stopwatch();
        firstPartExecutionTime.Start();
        Console.WriteLine($"First part: {FirstPart()}");
        firstPartExecutionTime.Stop();
        Console.WriteLine($"First part took {firstPartExecutionTime.ElapsedMilliseconds}ms.");

        secondPartExecutionTime = new Stopwatch();
        secondPartExecutionTime.Start();
        Console.WriteLine($"Second part: {SecondPart()}");
        secondPartExecutionTime.Stop();
        Console.WriteLine($"Second part took {secondPartExecutionTime.ElapsedMilliseconds}ms.");

    }
    public abstract string FirstPart();
    public abstract string SecondPart();
}
