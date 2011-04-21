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

        static void Main(string[] args)
        {
            Console.WriteLine("How Many Test Iterations Would You Like To Run?");
            //int loops = int.Parse(Console.ReadLine());

            int loops = 10000;

            CreateObjectUsingReflection(loops);
            CreateObjectUsingDynamicMethodCall(loops);

            GetValueUsingReflection(loops);
            GetValueUsingDynamicMethodCall(loops);
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

        // GetValueUsingReflection
        private static void GetValueUsingReflection(int loops)
        {
            StartTest("Begin GetValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)fieldInfo.GetValue(simpleClass);
            }

            EndTest("End GetValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private static void GetValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetValueUsingDynamicMethodCall");

            GetHandler getHandler = ReflectionUtils.CreateGetHandler(type, fieldInfo);

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetValueUsingDynamicMethodCall");
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
