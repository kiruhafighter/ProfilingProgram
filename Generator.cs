namespace ProfilingProgram
{
    internal sealed class Generator
    {
        private readonly Random _random = new();
        private readonly string[] _words;

        public Generator()
        {
            _words = Enumerable.Range(0, 1000)
                .Select(x =>
                {
                    var range = Enumerable.Range(0, _random.Next(20, 100));
                    var chars = range.Select(x => (char)_random.Next('A', 'Z' + 1)).ToArray();
                    var str = new string(chars);
                    return str;
                }).ToArray();
        }
        
        public string Generate(int linesCount)
        {
            var fileName = "L" + linesCount + ".txt";
            
            using var writer = new StreamWriter(fileName);

            for (int i = 0; i < linesCount; i++)
            {
                writer.WriteLine(GenerateNumberString() + ". " + GenerateString());
            }
            
            return fileName;
        }

        private string GenerateNumberString()
            => _random.Next(0, 10000).ToString();

        private string GenerateString()
            => _words[_random.Next(0, _words.Length)];
    }
}