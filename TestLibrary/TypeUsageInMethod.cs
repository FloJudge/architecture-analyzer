namespace TestLibrary
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    public class TypeUsageInMethod
    {
        public UsedType ReturnTypeMethod()
        {
            return null;
        }

        public void MethodParameterTypeMethod(UsedType usedType)
        {

        }

        public void TypeDeclarationInMethod()
        {
#pragma warning disable CS0168 // Variable is declared but never used
            UsedType usedType;
#pragma warning restore CS0168 // Variable is declared but never used
        }

        public void InitTypeInMethod()
        {
            var usedType = new UsedType();
        }

        public void TypeSafeCastInMethod()
        {
            var noType = new Object();
            var usedType = noType as UsedType;
        }

        public void TypeCastInMethod()
        {
            var noType = new Object();
            var usedType = (UsedType)noType;
        }

        public void InitListTypeInMethod()
        {
            var list = new List<UsedType>();
        }

        public void InitTypesInTupleInMethod()
        {
            var value = new Tuple<UsedType, bool>(null, false);
        }

        public void InitMultipleTypesInMultipleTupleInMethod()
        {
            var value1 = new Tuple<UsedType, bool>(null, false);
            var value2 = new Tuple<int, float>(0, 0.0f);
        }

        public void InitListTupleTypesInMethod()
        {
            var value = new List<Tuple<UsedType, bool>>() { new Tuple<UsedType, bool>(null, false)};
        }

        public void InitListActionTypeInMethod()
        {
            var value = new List<Action<UsedType>>() { new Action<UsedType>(type => {})};
        }

        public void InitListListTypeInMethod()
        {
            var value = new List<List<UsedType>>() { new List<UsedType>()};
        }

        public void InitTupleWithSameTypesInMethod()
        {
            var value = new Tuple<UsedType, UsedType>(null, null);
        }

        public void InitDynamicListTypInMethod()
        {
            var list = new [] { new UsedType() };
        }

        public void InitTypeOfTypeInMethod()
        {
            var ofType = typeof(UsedType);
        }
    }
}