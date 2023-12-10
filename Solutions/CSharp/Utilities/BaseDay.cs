using System;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Base class for day-specific solutions.
/// </summary>
public abstract class BaseDay
{
    private const string BaseSolutionsDir = "Solutions";
    private const string BaseInputExt = ".txt";
    protected string[] baseInfo;
    protected string[] inputContent;
    private Stopwatch firstPartExecutionTime;
    private Stopwatch secondPartExecutionTime;

    public virtual string[] ReadInput(string Filename = "")
    {
        return File.ReadAllLines(GetInput(Filename));

    }
    protected virtual string GetInput(string filename = "")
    {
        baseInfo = GetType().FullName.Split('.');
        string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        string dayDir = Path.Combine(baseDir, BaseSolutionsDir, baseInfo[0], baseInfo[1]);

        return Path.Combine(dayDir, string.IsNullOrEmpty(filename) ? $"{baseInfo[1]}{BaseInputExt}" : $"{filename}{BaseInputExt}");
    }

    public virtual void Run(string Filename = "")
    {
        inputContent = ReadInput(Filename);
        Console.WriteLine($"Solution {baseInfo[0]} - {baseInfo[1]}");
        ExecuteSolutionPart(FirstPart, "First Part");
        ExecuteSolutionPart(SecondPart, "Second Part");
    }
    private void ExecuteSolutionPart(Func<string> solutionPart, string partName)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();
        Console.WriteLine($"{partName} result -> {solutionPart()}");
        timer.Stop();
        Console.WriteLine($"{partName} took {timer.ElapsedMilliseconds}ms.");
    }

    public abstract string FirstPart();
    public abstract string SecondPart();
}
