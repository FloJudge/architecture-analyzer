namespace ArchitectureAnalyzer.Net.Scanner.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using ArchitectureAnalyzer.Net.Model;

    using NUnit.Framework;

    using TestLibrary;

    [TestFixture]
    public class MethodScannerTest : MetadataScannerTestBase
    {
        private MethodScanner _scanner;

        [SetUp]
        public void SetupScanner()
        {
            _scanner = new MethodScanner(Module, ModelFactory, Logger);

            var assembly = new NetAssembly { Name = "TestLibrary" };

            var typeScanner = new TypeScanner(Module, ModelFactory, Logger);
            typeScanner.ScanType(GetTypeDefintion<ClassWithMembers>(), assembly);
            typeScanner.ScanType(GetTypeDefintion<ClassWithMefUsages>(), assembly);
            typeScanner.ScanType(GetTypeDefintion<InheritedFromClassWithMembers>(), assembly);
        }

        [Test]
        public void NameIsCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.SomeMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.Name, Is.EqualTo(nameof(ClassWithMembers.SomeMethod)));
        }

        [Test]
        public void ReturnTypeIsCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.IntMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.ReturnType.Name, Is.EqualTo(nameof(Int32)));
        }

        [Test]
        public void ParamterTypesAreCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.MethodWithParams));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.Parameters.Count, Is.EqualTo(2));

            Assert.That(model.Parameters[0].Name, Is.EqualTo("a"));
            Assert.That(model.Parameters[0].Order, Is.EqualTo(1));
            Assert.That(model.Parameters[0].Type.Name, Is.EqualTo(nameof(Int32)));

            Assert.That(model.Parameters[1].Name, Is.EqualTo("b"));
            Assert.That(model.Parameters[1].Order, Is.EqualTo(2));
            Assert.That(model.Parameters[1].Type.Name, Is.EqualTo(nameof(String)));
        }

        [Test]
        public void GenericParametersAreCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.GenericMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.GenericParameters.Count, Is.EqualTo(1));

            Assert.That(model.GenericParameters[0].Name, Is.EqualTo("TMethodArg"));
            Assert.That(model.GenericParameters[0].Type, Is.EqualTo(Net.Model.NetType.TypeClass.GenericTypeArg));
        }

        [Test]
        public void GenericMethodArgIsCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.GenericMethodArg));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.Parameters.Count, Is.EqualTo(1));

            Assert.That(model.Parameters[0].Name, Is.EqualTo("arg"));
            Assert.That(model.Parameters[0].Type.Name, Is.EqualTo("TMethodArg"));
        }

        [Test]
        public void IsStaticFlagIsSetForStaticMethod()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.StaticMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.IsStatic, Is.True);
            Assert.That(model.IsAbstract, Is.False);
            Assert.That(model.IsSealed, Is.False);
            Assert.That(model.IsGeneric, Is.False);
        }

        [Test]
        public void IsAbstractFlagIsSetForAbstractMethod()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.AbstractMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.IsStatic, Is.False);
            Assert.That(model.IsAbstract, Is.True);
            Assert.That(model.IsSealed, Is.False);
            Assert.That(model.IsGeneric, Is.False);
        }

        [Test]
        public void IsSealedFlagIsSetForSealedMethod()
        {
            var method = GetMethodDefinition<InheritedFromClassWithMembers>(nameof(InheritedFromClassWithMembers.AbstractMethod));

            var model = _scanner.ScanMethod(method, NetType<InheritedFromClassWithMembers>());

            Assert.That(model.IsStatic, Is.False);
            Assert.That(model.IsAbstract, Is.False);
            Assert.That(model.IsSealed, Is.True);
            Assert.That(model.IsGeneric, Is.False);
        }

        [Test]
        public void IsGenericFlagIsSetForGenericMethod()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.GenericMethod));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.IsStatic, Is.False);
            Assert.That(model.IsAbstract, Is.False);
            Assert.That(model.IsSealed, Is.False);
            Assert.That(model.IsGeneric, Is.True);
        }

        [TestCase(nameof(ClassWithMembers.PublicMethod), ExpectedResult = Visibility.Public)]
        [TestCase(nameof(ClassWithMembers.InternalMethod), ExpectedResult = Visibility.Internal)]
        [TestCase("ProtectedMethod", ExpectedResult = Visibility.Protected)]
        [TestCase("PrivateMethod", ExpectedResult = Visibility.Private)]
        [TestCase("DefaultVisibilityMethod", ExpectedResult = Visibility.Private)]
        public Visibility VisibilityIsCorrect(string methodName)
        {
            var method = GetMethodDefinition<ClassWithMembers>(methodName);

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            return model.Visibility;
        }

        [Test]
        public void GenericTypeInstantiationIsCorrect()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.ReturnTypeIsGenericTypeInstantiation));

            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            var returnType = model.ReturnType;

            Assert.That(returnType.IsGenericTypeInstantiation, Is.True);
            Assert.That(returnType.GenericType, Is.EqualTo(NetType(typeof(IEnumerable<>))));
            Assert.That(returnType.GenericTypeInstantiationArgs, Is.EqualTo(new[] { NetType<string>() }));
        }

        [Test]
        public void TupleMethodParametersAreAccepted()
        {
            var method = GetMethodDefinition<ClassWithMembers>(nameof(ClassWithMembers.TupleMethod));
            var model = _scanner.ScanMethod(method, NetType<ClassWithMembers>());

            Assert.That(model.GenericParameters.Count, Is.EqualTo(0));
        }

        [TestCase(nameof(ClassWithMefUsages.ExportMethod), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportMethodWithName), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportMethodWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ExportMethodWithNameAndType), 1)]
        public void HasMethodExportTypes(string propertyName, int expectedValue)
        {
            var method = GetMethodDefinition<ClassWithMefUsages>(propertyName);

            var model = _scanner.ScanMethod(method, NetType<ClassWithMefUsages>());

            Assert.That(model.Exports.Count, Is.EqualTo(expectedValue));
        }

        [TestCase(nameof(ClassWithMefUsages.ImportMethodVoidType), 0)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodImportType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithName), 0)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithNameAndType), 1)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithAttribute), 8)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithAttributeImportType), 9)]
        [TestCase(nameof(ClassWithMefUsages.ImportMethodWithoutAttribute), 8)]
        [TestCase(".ctor", 8)]
        public void HasMethodImportTypes(string methodName, int expectedValue)
        {
            var method = GetMethodDefinition<ClassWithMefUsages>(methodName);

            var model = _scanner.ScanMethod(method, NetType<ClassWithMefUsages>());

            Assert.That(model.Imports.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        [Description("it seems we get the return type twice :-P")]
        public void DoesPublicTypeUsesDependingTypeInMethod()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsMethodReturnType));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<UsedType>(), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodParameter()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsMethodParamaterType));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodBodyAsVariable()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsVariableTypeInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodBodyAsInstruction()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsVariableAssignedTypeInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodBodyAsSafeCast()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsVariableSafeCastTypeInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<Object>(), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodBodyAsCast()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsVariableCastTypeInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<Object>(), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDependingTypeInMethodBodyAsCollectionGeneric()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsVariableListTypeInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(List<>)), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDifferentTypesAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingDifferentTypesAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(Tuple<,>)), NetType<UsedType>(), NetType<bool>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDifferentTypesInMultipleVariablesAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingDifferentTypesInMultipleVariablesAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(Tuple<,>)), NetType(typeof(Tuple<,>)), NetType<UsedType>(), NetType<bool>(), NetType<int>(), NetType<float>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesSameTypeAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingSameTypeAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(Tuple<,>)), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesTypeAsElementTypeInACollectionInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingTypeAsElementTypeInACollectionInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }
        
        [Test]
        public void DoesPublicTypeUsesDifferentTypesInListTupleAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingDifferentTypesInListTupleAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(List<>)), NetType(typeof(Tuple<,>)), NetType<UsedType>(), NetType<bool>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDifferentTypesInListActionAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingDifferentTypesInListActionAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(List<>)), NetType(typeof(Action<>)), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }

        [Test]
        public void DoesPublicTypeUsesDifferentTypesInListListAsGenericArgumentsInMethodBody()
        {
            var method = GetMethodDefinition<TypeUsingOtherTypeInMethod>(nameof(TypeUsingOtherTypeInMethod.UsingDifferentTypesInListListAsGenericArgumentsInMethodBody));

            var model = _scanner.ScanMethod(method, NetType<TypeUsingOtherTypeInMethod>());

            var expectedTypes = new[] { NetType(typeof(List<>)), NetType(typeof(List<>)), NetType<UsedType>() };
            Assert.That(model.MethodTypes, Is.EquivalentTo(expectedTypes));
        }
    }
}
