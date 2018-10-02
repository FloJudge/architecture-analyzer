
namespace ArchitectureAnalyzer.Neo4j.Graph
{
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Core.Graph;

    using global::Neo4j.Driver.V1;
    
    internal class Neo4JTransaction : IGraphDatabaseTransaction
    {
        //private readonly ISession _session;

        //private readonly ITransaction _tx;

        private readonly IDriver _driver;

        public Neo4JTransaction(IDriver driver)
        {
            _driver = driver;
            //_session = driver.Session();
            //_tx = _session.BeginTransaction();
        }

        public void Dispose()
        {
            //_tx.Dispose();
            //_session.Dispose();
        }

        public void Commit()
        {
            //_tx.Success();
        }

        public void Clear()
        {
            using (var session = _driver.Session())
            using (var tx = session.BeginTransaction())
            {
                var statement = new Statement("MATCH (n) DETACH DELETE n");

                tx.Run(statement);

                tx.Success();
                tx.Dispose();
                session.Dispose();
            }
        }

        public void CreateNode<T>(T model)
            where T : Node
        {
            using (var session = _driver.Session())
            using (var tx = session.BeginTransaction())
            {
                var label = typeof(T).Name;

                var statement = $"CREATE (:{label} {{model}})";

                var modelData = ModelConverter.Convert(model);
                var parameters = new Dictionary<string, object> { { "model", modelData } };

                tx.Run(statement, parameters);

                tx.Success();
                tx.Dispose();
                session.Dispose();
            }
        }

        public void CreateRelationship<TFrom, TTo>(TFrom fromNode, TTo toNode, string relationType)
            where TFrom : Node where TTo : Node
        {
            using (var session = _driver.Session())
            using (var tx = session.BeginTransaction())
            {
                var fromLabel = typeof(TFrom).Name;
                var toLabel = typeof(TTo).Name;

                var fromId = fromNode.Id;
                var toId = toNode.Id;

                var statement = $"MATCH (from:{fromLabel} {{ Id: {{fromId}} }}), (to:{toLabel}  {{ Id: {{toId}} }})"
                                + $" CREATE (from)-[:{relationType}]->(to)";

                var parameters = new Dictionary<string, object> { { "fromId", fromId }, { "toId", toId } };

                tx.Run(statement, parameters);

                tx.Success();
                tx.Dispose();
                session.Dispose();
            }
        }
    }
}
