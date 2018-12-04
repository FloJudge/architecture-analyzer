
namespace ArchitectureAnalyzer.Net.Scanner.Test
{
    using System;
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Net.Model;

    using NUnit.Framework;

    using TestLibrary;

    [TestFixture]
    public class PropertyScannerTest : MetadataScannerTestBase
    {
        private PropertyScanner _scanner;

        [SetUp]
        public void SetupScanner()
        {
            _scanner = new PropertyScanner(Module, ModelFactory, Logger);

            var assembly = new NetAssembly { Name = "TestLibrary" };

            var typeScanner = new TypeScanner(Module, ModelFactory, Logger);
            typeScanner.ScanType(GetTypeDefintion<ClassWithMembers>(), assembly);
            typeScanner.ScanType(GetTypeDefintion<InheritedFromClassWithMembers>(), assembly);
        }

        [Test]
        public void NameIsCorrect()
        {
            var property = GetPropertyDefinition<ClassWithMembers>(nameof(ClassWithMembers.Property));

            var model = _scanner.ScanProperty(property, NetType<ClassWithMembers>());

            Assert.That(model.Name, Is.EqualTo(nameof(ClassWithMembers.Property)));
        }

        [Test]
        public void ReturnTypeIsCorrect()
        {
            var property = GetPropertyDefinition<ClassWithMembers>(nameof(ClassWithMembers.Property));

            var model = _scanner.ScanProperty(property, NetType<ClassWithMembers>());

            Assert.That(model.Type.Name, Is.EqualTo(nameof(String)));
        }

        [Test]
        [TestCase(nameof(ClassWithMefUsages.ExportProperty), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportPropertyWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportPropertyWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportPropertyWithNameAndType), 1)]
        public void HasPropertyExportTypes(string propertyName, int expectedValue)
        {
            var type = GetPropertyDefinition<ClassWithMefUsages>(propertyName);

            var model = _scanner.ScanProperty(type, NetType<ClassWithMefUsages>());

            Assert.That(model.Exports.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCase(nameof(ClassWithMefUsages.ImportProperty), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportPropertyWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportPropertyWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportPropertyWithNameAndType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyProperty), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyPropertyWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyPropertyWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyPropertyWithNameAndType), 1)]
        public void HasPropertyImportTypes(string propertyName, int expectedValue)
        {
            var type = GetPropertyDefinition<ClassWithMefUsages>(propertyName);

            var model = _scanner.ScanProperty(type, NetType<ClassWithMefUsages>());

            Assert.That(model.Imports.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeTypeInPropertyGetter()
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(nameof(TypeUsageInProperty.ReturnTypeInPropertyGetter));

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.TypeDeclarationInPropertyGetter))]
        [TestCase(nameof(TypeUsageInProperty.TypeDeclarationInPropertySetter))]
        public void DoesPropertyUsesTypeAsTypeDeclaration(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }


        [TestCase(nameof(TypeUsageInProperty.InitTypeInPropertyGetter))]
        [TestCase(nameof(TypeUsageInProperty.InitTypeInPropertySetter))]
        public void DoesPropertyUsesTypeAsInitType(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.TypeSafeCastInPropertyGetter))]
        [TestCase(nameof(TypeUsageInProperty.TypeSafeCastInPropertySetter))]
        public void DoesPropertyUsesTypeAsTypeSafeCast(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType<Object>(), NetType<UsedType>() };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.TypeCastInPropertyGetter))]
        [TestCase(nameof(TypeUsageInProperty.TypeCastInPropertySetter))]
        public void DoesPropertyUsesTypeAsTypeCast(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Object)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitListTypeInPropertyGetter))]
        public void DoesPropertyUsesTypeAsInitListTypeInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<UsedType>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitListTypeInPropertySetter))]
        public void DoesPropertyUsesTypeAsInitListTypeInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<UsedType>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitTypesInTupleInPropertyGetter))]
        public void DoesPropertyUsesInitTypesInTupleInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, bool>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitTypesInTupleInPropertySetter))]
        public void DoesPropertyUsesInitTypesInTupleInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, bool>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitMultipleTypesInMultipleTupleInPropertyGetter))]
        public void DoesPropertyUsesInitMultipleTypesInMultipleTupleInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, bool>)), NetType(typeof(Tuple<int, float>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitMultipleTypesInMultipleTupleInPropertySetter))]
        public void DoesPropertyUsesInitMultipleTypesInMultipleTupleInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, bool>)), NetType(typeof(Tuple<int, float>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitTupleWithSameTypesInPropertyGetter))]
        public void DoesPropertyUsesInitTupleWithSameTypesInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, UsedType>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitTupleWithSameTypesInPropertySetter))]
        public void DoesPropertyUsesInitTupleWithSameTypesInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(Tuple<UsedType, UsedType>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitDynamicListTypInPropertyGetter))]
        public void DoesPropertyUsesInitDynamicListTypInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(UsedType[])), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitDynamicListTypInPropertySetter))]
        public void DoesPropertyUsesInitDynamicListTypInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(UsedType[])) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitListTupleTypesInPropertyGetter))]
        public void DoesPropertyUsesInitListTupleTypesInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<Tuple<UsedType, bool>>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitListTupleTypesInPropertySetter))]
        public void DoesPropertyUsesInitListTupleTypesInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<Tuple<UsedType, bool>>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitListActionTypeInPropertyGetter))]
        public void DoesPropertyUsesInitListActionTypeInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<Action<UsedType>>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitListActionTypeInPropertySetter))]
        public void DoesPropertyUsesInitListActionTypeInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<Action<UsedType>>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }

        [TestCase(nameof(TypeUsageInProperty.InitListListTypeInPropertyGetter))]
        public void DoesPropertyUsesInitListListTypeInPropertyGetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<List<UsedType>>)), NetType(typeof(UsedType)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
        
        [TestCase(nameof(TypeUsageInProperty.InitListListTypeInPropertySetter))]
        public void DoesPropertyUsesInitListListTypeInPropertySetter(string propertyName)
        {
            var property = GetPropertyDefinition<TypeUsageInProperty>(propertyName);

            var model = _scanner.ScanProperty(property, NetType<TypeUsageInProperty>());

            var expectedTypes = new[] { NetType(typeof(List<List<UsedType>>)) };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
    }
}
