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
            int partNumber = 0;
            
            foreach (var batch in Batch(File.ReadLines(fileName), partLength))
            {
                partNumber++;
                var partFileName = partNumber + ".txt";
                list.Add(partFileName);
                Array.Sort(batch, 0, batch.Length);
                File.WriteAllLines(partFileName, batch.Select(x => x.Build()));
            }
            
            return list.ToArray();

            // var list = new List<string>();
            //
            // using var reader = new StreamReader(fileName);
            // int partNumber = 0;
            // while (!reader.EndOfStream)
            // {
            //     partNumber++;
            //     
            //     var partFileName = partNumber + ".txt";
            //     list.Add(partFileName);
            //     
            //     using (var writer = new StreamWriter(partFileName))
            //     {
            //         for (int i = 0; i < partLength; i++)
            //         {
            //             if (reader.EndOfStream)
            //             {
            //                 break;
            //             }
            //             
            //             writer.WriteLine(reader.ReadLine());
            //         }
            //     }
            // }
            //
            // return list.ToArray();
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
    }
}