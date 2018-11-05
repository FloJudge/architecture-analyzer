
namespace ArchitectureAnalyzer.Net.Model
{
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Core.Graph;
    
    public class NetProperty : Node, IGenericContext
    {
        private static readonly IReadOnlyList<NetType> NoGenericParameters = new NetType[0];

        public string Name { get; set; }

        [Ignore]
        public IList<NetType> Exports { get; set; }

        [Ignore]
        public IList<NetType> Imports { get; set; }

        [Ignore]
        public NetType Type { get; set; }

        [Ignore]
        public NetType DeclaringType { get; set; }

        [Ignore]
        public IReadOnlyList<NetType> GenericParameters => NoGenericParameters;

        public NetProperty()
        {
            Exports = new List<NetType>();
            Imports = new List<NetType>();
        }
    }
}
