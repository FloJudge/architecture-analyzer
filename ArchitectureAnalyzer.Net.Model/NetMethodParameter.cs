namespace ArchitectureAnalyzer.Net.Model
{
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Core.Graph;
    
    public class NetMethodParameter : Node, IAnalyzerExtension
    {
        public string Name { get; set; }

        public int Order { get; set; }

        [Ignore]
        public NetType Type { get; set; }

        [Ignore]
        public NetMethod DeclaringMethod { get; set; }
        
        [Ignore]
        public IList<NetType> Exports { get; set; }

        [Ignore]
        public IList<NetType> Imports { get; set; }

        [Ignore]
        public IList<NetType> TypesUsedInBody { get; set; }

        public NetMethodParameter()
        {
            Imports = new List<NetType>();
            Exports = new List<NetType>();
            TypesUsedInBody = new List<NetType>();
        }

        public override string ToString()
        {
            return $"NetMethodParameter({Name})";
        }
    }
}