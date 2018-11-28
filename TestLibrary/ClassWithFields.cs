namespace TestLibrary
{
    using System;
    using System.Collections.Generic;

    public class ClassWithFields
    {
        public int Primitive = 0;

        public UsedType Reference = new UsedType();

        public List<int> GenericPrimitive = new List<int>();

        public List<UsedType> GenericReference = new List<UsedType>();

        public Tuple<UsedType, int> GenericMixed = new Tuple<UsedType, int>(null, 0);

        public List<Tuple<UsedType, int>> GenericGeneric = new List<Tuple<UsedType, int>>();
    }
}