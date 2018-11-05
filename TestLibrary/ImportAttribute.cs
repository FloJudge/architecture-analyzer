namespace TestLibrary
{
    using System;
    public class ImportAttribute : Attribute
    {
        public ImportAttribute() {}
        public ImportAttribute(Type type) {}
        public ImportAttribute(string name) {}
        public ImportAttribute(string name, Type type) {}
    }

    public class ImportManyAttribute : Attribute
    {
        public ImportManyAttribute() { }
        public ImportManyAttribute(Type type) { }
        public ImportManyAttribute(string name) { }
        public ImportManyAttribute(string name, Type type) { }
    }

    public class ImportingConstructorAttribute : Attribute
    {
    }
}
