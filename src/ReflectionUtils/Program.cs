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

            SetValueUsingReflection(loops);
            SetValueUsingDynamicMethodCall(loops);

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

        private static void SetValueUsingReflection(int loops)
        {
            StartTest("Begin SetValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                fieldInfo.SetValue(simpleClass, "test");
            }

            EndTest("End SetValueUsingReflection");
        }

        // SetValueUsingDynamicMethodCall
        private static void SetValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin SetValueUsingDynamicMethodCall");

            SetHandler setHandler = ReflectionUtils.CreateSetHandler(type, fieldInfo);

            for (int i = 0; i < loops; i++)
            {
                setHandler(simpleClass, "test");
            }

            EndTest("End SetValueUsingDynamicMethodCall");
        }

        // GetValueUsingReflection
        private static void GetFieldValueUsingReflection(int loops)
        {
            StartTest("Begin GetValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)fieldInfo.GetValue(simpleClass);
            }

            EndTest("End GetValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private static void GetFieldValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetValueUsingDynamicMethodCall");

            GetHandler getHandler = ReflectionUtils.CreateGetHandler(type, fieldInfo);

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetValueUsingDynamicMethodCall");
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
