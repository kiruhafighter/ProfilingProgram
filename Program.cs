using System.Diagnostics;
using ProfilingProgram;

var sw = Stopwatch.StartNew();

var generator = new Generator();

var fileName = generator.Generate(5_000);

var sorter = new Sorter();

await sorter.Sort(fileName, 5_00);

sw.Stop();

Console.WriteLine($"Execution took: {sw.ElapsedMilliseconds}ms");