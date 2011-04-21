using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ReflectionUtils
{
    class Program
    {
        const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static DateTime lastTestStartTime;
        private static Type type = typeof(SimpleClass);
        private static SimpleClass simpleClass = SimpleClass.CreateInstance();
        private static FieldInfo fieldInfo = type.GetField("stringField", BINDING_FLAGS);
        private static PropertyInfo propertyInfo = type.GetProperty("stringProperty", BINDING_FLAGS);

        static void Main(string[] args)
        {
            Console.WriteLine("How Many Test Iterations Would You Like To Run?");
            //int loops = int.Parse(Console.ReadLine());

            int loops = 10000;

            CreateObjectUsingReflection(loops);
            CreateObjectUsingDynamicMethodCall(loops);

            SetFieldValueUsingReflection(loops);
            SetFieldValueUsingDynamicMethodCall(loops);

            SetFieldValueUsingReflection(loops);
            SetFieldValueUsingDynamicMethodCall(loops);

            GetFieldValueUsingReflection(loops);
            GetFieldValueUsingDynamicMethodCall(loops);

            GetPropertyValueUsingReflection(loops);
            GetPropertyValueUsingDynamicMethodCall(loops);
        }

        private static void CreateObjectUsingReflection(int loops)
        {
            StartTest("Begin CreateObjectUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                object result = Activator.CreateInstance(type);
            }

            EndTest("End CreateObjectUsingReflection");
        }

        // CreateObjectUsingDynamicMethodCall
        private static void CreateObjectUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin CreateObjectUsingDynamicMethodCall");


            for (int i = 0; i < loops; i++)
            {
                object result = ReflectionUtils.Instance.GetNewInstance(type);
            }

            EndTest("End CreateObjectUsingDynamicMethodCall");
        }

        private static void SetFieldValueUsingReflection(int loops)
        {
            StartTest("Begin SetFieldValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                fieldInfo.SetValue(simpleClass, "test");
            }

            EndTest("End SetFieldValueUsingReflection");
        }

        // SetValueUsingDynamicMethodCall
        private static void SetFieldValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin SetFieldValueUsingDynamicMethodCall");

            SetHandler setHandler = ReflectionUtils.CreateSetHandler(type, fieldInfo);

            for (int i = 0; i < loops; i++)
            {
                setHandler(simpleClass, "test");
            }

            EndTest("End SetFieldValueUsingDynamicMethodCall");
        }

        private static void SetPropertyValueUsingReflection(int loops)
        {
            StartTest("Begin SetPropertyValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                propertyInfo.SetValue(simpleClass, "test", null);
            }

            EndTest("End SetPropertyValueUsingReflection");
        }

        // SetValueUsingDynamicMethodCall
        private static void SetPropertyValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin SetPropertyValueUsingDynamicMethodCall");

            SetHandler setHandler = ReflectionUtils.CreateSetHandler(type, propertyInfo);

            for (int i = 0; i < loops; i++)
            {
                setHandler(simpleClass, "test");
            }

            EndTest("End SetPropertyValueUsingDynamicMethodCall");
        }

        // GetValueUsingReflection
        private static void GetFieldValueUsingReflection(int loops)
        {
            StartTest("Begin GetFieldValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)fieldInfo.GetValue(simpleClass);
            }

            EndTest("End GetFieldValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private static void GetFieldValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetFieldValueUsingDynamicMethodCall");

            GetHandler getHandler = ReflectionUtils.CreateGetHandler(type, fieldInfo);

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetFieldValueUsingDynamicMethodCall");
        }

        // GetValueUsingReflection
        private static void GetPropertyValueUsingReflection(int loops)
        {
            StartTest("Begin GetPropertyValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)propertyInfo.GetValue(simpleClass, null);
            }

            EndTest("End GetPropertyValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private static void GetPropertyValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetPropertyValueUsingDynamicMethodCall");

            GetHandler getHandler = ReflectionUtils.CreateGetHandler(type, propertyInfo);

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetPropertyValueUsingDynamicMethodCall");
        }

        // StartTest
        private static void StartTest(string message)
        {
            lastTestStartTime = DateTime.Now;
            Console.WriteLine(message);
        }

        // EndTest
        private static void EndTest(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(DateTime.Now - lastTestStartTime);
            Console.WriteLine("");
        }
    }
}
