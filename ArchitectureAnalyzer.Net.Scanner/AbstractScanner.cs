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
            var genericTypeArgs = GetTypesFromGenericEnumerable(variables);
            var variableTypes = variables?.Select(variable => GetTypeFromTypeReference(variable.VariableType.GetElementType())) ?? Enumerable.Empty<NetType>();

            return variableTypes.Concat(genericTypeArgs);
        }

        private IEnumerable<NetType> GetTypesFromGenericEnumerable(ICollection<VariableDefinition> variables)
        {
            if (variables == null)
            {
                return Enumerable.Empty<NetType>();
            }

            return variables.Where(varType => varType.VariableType is GenericInstanceType).SelectMany(
                varType => GetTypesFromGeneric(varType.VariableType as GenericInstanceType)).Distinct();
        }

        private IEnumerable<NetType> GetTypesFromGeneric(GenericInstanceType genericType)
        {
            var typesInGenericGeneric = GetTypesFromInnerGeneric(genericType) ?? Enumerable.Empty<NetType>();
            var typesInGeneric = genericType.GenericArguments?.Select(argument => GetTypeFromTypeReference(argument.GetElementType())) ?? Enumerable.Empty<NetType>();

            return typesInGeneric.Concat(typesInGenericGeneric);
        }

        private IEnumerable<NetType> GetTypesFromInnerGeneric(GenericInstanceType genericType)
        {
           return genericType.GenericArguments
                ?.Where(genericArgument => genericArgument is GenericInstanceType).SelectMany(
                    genericArgument => GetTypesFromGeneric(genericArgument as GenericInstanceType)).Distinct();
        }

        protected IEnumerable<NetType> GetTypesFromMethodInstructions(ICollection<Instruction> instructions)
        {
            var typesFromInstructions = instructions?
                .Where(i => i.Operand != null)
                .Select(instruction => instruction.Operand.GetType());
            return Enumerable.Empty<NetType>();
        }

        protected bool IsNetType(NetType type)
        {
            return type != null;
        }
    }
}