using System.Diagnostics;
using ProfilingProgram;

var sw = Stopwatch.StartNew();

var generator = new Generator();

var fileName = generator.Generate(50_000);

var sorter = new Sorter();

await sorter.Sort(fileName, 5_000);

sw.Stop();

Console.WriteLine($"Execution took: {sw.ElapsedMilliseconds}ms");