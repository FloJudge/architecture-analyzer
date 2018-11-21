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
            UsedType usedType;
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

        public void UsingTypeAsElementTypeInACollectionInMethodBody()
        {
            var list = new[] { new UsedType() };
        }

        public void UsingTypeInTypeOfInMethodBody()
        {
            var ofType = typeof(UsedType);
        }
    }
}