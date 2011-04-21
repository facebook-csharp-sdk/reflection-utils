namespace ReflectionUtils
{
    public class SimpleClass
    {
        // SimpleClass
        public SimpleClass()
        {
        }

        // StringField
        public string stringField;

        public string stringProperty { get; set; }

        // CreateInstance
        internal static SimpleClass CreateInstance()
        {
            return new SimpleClass();
        }
    }
}