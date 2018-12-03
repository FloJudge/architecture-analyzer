using System;
using System.Collections.Generic;
using System.Text;

namespace ArchitectureAnalyzer.Net.Model
{
    using ArchitectureAnalyzer.Core.Graph;

    public class NetField : Node, IGenericContext, IAnalyzerExtension
    {
        private static readonly IReadOnlyList<NetType> NoGenericParameters = new NetType[0];

        public string Name { get; set; }

        [Ignore]
        public NetType Type { get; set; }

        [Ignore]
        public NetType DeclaringType { get; set; }

        [Ignore]
        public IReadOnlyList<NetType> GenericParameters => NoGenericParameters;

        #region IAnaylzerExtension

        [Ignore]
        public IList<NetType> Exports { get; set; }

        [Ignore]
        public IList<NetType> Imports { get; set; }

        [Ignore]
        public IList<NetType> TypesUsedInBody { get; set; }

        #endregion

        public NetField()
        {
            Exports = new List<NetType>();
            Imports = new List<NetType>();
            TypesUsedInBody = new List<NetType>();
        }
    }
}
