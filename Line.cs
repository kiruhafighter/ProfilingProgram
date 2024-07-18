namespace ProfilingProgram
{
    public struct Line : IComparable<Line>
    {
        private readonly int _pos;
        
        private readonly string _line;
        
        private readonly int _number;
        
        private ReadOnlySpan<char> Text => _line.AsSpan(_pos + 2);
        
        public Line(string line)
        {
            _pos = line.IndexOf('.');
            _number = int.Parse(line.AsSpan(0, _pos));
            _line = line;
        }

        public string Build() => _line;

        public int CompareTo(Line other)
        {
            int result = Text.CompareTo(other.Text, StringComparison.Ordinal);
            
            return result != 0 ? 
                result : _number.CompareTo(other._number);
        }
    }
}