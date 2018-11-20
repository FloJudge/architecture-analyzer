namespace ArchitectureAnalyzer.Net.Scanner
{
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;
    using ArchitectureAnalyzer.Net.Scanner.Utils;

    using Microsoft.Extensions.Logging;

    using Mono.Cecil;
    
    internal class MethodScanner : AbstractScanner
    {
        public MethodScanner(ModuleDefinition module, IModelFactory factory, ILogger logger)
            : base(module, factory, logger)
        {
        }

        public NetMethod ScanMethod(MethodDefinition method, NetType typeModel)
        {
            Logger.LogTrace("    Scanning method '{0}'", method.Name);
            
            var methodModel = Factory.CreateMethodModel(method);
            methodModel.DeclaringType = typeModel;
            methodModel.Visibility = GetVisibility(method);
            methodModel.IsAbstract = IsAbstract(method);
            methodModel.IsStatic = IsStatic(method);
            methodModel.IsSealed = IsSealed(method);
            methodModel.IsGeneric = IsGeneric(method);
            methodModel.GenericParameters = CreateGenericParameters(method);
            
            methodModel.ReturnType = GetTypeFromTypeReference(method.ReturnType);
            methodModel.Parameters = CreateParameters(method, methodModel);
            
            methodModel.Exports = GetMefUsedInterfaces(method, nameof(AttributeType.Export));
            methodModel.Imports = GetMefUsedInterfaces(method, nameof(AttributeType.Import));

            methodModel.MethodTypes = GetUsedTypesInMethodBody(method);

            return methodModel;
        }

        private Visibility GetVisibility(MethodDefinition method)
        {
            return method.Attributes.ToVisibility();
        }

        private IReadOnlyList<NetMethodParameter> CreateParameters(
            MethodDefinition method,
            NetMethod methodModel)
        {
            return method.Parameters
                .Select(param => CreateParameter(method, param, methodModel))
                .ToList();
        }

        private NetMethodParameter CreateParameter(
            MethodDefinition method,
            ParameterDefinition param,
            NetMethod methodModel)
        {
            var model = Factory.CreateMethodParameter(method, param);
            model.Order = param.Sequence;
            model.Type = GetTypeFromTypeReference(param.ParameterType);
            model.DeclaringMethod = methodModel;

            return model;
        }

        private IReadOnlyList<NetType> CreateGenericParameters(MethodDefinition method)
        {
            return method.GenericParameters
                .Select(CreateGenericParameter)
                .ToList();
        }

        private NetType CreateGenericParameter(GenericParameter arg)
        {
            return Factory.CreateGenericTypeArg(arg);
        }

        private static bool IsAbstract(MethodDefinition method)
        {
            return method.Attributes.HasFlag(MethodAttributes.Abstract);
        }

        private static bool IsStatic(MethodDefinition method)
        {
            return method.Attributes.HasFlag(MethodAttributes.Static);
        }
        
        private static bool IsSealed(MethodDefinition method)
        {
            return method.Attributes.HasFlag(MethodAttributes.Final);
        }

        private static bool IsGeneric(MethodDefinition method)
        {
            return method.HasGenericParameters;
        }
        private IList<NetType> GetMefUsedInterfaces(MethodDefinition methodDefinition, string attributeTypeName)
        {
            // Case: Attribute on Method
            var mefMethodTypes = methodDefinition.CustomAttributes
                .Select(attribute => GetMefUsedTypesFromCustomAttribute(attribute, attributeTypeName, methodDefinition.ReturnType))
                .Where(IsValidNetType)
                .ToList();

            // Case: Attribute on MethodParameter
            var mefParamTypes = methodDefinition.Parameters
                    .Select(param => GetMefUsedMethodInterface(param, attributeTypeName, methodDefinition.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.Name.Contains(attributeTypeName))))
                    .Where(IsNetType)
                    .ToList();

            return mefMethodTypes.Concat(mefParamTypes).ToList();
        }

        private bool IsValidNetType(NetType t)
        {
            return IsNetType(t) && !t.Name.Equals("Void");
        }

        private NetType GetMefUsedMethodInterface(
            ParameterDefinition parameter,
            string attributeTypeName,
            CustomAttribute methodAttribute)
        {
            var paramAttribute =
                parameter.CustomAttributes.FirstOrDefault(attr => attr.AttributeType.Name.Contains(attributeTypeName));

            return paramAttribute == null
                       ? GetMefUsedTypesFromCustomAttribute(methodAttribute, attributeTypeName, parameter.ParameterType)
                       : GetMefUsedTypesFromCustomAttribute(paramAttribute, attributeTypeName, parameter.ParameterType);
        }

        private IList<NetType> GetUsedTypesInMethodBody(MethodDefinition methodDefinition)
        {
            var types = GetUsedParameterTypes(methodDefinition);
            var returnTypes = GetReturnTypes(methodDefinition);
            var typesInBody = GetTypesFromBody(methodDefinition);

            return types.Concat(returnTypes).Concat(typesInBody).Where(IsValidNetType).ToList();
        }
        
        private IEnumerable<NetType> GetUsedParameterTypes(MethodDefinition methodDefinition)
        {
            return methodDefinition.Parameters.Select(t => GetTypeFromTypeReference(t.ParameterType));
        }

        private IEnumerable<NetType> GetReturnTypes(MethodDefinition methodDefinition)
        {
            return new[] { GetTypeFromTypeReference(methodDefinition.ReturnType) };
        }

        private IEnumerable<NetType> GetTypesFromBody(MethodDefinition methodDefinition)
        {
            var variableTypes = GetTypesFromMethodVariables(methodDefinition);
            // TODO add function to get type from instructions

            return variableTypes ?? Enumerable.Empty<NetType>();
        }

        private IEnumerable<NetType> GetTypesFromMethodVariables(MethodDefinition methodDefinition)
        {
            return methodDefinition.Body?.Variables.Select(variableDefinition => GetTypeFromTypeReference(variableDefinition.VariableType));
        }
    }
}