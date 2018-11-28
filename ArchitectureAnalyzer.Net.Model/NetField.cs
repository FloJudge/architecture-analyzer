using System;
using System.Collections.Generic;
using System.Text;

namespace ArchitectureAnalyzer.Net.Model
{
    using ArchitectureAnalyzer.Core.Graph;

    public class NetField : Node
    {
        public string Name { get; set; }

        [Ignore]
        public IList<NetType> Exports { get; set; }

        [Ignore]
        public IList<NetType> Imports { get; set; }

        [Ignore]
        public NetType Type { get; set; }

        [Ignore]
        public NetType DeclaringType { get; set; }


        public NetField()
        {
            Exports = new List<NetType>();
            Imports = new List<NetType>();
        }
    }
}
