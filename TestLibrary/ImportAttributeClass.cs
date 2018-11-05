using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary
{
    [Import]
    public class ImportAttributeClass
    {
    }

    [Import(typeof(IMefUsage))]
    public class ImportAttributeClassWithType
    {
    }

    [Import("Interface")]
    public class ImportAttributeClassWithName
    {
    }

    [Import("Interface", typeof(IMefUsage))]
    public class ImportAttributeClassWithNameAndType
    {
    }

    [ImportMany]
    public class ImportManyAttributeClass
    {
    }

    [ImportMany(typeof(IMefUsage))]
    public class ImportManyAttributeClassWithType
    {
    }

    [ImportMany("Interface")]
    public class ImportManyAttributeClassWithName
    {
    }

    [ImportMany("Interface", typeof(IMefUsage))]
    public class ImportManyAttributeClassWithNameAndType
    {
    }

    [ImportingConstructor]
    public class ImportingConstructorAttributeClass
    {
    }
}
