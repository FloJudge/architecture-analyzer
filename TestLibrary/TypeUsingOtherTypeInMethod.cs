namespace TestLibrary
{
    public class TypeUsingOtherTypeInMethod
    {
        public UsedType UsingTypeAsReturnValueMethod()
        {
            return null;
        }

        public void UsingTypeAsParameterMethod(UsedType usedType)
        {
            
        }

        public void UsingTypeAsVariableInMethodBody()
        {
            UsedType usedType;
        }
    }
}