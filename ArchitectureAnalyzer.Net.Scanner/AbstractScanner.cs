namespace ArchitectureAnalyzer.Net.Scanner
{
    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;

    using Microsoft.Extensions.Logging;

    using Mono.Cecil;

    internal abstract class AbstractScanner
    {
        protected ModuleDefinition Module { get; }

        protected IModelFactory Factory { get; }

        protected ILogger Logger { get; }

        protected AbstractScanner(ModuleDefinition module, IModelFactory factory, ILogger logger)
        {
            Module = module;
            Factory = factory;
            Logger = logger;
        }

        protected NetType GetTypeFromCustomAttribute(CustomAttribute customAttribute)
        {
            return GetTypeFromTypeReference(customAttribute.AttributeType);
        }

        protected NetType GetMefUsedTypesFromCustomAttribute(CustomAttribute customAttribute, string typeName, TypeReference typeRef)
        {
            if (customAttribute == null || !customAttribute.AttributeType.Name.Contains(typeName))
            {
                return null;
            }

            var nameOfAttributeType = string.Empty;
            foreach (var customArg in customAttribute.ConstructorArguments)
            {
                if (customArg.Value is string)
                {
                    nameOfAttributeType = customArg.Value.ToString();
                }

                if (customArg.Value is TypeReference argTypeRef)
                {
                    if (string.IsNullOrEmpty(nameOfAttributeType))
                    {
                        return Factory.CreateTypeModel(argTypeRef);
                    }

                    argTypeRef.Name = nameOfAttributeType;
                    return Factory.CreateTypeModel(argTypeRef);
                }
            }
                
            return Factory.CreateTypeModel(typeRef);
        }

        protected NetType GetTypeFromTypeReference(TypeReference typeReference)
        {
            return Factory.CreateTypeModel(typeReference);
        }
    }
}
