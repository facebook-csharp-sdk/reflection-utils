namespace ReflectionUtils
{
    public class SimpleClass
    {
        // SimpleClass
        public SimpleClass()
        {
        }

        // StringField
        private string stringField;

        private string stringProperty { get; set; }

        // CreateInstance
        internal static SimpleClass CreateInstance()
        {
            return new SimpleClass();
        }
    }
}