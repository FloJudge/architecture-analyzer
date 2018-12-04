// ReSharper disable UnusedVariable
namespace TestLibrary
{
    using System;
    using System.Collections.Generic;

    public class TypeUsageInProperty
    {
        public UsedType UsedTypeProperty { get; set; }

        #region Getter Methods

        public UsedType ReturnTypeInPropertyGetter
        {
            get
            {
                return new UsedType();
            }
        }

        public UsedType TypeDeclarationInPropertyGetter
        {
            get
            {
#pragma warning disable CS0168 // Variable is declared but never used
                UsedType usedType;
#pragma warning restore CS0168 // Variable is declared but never used
                return null;
            }
        }

        public UsedType InitTypeInPropertyGetter
        {
            get
            {
                var usedType = new UsedType();
                return null;
            }
        }

        public UsedType TypeSafeCastInPropertyGetter
        {
            get
            {
                var noType = new Object();
                var usedType = noType as UsedType;
                return null;
            }
        }

        public UsedType TypeCastInPropertyGetter
        {
            get
            {
                var noType = new Object();
                var usedType = (UsedType)noType;

                return null;
            }
        }

        public UsedType InitListTypeInPropertyGetter
        {
            get
            {
                var list = new List<UsedType>();

                return null;
            }
        }

        public UsedType InitTypesInTupleInPropertyGetter
        {
            get
            {
                var value = new Tuple<UsedType, bool>(null, false);

                return null;
            }
        }

        public UsedType InitMultipleTypesInMultipleTupleInPropertyGetter
        {
            get
            {
                var value1 = new Tuple<UsedType, bool>(null, false);
                var value2 = new Tuple<int, float>(0, 0.0f);

                return null;
            }
        }

        public UsedType InitListTupleTypesInPropertyGetter
        {
            get
            {
                var value = new List<Tuple<UsedType, bool>>() { new Tuple<UsedType, bool>(null, false) };
                return null;
            }
        }

        public UsedType InitListActionTypeInPropertyGetter
        {
            get
            {
                var value = new List<Action<UsedType>>() { new Action<UsedType>(type => { }) };

                return null;
            }
        }

        public UsedType InitListListTypeInPropertyGetter
        {
            get
            {
                var value = new List<List<UsedType>>() { new List<UsedType>() };
                return null;
            }
        }

        public UsedType InitTupleWithSameTypesInPropertyGetter
        {
            get
            {
                var value = new Tuple<UsedType, UsedType>(null, null);
                return null;
            }
        }

        public UsedType InitDynamicListTypInPropertyGetter
        {
            get
            {
                var list = new[] { new UsedType() };
                return null;
            }
        }

        public UsedType InitTypeOfTypeInPropertyGetter
        {
            get
            {
                var ofType = typeof(UsedType);
                return null;
            }
        }

        #endregion

        #region Setter Methods

        public UsedType UsedTypePropertyWithSetter
        {
            get => null;
            set
            {
                var usedType = new UsedType();
            }
        }

        public UsedType TypeDeclarationInPropertySetter
        {
            get => null;
            set
            {
#pragma warning disable CS0168 // Variable is declared but never used
                UsedType usedType;
#pragma warning restore CS0168 // Variable is declared but never used
            }
        }

        public UsedType InitTypeInPropertySetter
        {
            get => null;
            set
            {
                var usedType = new UsedType();
            }
        }

        public UsedType TypeSafeCastInPropertySetter
        {
            get => null;
            set
            {
                var noType = new Object();
                var usedType = noType as UsedType;
            }
        }

        public UsedType TypeCastInPropertySetter
        {
            get => null;
            set
            {
                var noType = new Object();
                var usedType = (UsedType)noType;
            }
        }

        public UsedType InitListTypeInPropertySetter
        {
            get => null;
            set
            {
                var list = new List<UsedType>();
            }
        }

        public UsedType InitTypesInTupleInPropertySetter
        {
            get => null;
            set
            {
                var valueType = new Tuple<UsedType, bool>(null, false);
            }
        }

        public UsedType InitMultipleTypesInMultipleTupleInPropertySetter
        {
            get => null;
            set
            {
                var value1 = new Tuple<UsedType, bool>(null, false);
                var value2 = new Tuple<int, float>(0, 0.0f);
            }
        }

        public UsedType InitListTupleTypesInPropertySetter
        {
            get => null;
            set
            {
                var valueType = new List<Tuple<UsedType, bool>>() { new Tuple<UsedType, bool>(null, false) };
            }
        }

        public UsedType InitListActionTypeInPropertySetter
        {
            get => null;
            set
            {
                var valueType = new List<Action<UsedType>>() { new Action<UsedType>(type => { }) };
            }
        }

        public UsedType InitListListTypeInPropertySetter
        {
            get => null;
            set
            {
                var valueType = new List<List<UsedType>>() { new List<UsedType>() };
            }
        }

        public UsedType InitTupleWithSameTypesInPropertySetter
        {
            get => null;
            set
            {
                var valueType = new Tuple<UsedType, UsedType>(null, null);
            }
        }

        public UsedType InitDynamicListTypInPropertySetter
        {
            get => null;
            set
            {
                var list = new[] { new UsedType() };
            }
        }

        public UsedType InitTypeOfTypeInPropertySetter
        {
            get => null;
            set
            {
                var ofType = typeof(UsedType);
            }
        }

        #endregion
    }
}