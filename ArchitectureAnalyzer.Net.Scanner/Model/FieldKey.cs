namespace ArchitectureAnalyzer.Net.Scanner.Model
{
    public struct FieldKey
    {
        public string Type { get; }

        public string Name { get; }

        public FieldKey(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}