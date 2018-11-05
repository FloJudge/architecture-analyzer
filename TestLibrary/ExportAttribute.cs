namespace TestLibrary
{
    using System;

    public class ExportAttribute : Attribute
    {
        public ExportAttribute() {}

        public ExportAttribute(Type type) {}

        public ExportAttribute(string name) {}

        public ExportAttribute(string name, Type type) {}
    }
}
