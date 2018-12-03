namespace ArchitectureAnalyzer.Net.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public interface IAnalyzerExtension
    {
        IList<NetType> Exports { get; set; }

        IList<NetType> Imports { get; set; }

        IList<NetType> TypesUsedInBody { get; set; } 
    }
}
