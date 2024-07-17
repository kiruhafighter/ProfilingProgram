namespace ProfilingProgram
{
    public class Sorter
    {
        public async Task Sort(string fileName, int partLength)
        {
            var files = await SplitFile(fileName, partLength);
            //SortParts(files, partLength);
            await SortResult(files);
        }
        
        private async Task<string[]> SplitFile(string fileName, int partLength)
        {
            var list = new List<string>();
            int partNumber = 0;
            Line[] lines = new Line[partLength];
            int i = 0;
            
            using var reader = new StreamReader(fileName);
            
            for (string line = await reader.ReadLineAsync(); 
                 !reader.EndOfStream;
                 line = await reader.ReadLineAsync())
            {
                lines[i] = new Line(line!);
                i++;

                if (i == partLength)
                {
                    partNumber++;
                    var partFileName = partNumber + ".txt";
                    list.Add(partFileName);
                    Array.Sort(lines);
                    await File.WriteAllLinesAsync(partFileName, lines.Select(x => x.Build()));
                    i = 0;
                }
            }

            if (i > 0)
            {
                Array.Resize(ref lines, i);
                partNumber++;
                var partFileName = partNumber + ".txt";
                list.Add(partFileName);
                Array.Sort(lines);
                await File.WriteAllLinesAsync(partFileName, lines.Select(x => x.Build()));
            }
            
            return list.ToArray();
        }
         
        private void SortParts(string[] files, int partLength)
        {
            Line[] lines = new Line[partLength];
            foreach (var file in files)
            {
                var strings = File.ReadAllLines(file);
                for (int i = 0; i < strings.Length; i++)
                {
                    lines[i] = new Line(strings[i]);
                }
                
                Array.Sort(lines, 0, strings.Length);
                File.WriteAllLines(file, lines.Select(x => x.Build()));
            }
        }
        
        private async Task SortResult(string[] files)
        {
            var readers = files.Select(x => new StreamReader(x)).ToList();

            try
            {
                var lines = readers.Select(x => new LineState
                {
                    Reader = x,
                    Line = new Line(x.ReadLine())
                }).OrderBy(x => x.Line).ToList();
                
                using var writer = new StreamWriter("result.txt");

                while (lines.Count > 0)
                {
                    var current = lines[0];
                    await writer.WriteLineAsync(current.Line.Build());
                    
                    if (current.Reader.EndOfStream)
                    {
                        lines.Remove(current);
                        continue;
                    }
                    
                    current.Line = new Line(await current.Reader.ReadLineAsync());
                    Reorder(lines);
                }
            }
            finally
            {
                foreach (var reader in readers)
                {
                    reader.Dispose();
                }
            }
        }

        private IEnumerable<Line[]> Batch(IEnumerable<string> lines, int batchSize) // this or Chunk method???
        {
            Line[] l = new Line[batchSize];

            int i = 0;
            foreach (var line in lines)
            {
                l[i] = new Line(line);
                i++;
                
                if (i == batchSize)
                {
                    yield return l;
                    i = 0;
                }
            }

            if (i > 0)
            {
                Array.Resize(ref l, i);
                yield return l;
            }
        }

        private void Reorder(List<LineState> lines)
        {
            if (lines.Count == 1)
            {
                return;
            }
            
            int i = 0;
            while (lines[i].Line.CompareTo(lines[i + 1].Line) > 0)
            {
                (lines[i], lines[i + 1]) = (lines[i + 1], lines[i]);
                i++;
                
                if (i + 1 == lines.Count)
                {
                    return;
                }
            }
        }
    }
}