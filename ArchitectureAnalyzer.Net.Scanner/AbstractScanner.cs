namespace ArchitectureAnalyzer.Net.Scanner
{
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;

    using Microsoft.Extensions.Logging;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

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
                    return GetTypeFromTypeReference(argTypeRef);
                }
            }

            return GetTypeFromTypeReference(typeRef);
        }

        protected NetType GetTypeFromTypeReference(TypeReference typeReference)
        {
            return Factory.CreateTypeModel(typeReference);
        }

        protected IEnumerable<NetType> GetTypesFromMethodVariables(ICollection<VariableDefinition> variables)
        {
            return variables?.Select(variable => GetTypeFromTypeReference(variable.VariableType)) ?? Enumerable.Empty<NetType>();
        }

        protected IEnumerable<NetType> GetTypesFromMethodInstructions(ICollection<Instruction> instructions)
        {
            var typesFromInstructions = instructions?.Select(instruction => instruction);
            return Enumerable.Empty<NetType>();
        }

        protected bool IsNetType(NetType type)
        {
            return type != null;
        }
    }
}