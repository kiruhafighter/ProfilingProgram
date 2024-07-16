namespace ProfilingProgram
{
    public class Line : IComparable<Line>
    {
        public int Number { get; set; }
        
        public string Text { get; set; }
        
        public Line(string line)
        {
            int pos = line.IndexOf('.');
            Number = int.Parse(line[..pos]);
            Text = line[(pos + 2)..];
        }
        
        public string Build()
        {
            return $"{Number}. {Text}";
        }
        
        public int CompareTo(Line? other)
        {
            int result = Text.CompareTo(other.Text);
            if (result != 0)
            {
                return result;
            }
            
            return Number.CompareTo(other.Number);
        }
    }
}