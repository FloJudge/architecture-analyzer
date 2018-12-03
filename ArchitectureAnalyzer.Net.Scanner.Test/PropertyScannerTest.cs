
namespace ArchitectureAnalyzer.Net.Scanner.Test
{
    using System;

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
            var property = GetPropertyDefinition<TypeUsingOtherTypeInProperty>(nameof(TypeUsingOtherTypeInProperty.UsedTypePropertyWithGetter));

            var model = _scanner.ScanProperty(property, NetType<TypeUsingOtherTypeInProperty>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.TypesUsedInBody, Is.EquivalentTo(expectedTypes));
        }
    }
}
