namespace ArchitectureAnalyzer.Net.Scanner
{
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;

    using Microsoft.Extensions.Logging;

    using Mono.Cecil;

    internal class FieldScanner : AbstractScanner
    {
        public FieldScanner(ModuleDefinition module, IModelFactory factory, ILogger logger)
            : base(module, factory, logger)
        {
        }

        public NetField ScanField(FieldDefinition field, NetType typeModel)
        {
            var fieldModel = Factory.CreateFieldModel(field);
            fieldModel.DeclaringType = typeModel;
            fieldModel.Type = GetTypeFromTypeReference(field.FieldType);

            fieldModel.Exports = GetMefUsedInterfaces(field, nameof(AttributeType.Export));
            fieldModel.Imports = GetMefUsedInterfaces(field, nameof(AttributeType.Import));
            
            return fieldModel;
        }

        private IList<NetType> GetMefUsedInterfaces(FieldDefinition field, string attributeTypeName)
        {
            return field.CustomAttributes
                .Select(attribute => GetMefUsedTypesFromCustomAttribute(attribute, attributeTypeName, field.FieldType))
                .Where(IsNetType)
                .ToList();
        }
    }
}