namespace TestLibrary
{
    [Export(typeof(IMefUsage))]
    public abstract class ClassWithMefUsages : IMefUsage
    {
        #region Import MEF Field

        [Import]
        public IMefUsage ImportField;

        [Import(typeof(IMefUsage))]
        public IMefUsage ImportFieldWithType;

        [Import("Name")]
        public IMefUsage ImportFieldWithName;

        [Import("Name", typeof(IMefUsage))]
        public IMefUsage ImportFieldWithNameAndType;

        [ImportMany]
        public IMefUsage ImportManyField;

        [ImportMany("Name")]
        public IMefUsage ImportManyFieldWithName;

        [ImportMany(typeof(IMefUsage))]
        public IMefUsage ImportManyFieldWithType;

        [ImportMany("Name", typeof(IMefUsage))]
        public IMefUsage ImportManyFieldWithNameAndType;

        #endregion

        #region Export MEF Field

        [Export]
        public IMefUsage ExportField;

        [Export("Name")]
        public IMefUsage ExportFieldWithName;

        [Export(typeof(IMefUsage))]
        public IMefUsage ExportFieldWithType;

        [Export("Name", typeof(IMefUsage))]
        public IMefUsage ExportFieldWithNameAndType;

        #endregion

        #region Import MEF Property

        [Import]
        public IMefUsage ImportProperty { get; set; }

        [Import(typeof(IMefUsage))]
        public IMefUsage ImportPropertyWithType { get; set; }

        [Import("Name")]
        public IMefUsage ImportPropertyWithName { get; set; }

        [Import("Name", typeof(IMefUsage))]
        public IMefUsage ImportPropertyWithNameAndType { get; set; }

        [ImportMany]
        public IMefUsage ImportManyProperty { get; set; }

        [ImportMany("Name")]
        public IMefUsage ImportManyPropertyWithName { get; set; }

        [ImportMany(typeof(IMefUsage))]
        public IMefUsage ImportManyPropertyWithType { get; set; }

        [ImportMany("Name", typeof(IMefUsage))]
        public IMefUsage ImportManyPropertyWithNameAndType { get; set; }

        #endregion

        #region Export MEF Property

        [Export]
        public IMefUsage ExportProperty { get; set; }

        [Export("Name")]
        public IMefUsage ExportPropertyWithName { get; set; }

        [Export(typeof(IMefUsage))]
        public IMefUsage ExportPropertyWithType { get; set; }

        [Export("Name", typeof(IMefUsage))]
        public IMefUsage ExportPropertyWithNameAndType { get; set; }

        #endregion

        [ImportingConstructor]
        public ClassWithMefUsages([Import] IMefUsage import,
                                  [Import("Name")] IMefUsage importWithName,
                                  [Import(typeof(IMefUsage))] IMefUsage importWithType,
                                  [Import("Name", typeof(IMefUsage))] IMefUsage importWithNameAndType,
                                  [ImportMany] IMefUsage importMany,
                                  [ImportMany("Name")] IMefUsage importManyWithName,
                                  [ImportMany(typeof(IMefUsage))] IMefUsage importManyWithType,
                                  [ImportMany("Name", typeof(IMefUsage))] IMefUsage importManyWithNameAndType)
        {
        }

        #region Import MEF Method
        
        public void ImportMethodWithoutAttribute(
            [Import] IMefUsage import,
            [Import("Name")] IMefUsage importWithName,
            [Import(typeof(IMefUsage))] IMefUsage importWithType,
            [Import("Name", typeof(IMefUsage))] IMefUsage importWithNameAndType,
            [ImportMany] IMefUsage importMany,
            [ImportMany("Name")] IMefUsage importManyWithName,
            [ImportMany(typeof(IMefUsage))] IMefUsage importManyWithType,
            [ImportMany("Name", typeof(IMefUsage))] IMefUsage importManyWithNameAndType)
        {
        }

        [Import]
        public void ImportMethodVoidType()
        {
        }

        [Import]
        public IMefUsage ImportMethodImportType()
        {
            return ImportProperty;
        }

        [Import("Name")]
        public void ImportMethodWithName()
        {
        }

        [Import(typeof(IMefUsage))]
        public void ImportMethodWithType()
        {
        }

        [Import("Name", typeof(IMefUsage))]
        public void ImportMethodWithNameAndType()
        {
        }

        [Import]
        public void ImportMethodWithAttribute(
            [Import] IMefUsage import,
            [Import("Name")] IMefUsage importWithName,
            [Import(typeof(IMefUsage))] IMefUsage importWithType,
            [Import("Name", typeof(IMefUsage))] IMefUsage importWithNameAndType,
            [ImportMany] IMefUsage importMany,
            [ImportMany("Name")] IMefUsage importManyWithName,
            [ImportMany(typeof(IMefUsage))] IMefUsage importManyWithType,
            [ImportMany("Name", typeof(IMefUsage))] IMefUsage importManyWithNameAndType)
        {
        }

        [Import]
        public IMefUsage ImportMethodWithAttributeImportType(
            [Import] IMefUsage import,
            [Import("Name")] IMefUsage importWithName,
            [Import(typeof(IMefUsage))] IMefUsage importWithType,
            [Import("Name", typeof(IMefUsage))] IMefUsage importWithNameAndType,
            [ImportMany] IMefUsage importMany,
            [ImportMany("Name")] IMefUsage importManyWithName,
            [ImportMany(typeof(IMefUsage))] IMefUsage importManyWithType,
            [ImportMany("Name", typeof(IMefUsage))] IMefUsage importManyWithNameAndType)
        {
            return ImportProperty;
        }

        #endregion

        #region Export MEF Method

        [Export]
        public IMefUsage ExportMethod()
        {
            return null;
        }

        [Export("Name")]
        public IMefUsage ExportMethodWithName()
        {
            return null;
        }

        [Export(typeof(IMefUsage))]
        public IMefUsage ExportMethodWithType()
        {
            return null;
        }

        [Export("Name", typeof(IMefUsage))]
        public IMefUsage ExportMethodWithNameAndType()
        {
            return null;
        }

        #endregion
    }
}
