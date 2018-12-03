namespace ArchitectureAnalyzer.Net.Model
{
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Core.Graph;
    
    public class NetMethod : Node, IGenericContext, IAnalyzerExtension
    {
        public string Name { get; set; }

        public bool IsAbstract { get; set; }

        public bool IsStatic { get; set; }

        public bool IsSealed { get; set; }

        public bool IsGeneric { get; set; }
        
        public Visibility Visibility { get; set; }

        [Ignore]
        public NetType DeclaringType { get; set; }

        [Ignore]
        public NetType ReturnType { get; set; }

        [Ignore]
        public IReadOnlyList<NetMethodParameter> Parameters { get; set; }

        [Ignore]
        public IReadOnlyList<NetType> GenericParameters { get; set; }

        #region IAnalyzerExtension

        [Ignore]
        public IList<NetType> Exports { get; set; }

        [Ignore]
        public IList<NetType> Imports { get; set; }

        [Ignore]
        public IList<NetType> TypesUsedInBody { get; set; }

        #endregion

        public NetMethod()
        {
            Parameters = new List<NetMethodParameter>();
            GenericParameters = new List<NetType>();

            Exports = new List<NetType>();
            Imports = new List<NetType>();
            TypesUsedInBody = new List<NetType>();
        }
        
        public override string ToString()
        {
            return $"NetMethod({Name})";
        }
    }
}
