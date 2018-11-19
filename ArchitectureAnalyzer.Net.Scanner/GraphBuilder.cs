﻿
namespace ArchitectureAnalyzer.Net.Scanner
{
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Core.Graph;
    using ArchitectureAnalyzer.Net.Model;
    using ArchitectureAnalyzer.Net.Scanner.Model;

    using Microsoft.Extensions.Logging;

    internal class GraphBuilder
    {
        private const int NUMBER_OF_NODES_TO_COMMIT = 100;

        private readonly IModelFactory _factory;

        private readonly IGraphDatabase _db;

        private readonly ILogger _logger;

        public GraphBuilder(
            IModelFactory factory,
            IGraphDatabase db,
            ILogger logger)
        {
            _factory = factory;
            //_tx = tx;
            _db = db;
            _logger = logger;
        }

        public void Build(IDictionary<long, NetAssembly> scannedAssemblies)
        {
            ClearDatabase();

            CreateAllNodes(scannedAssemblies);

            ConnectAssemblyReferences(scannedAssemblies);

            ConnectTypes();
        }

        private void ConnectAssemblyReferences(IDictionary<long, NetAssembly> scannedAssemblies)
        {
            using (var tx = _db.BeginTransaction())
            {
                foreach (var assemblyModel in scannedAssemblies)
                {
                    ConnectAssemblyReferences(assemblyModel.Value, tx);
                }

                tx.Commit();
            }
        }

        private void ClearDatabase()
        {
            using (var tx = _db.BeginTransaction())
            {
                _logger.LogInformation("Clearing database");

                tx.Clear();
                tx.Commit();
            }
        }

        private void CreateAllNodes(IDictionary<long, NetAssembly> scannedAssemblies)
        {
            _logger.LogInformation("Adding nodes to database");

            CreateNodes(scannedAssemblies.Values);
            CreateNodes(_factory.GetTypeModels());
            CreateNodes(_factory.GetMethodModels());
            CreateNodes(_factory.GetMethodParameterModels());
            CreateNodes(_factory.GetPropertyModels());
        }

        private void CreateNodes<TNode>(IEnumerable<TNode> nodes)
            where TNode : Node
        {
            _logger.LogInformation("  Creating {0} nodes", typeof(TNode).Name);

            for(var index = 0; index < nodes.Count(); index++)
            {
                if (index*NUMBER_OF_NODES_TO_COMMIT >= nodes.Count())
                {
                    break;
                }

                CreateNodesPipeline(nodes.Skip(index*NUMBER_OF_NODES_TO_COMMIT).Take(NUMBER_OF_NODES_TO_COMMIT));
            }
        }

        private void CreateNodesPipeline<TNode>(IEnumerable<TNode> nodes) where TNode : Node
        {
            using (var tx = _db.BeginTransaction())
            {
                foreach (var model in nodes)
                {
                    tx.CreateNode(model);
                }

                tx.Commit();
            }
        }

        private void ConnectAssemblyReferences(NetAssembly assembly, IGraphDatabaseTransaction tx)
        {
            _logger.LogInformation("Connecting assembly references for '{0}'", assembly.Name);

            foreach (var reference in assembly.References)
            {
                tx.CreateRelationship(assembly, reference, Relationship.DEPENDS_ON);
            }
        }

        private void ConnectTypes()
        {
            _logger.LogInformation("Connecting types");
            var typeModels = _factory.GetTypeModels();
            
            for (int index = 0; index < typeModels.Count(); index++)
            {
                if (index * NUMBER_OF_NODES_TO_COMMIT >= typeModels.Count())
                {
                    break;
                }

                ConnectTypesPipeline(
                    typeModels.Skip(index * NUMBER_OF_NODES_TO_COMMIT).Take(NUMBER_OF_NODES_TO_COMMIT));
            }
        }

        private void ConnectTypesPipeline(IEnumerable<NetType> typeModels)
        {
            using (var tx = _db.BeginTransaction())
            {
                foreach (var type in typeModels)
                {
                    ConnectTypeDefinition(type, tx);
                    ConnectBaseType(type, tx);
                    ConnectInterfaceImplementations(type, tx);
                    ConnectMethods(type, tx);
                    ConnectProperties(type, tx);
                    ConnectAttributes(type, tx);
                    ConnectMefUsedInterfaces(type, tx);
                    ConnectGenericTypeArgs(type, tx);
                    ConnectGenericTypeInstantiation(type, tx);
                    ConnectUsedTypes(type, tx);
                }

                tx.Commit();
            }
        }

        private void ConnectUsedTypes(NetType type, IGraphDatabaseTransaction tx)
        {
            if (type.Types.Any())
            {
                foreach (var usedType in type.Types)
                {
                    if (usedType != null)
                    {
                        tx.CreateRelationship(type, usedType, Relationship.USES_TYPE);
                    }
                }
            }
        }

