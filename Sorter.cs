namespace ProfilingProgram
{
    public class Sorter
    {
        public void Sort(string fileName, int partLength)
        {
            var files = SplitFile(fileName, partLength);
            SortParts(files, partLength);
            SortResult(files);
        }

        private string[] SplitFile(string fileName, int partLength)
        {
            var list = new List<string>();
            
            using var reader = new StreamReader(fileName);
            int partNumber = 0;
            while (!reader.EndOfStream)
            {
                partNumber++;
                
                var partFileName = partNumber + ".txt";
                list.Add(partFileName);
                
                using (var writer = new StreamWriter(partFileName))
                {
                    for (int i = 0; i < partLength; i++)
                    {
                        if (reader.EndOfStream)
                        {
                            break;
                        }
                        
                        writer.WriteLine(reader.ReadLine());
                    }
                }
            }
            
            return list.ToArray();
        }
         
        private void SortParts(string[] files, int partLength)
        {
            foreach (var file in files)
            {
                var sortedLines = File.ReadAllLines(file)
                    .Select(x => new Line(x))
                    .OrderBy(x => x);
                
                File.WriteAllLines(file, sortedLines.Select(x => x.Build()));
            }
        }
        
        private void SortResult(string[] files)
        {
            var readers = files.Select(x => new StreamReader(x)).ToList();

            try
            {
                var lines = readers.Select(x => new LineState
                {
                    Reader = x,
                    Line = new Line(x.ReadLine())
                }).ToList();
                
                using var writer = new StreamWriter("result.txt");

                while (lines.Count > 0)
                {
                    var current = lines.OrderBy(x => x.Line).First();
                    writer.WriteLine(current.Line.Build());
                    
                    if (current.Reader.EndOfStream)
                    {
                        lines.Remove(current);
                        continue;
                    }
                    
                    current.Line = new Line(current.Reader.ReadLine());
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
    }
}