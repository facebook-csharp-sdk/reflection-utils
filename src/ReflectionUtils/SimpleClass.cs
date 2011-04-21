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

        // CreateInstance
        internal static SimpleClass CreateInstance()
        {
            return new SimpleClass();
        }
    }
}