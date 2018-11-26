namespace TestLibrary
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    public class TypeUsingOtherTypeInMethod
    {
        public UsedType UsingTypeAsMethodReturnType()
        {
            return null;
        }

        public void UsingTypeAsMethodParamaterType(UsedType usedType)
        {

        }

        public void UsingTypeAsVariableTypeInMethodBody()
        {
#pragma warning disable CS0168 // Variable is declared but never used
            UsedType usedType;
#pragma warning restore CS0168 // Variable is declared but never used
        }

        public void UsingTypeAsVariableAssignedTypeInMethodBody()
        {
            var usedType = new UsedType();
        }

        public void UsingTypeAsVariableSafeCastTypeInMethodBody()
        {
            var noType = new Object();
            var usedType = noType as UsedType;
        }

        public void UsingTypeAsVariableCastTypeInMethodBody()
        {
            var noType = new Object();
            var usedType = (UsedType)noType;
        }

        public void UsingTypeAsVariableListTypeInMethodBody()
        {
            var list = new List<UsedType>();
        }

        public void UsingDifferentTypesAsGenericArgumentsInMethodBody()
        {
            var value = new Tuple<UsedType, bool>(null, false);
        }

        public void UsingDifferentTypesInMultipleVariablesAsGenericArgumentsInMethodBody()
        {
            var value1 = new Tuple<UsedType, bool>(null, false);
            var value2 = new Tuple<int, float>(0, 0.0f);
        }

        public void UsingSameTypeAsGenericArgumentsInMethodBody()
        {
            var value = new Tuple<UsedType, UsedType>(null, null);
        }

        public void UsingTypeAsElementTypeInACollectionInMethodBody()
        {
            var list = new [] { new UsedType() };
        }

        public void UsingTypeInTypeOfInMethodBody()
        {
            var ofType = typeof(UsedType);
        }
    }
}