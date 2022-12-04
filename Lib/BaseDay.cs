using System;
using System.IO;
using System.Reflection;

public abstract class BaseDay
{
    const string baseSolutionsDir = "Solutions";
    const string baseInputExt = ".txt";
    protected string[] baseInfo;
    protected string[] inputContent;
    public virtual string[] ReadInput()
    {
        return File.ReadAllLines(GetInput());

    }
    protected virtual string GetInput()
    {
        baseInfo = GetType().FullName.Split(".");
        string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
        string dayDir = Path.Combine(baseDir, baseSolutionsDir, baseInfo[0], baseInfo[1]);
        string inputPath = dayDir + $"\\{baseInfo[1]}{baseInputExt}";
        return inputPath;

    }

    public virtual void Run()
    {
        inputContent = ReadInput();
        Console.WriteLine($"Solution {baseInfo[0]} - {baseInfo[1]}");
        Console.WriteLine($"First Part: {FirstPart()}");
        Console.WriteLine($"Second Part: {SecondPart()}");
    }
    public abstract string FirstPart();
    public abstract string SecondPart();
}
