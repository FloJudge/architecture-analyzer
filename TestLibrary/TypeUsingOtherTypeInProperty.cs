namespace TestLibrary
{
    public class TypeUsingOtherTypeInProperty
    {

        public UsedType UsedTypeProperty { get; set; }

        public UsedType UsedTypePropertyWithGetter
        {
            get
            {
                return new UsedType();
            }
        }

        public UsedType UsedTypePropertyWithSetter
        {
            set
            {
                var usedType = new UsedType();
            }
        }
    }
}
