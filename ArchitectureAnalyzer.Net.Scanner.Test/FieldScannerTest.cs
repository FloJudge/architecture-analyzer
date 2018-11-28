namespace ArchitectureAnalyzer.Net.Scanner.Test
{
    using System;
    using System.Collections.Generic;

    using ArchitectureAnalyzer.Net.Model;

    using NUnit.Framework;

    using TestLibrary;

    public class FieldScannerTest : MetadataScannerTestBase
    {
        private FieldScanner _scanner;

        [SetUp]
        public void SetupScanner()
        {
            _scanner = new FieldScanner(Module, ModelFactory, Logger);
        }

        [Test]
        public void NameIsCorrect()
        {
            var property = GetFieldDefinition<ClassWithFields>(nameof(ClassWithFields.Primitive));

            var model = _scanner.ScanField(property, NetType<ClassWithMembers>());

            Assert.That(model.Name, Is.EqualTo(nameof(ClassWithFields.Primitive)));
        }

        [TestCase(nameof(ClassWithFields.Primitive), typeof(Int32))]
        [TestCase(nameof(ClassWithFields.Reference), typeof(UsedType))]
        [TestCase(nameof(ClassWithFields.GenericPrimitive), typeof(List<Int32>))]
        [TestCase(nameof(ClassWithFields.GenericReference), typeof(List<UsedType>))]
        [TestCase(nameof(ClassWithFields.GenericMixed), typeof(Tuple<UsedType, Int32>))]
        [TestCase(nameof(ClassWithFields.GenericGeneric), typeof(List<Tuple<UsedType, Int32>>))]
        public void ReturnTypeIsCorrect(string fieldName, Type expectedType)
        {
            var field = GetFieldDefinition<ClassWithFields>(fieldName);

            var model = _scanner.ScanField(field, NetType<ClassWithFields>());

            Assert.That(model.Type.Name, Is.EqualTo(expectedType.Name));
        }

        [Test]
        [TestCase(nameof(ClassWithMefUsages.ExportField), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportFieldWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportFieldWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportFieldWithNameAndType), 1)]
        public void HasFieldExportTypes(string fieldName, int expectedValue)
        {
            var type = GetFieldDefinition<ClassWithMefUsages>(fieldName);

            var model = _scanner.ScanField(type, NetType<ClassWithMefUsages>());

            Assert.That(model.Exports.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        [TestCase(nameof(ClassWithMefUsages.ImportField), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportFieldWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportFieldWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportFieldWithNameAndType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyField), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyFieldWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyFieldWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportManyFieldWithNameAndType), 1)]
        public void HasPropertyImportTypes(string propertyName, int expectedValue)
        {
            var type = GetFieldDefinition<ClassWithMefUsages>(propertyName);

            var model = _scanner.ScanField(type, NetType<ClassWithMefUsages>());

            Assert.That(model.Imports.Count, Is.EqualTo(expectedValue));
        }
    }
}