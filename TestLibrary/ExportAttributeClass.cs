namespace TestLibrary
{
    [Export]
    public class ExportAttributeClass
    {
    }

    [Export(typeof(IMefUsage))]
    public class ExportAttributeClassWithType
    {
    }

    [Export("IMefUsage")]
    public class ExportAttributeClassWithName
    {
    }

    [Export("IMefUsage", typeof(IMefUsage))]
    public class ExportAttributeClassWithNameAndType
    {
    }
}
