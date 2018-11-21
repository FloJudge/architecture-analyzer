namespace ArchitectureAnalyzer.Net.Scanner
{
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;

    using Microsoft.Extensions.Logging;

    using Mono.Cecil;

    internal class PropertyScanner : AbstractScanner
    {
        public PropertyScanner(ModuleDefinition module, IModelFactory factory, ILogger logger) : base(module, factory, logger)
        {
        }

        public NetProperty ScanProperty(PropertyDefinition property, NetType typeModel)
        {
            var propertyModel = Factory.CreatePropertyModel(property);
            propertyModel.DeclaringType = typeModel;
            propertyModel.Type = GetTypeFromTypeReference(property.PropertyType);

            propertyModel.Exports = GetMefUsedInterfaces(property, nameof(AttributeType.Export));
            propertyModel.Imports = GetMefUsedInterfaces(property, nameof(AttributeType.Import));

            propertyModel.PropertyTypes = GetUsedTypesInPropertyBody(property);

            return propertyModel;
        }

        private IList<NetType> GetMefUsedInterfaces(PropertyDefinition property, string attributeTypeName)
        {
            return property.CustomAttributes
                .Select(attribute => GetMefUsedTypesFromCustomAttribute(attribute, attributeTypeName, property.PropertyType))
                .Where(IsNetType)
                .ToList();
        }

        private IList<NetType> GetUsedTypesInPropertyBody(PropertyDefinition property)
        {
            var typesInSetter = GetUsedTypesFromPropertyMethods(property.SetMethod);
            var typesInGetter = GetUsedTypesFromPropertyMethods(property.GetMethod);

            return typesInSetter.Concat(typesInGetter).Where(IsNetType).ToList();
        }

        private IList<NetType> GetUsedTypesFromPropertyMethods(MethodDefinition method)
        {
            var variablesTypes = GetTypesFromMethodVariables(method?.Body?.Variables);
            var instructionTypes = GetTypesFromMethodInstructions(method?.Body?.Instructions);

            return variablesTypes.ToList();
        }
    }
}