        private void ConnectTypeDefinition(NetType type, IGraphDatabaseTransaction tx)
        {
            var assembly = type.Assembly;
            if (assembly == null)
            {
                return;
            }

            tx.CreateRelationship(assembly, type, Relationship.DEFINES_TYPE);
        }

        private void ConnectBaseType(NetType type, IGraphDatabaseTransaction tx)
        {
            if (type.BaseType != null)
            {
                tx.CreateRelationship(type, type.BaseType, Relationship.EXTENDS);
            }
        }

        private void ConnectInterfaceImplementations(NetType type, IGraphDatabaseTransaction tx)
        {
            var baseTypeInterfaces = type.BaseType?.Implements ?? Enumerable.Empty<NetType>();
            var interfacesFromBaseInterfaces = baseTypeInterfaces.Concat(type.Implements.SelectMany(t => t.Implements)).ToList();

            foreach (var interfaceType in type.Implements.Except(interfacesFromBaseInterfaces))
            {
                tx.CreateRelationship(type, interfaceType, Relationship.IMPLEMENTS);
            }
        }

        private void ConnectMefUsedInterfaces(NetType type, IGraphDatabaseTransaction tx)
        {
            foreach (var typeExport in type.Exports)
            {
                tx.CreateRelationship(type, typeExport, Relationship.EXPORTS);
            }

            foreach (var typeImport in type.Imports)
            {
                tx.CreateRelationship(type, typeImport, Relationship.IMPORTS);
            }
        }

        private void ConnectAttributes(NetType type, IGraphDatabaseTransaction tx)
        {
            foreach (var attributeType in type.Attributes)
            {
                tx.CreateRelationship(type, attributeType, Relationship.HAS_ATTRIBUTE);
            }
        }

        private void ConnectMethods(NetType type, IGraphDatabaseTransaction tx)
        {
            var methods = type.Methods;
            foreach (var method in methods)
            {
                ConnectMethod(type, method, tx);
            }
        }

        private void ConnectMethod(NetType type, NetMethod method, IGraphDatabaseTransaction tx)
        {
            tx.CreateRelationship(type, method, Relationship.DEFINES_METHOD);
            tx.CreateRelationship(method, method.ReturnType, Relationship.RETURNS);

            ConnectMethodParameters(method, tx);
            ConnectGenericMethodParameters(method, tx);
            ConnectMefUsages(type, method, tx);
        }

        private void ConnectMefUsages(NetType type, NetMethod method, IGraphDatabaseTransaction tx)
        {
            foreach (var export in method.Exports)
            {
                tx.CreateRelationship(type, export, Relationship.EXPORTS);
            }

            foreach (var import in method.Imports)
            {
                tx.CreateRelationship(type, import, Relationship.IMPORTS);
            }
        }

        private void ConnectMethodParameters(NetMethod method, IGraphDatabaseTransaction tx)
        {
            foreach (var param in method.Parameters)
            {
                tx.CreateRelationship(
                    method,
                    param,
                    Relationship.DEFINES_PARAMETER);

                tx.CreateRelationship(
                    param,
                    param.Type,
                    Relationship.HAS_TYPE);
            }
        }

        private void ConnectGenericMethodParameters(NetMethod method, IGraphDatabaseTransaction tx)
        {
            foreach (var param in method.GenericParameters)
            {
                tx.CreateRelationship(method, param, Relationship.DEFINES_GENERIC_METHOD_ARG);
            }
        }

        private void ConnectGenericTypeArgs(NetType type, IGraphDatabaseTransaction tx)
        {
            foreach (var arg in type.GenericTypeArgs)
            {
                tx.CreateRelationship(type, arg, Relationship.DEFINES_GENERIC_TYPE_ARG);
            }
        }

        private void ConnectGenericTypeInstantiation(NetType type, IGraphDatabaseTransaction tx)
        {
            if (!type.IsGenericTypeInstantiation)
            {
                return;
            }

            tx.CreateRelationship(type, type.GenericType, Relationship.INSTANTIATES_GENERIC_TYPE);

            foreach (var arg in type.GenericTypeInstantiationArgs)
            {
                tx.CreateRelationship(type, arg, Relationship.HAS_TYPE_ARGUMENT);
            }
        }

        private void ConnectProperties(NetType type, IGraphDatabaseTransaction tx)
        {
            foreach (var property in type.Properties)
            {
                ConnectProperty(type, property, tx);

                foreach (var exportType in property.Exports)
                {
                    tx.CreateRelationship(type, exportType, Relationship.EXPORTS);
                }

                foreach (var importType in property.Imports)
                {
                    tx.CreateRelationship(type, importType, Relationship.IMPORTS);
                }
            }
        }

        private void ConnectProperty(NetType type, NetProperty property, IGraphDatabaseTransaction tx)
        {
            tx.CreateRelationship(type, property, Relationship.DEFINES_PROPERTY);
            tx.CreateRelationship(property, property.Type, Relationship.HAS_TYPE);
        }
    }
}
