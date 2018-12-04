namespace ArchitectureAnalyzer.Net.Scanner.Test
{
    using System.Linq;

    using Mono.Cecil;

    using NUnit.Framework;

    using TestLibrary;

    [TestFixture]
    public class AssemblyScannerTest : MetadataScannerTestBase
    {
        private AssemblyScanner _scanner;

        private AssemblyDefinition Assembly => Module.Assembly;

        [SetUp]
        public void SetupScanner()
        {
            _scanner = new AssemblyScanner(Module, ModelFactory, Logger);
        }
        
        [Test]
        public void NameIsCorrect()
        {
            var assemblyModel = _scanner.Scan(Assembly);
            
            Assert.That(assemblyModel.Name, Is.EqualTo("TestLibrary"));
        }

        [Test]
        public void DefinedTypesAreCorrect()
        {
            var assemblyModel = _scanner.Scan(Assembly);

            var expectedTypes = new[]
                                    {
                                        nameof(AbstractClass),
                                        nameof(ClassWithMembers),
                                        nameof(ClassWithMefUsages),
                                        nameof(DefaultVisiblityClass),
                                        nameof(EmptyClass),
                                        nameof(ExportAttribute),
                                        nameof(ExportAttributeClass),
                                        nameof(ExportAttributeClassWithName),
                                        nameof(ExportAttributeClassWithType),
                                        nameof(ExportAttributeClassWithNameAndType),
                                        typeof(GenericClass<>).Name,
                                        nameof(IAgainExtendedInterface),
                                        nameof(IExtendedInterface),
                                        nameof(IInterface),
                                        nameof(IMefUsage),
                                        nameof(ImplementsInterface),
                                        nameof(ImportAttribute),
                                        nameof(ImportAttributeClass),
                                        nameof(ImportAttributeClassWithName),
                                        nameof(ImportAttributeClassWithType),
                                        nameof(ImportAttributeClassWithNameAndType),
                                        nameof(ImportingConstructorAttribute),
                                        nameof(ImportingConstructorAttributeClass),
                                        nameof(ImportManyAttribute),
                                        nameof(ImportManyAttributeClass),
                                        nameof(ImportManyAttributeClassWithName),
                                        nameof(ImportManyAttributeClassWithType),
                                        nameof(ImportManyAttributeClassWithNameAndType),
                                        nameof(InheritedClass),
                                        nameof(InheritedFromClassWithMembers),
                                        nameof(InheritsInterfaceFromBaseClass),
                                        nameof(InternalClass),
                                        nameof(SealedClass),
                                        nameof(SomeEnum),
                                        nameof(StaticClass),
                                        nameof(TestFixtureAttributedClass),
                                        nameof(UserDefinedAttribute),
                                        nameof(UserTypeAttributedClass),
                                        nameof(TypeUsageInProperty),
                                        nameof(TypeUsageInMethod),
                                        nameof(UsedType),
                                        nameof(ClassWithFields)
                                    };

            Assert.That(assemblyModel.DefinedTypes.Select(t => t.Name), Is.EquivalentTo(expectedTypes));
        }
    }
}
