//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="The Outercurve Foundation">
//    Copyright (c) 2011, The Outercurve Foundation. 
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <author>Prabir Shrestha (prabir.me)</author>
// <website>https://github.com/facebook-csharp-sdk/ReflectionUtils</website>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Reflection;
using ReflectionUtils;

namespace ReflectionUtils_WinRT
{
    partial class MainPage
    {
        //const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static DateTime lastTestStartTime;
        private static Type type = typeof(SimpleClass);
        private static SimpleClass simpleClass = SimpleClass.CreateInstance();
        private static FieldInfo fieldInfo = type.GetTypeInfo().GetDeclaredField("stringField");
        private static PropertyInfo propertyInfo = type.GetTypeInfo().GetDeclaredProperty("stringProperty");

        public MainPage()
        {
            InitializeComponent();

            int loops = 10000;
            /*
            CreateObjectUsingReflection(loops);
            CreateObjectUsingDynamicMethodCall(loops);

            SetFieldValueUsingReflection(loops);
            SetFieldValueUsingDynamicMethodCall(loops);

            SetFieldValueUsingReflection(loops);
            SetFieldValueUsingDynamicMethodCall(loops);

            GetFieldValueUsingReflection(loops);
            GetFieldValueUsingDynamicMethodCall(loops);

            GetPropertyValueUsingReflection(loops);
            GetPropertyValueUsingDynamicMethodCall(loops);*/
        }

        /*
        private void CreateObjectUsingReflection(int loops)
        {
            StartTest("Begin CreateObjectUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                object result = Activator.CreateInstance(type);
            }

            EndTest("End CreateObjectUsingReflection");
        }

        // CreateObjectUsingDynamicMethodCall
        private void CreateObjectUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin CreateObjectUsingDynamicMethodCall");


            for (int i = 0; i < loops; i++)
            {
                object result = CacheResolver.GetNewInstance(type);
            }

            EndTest("End CreateObjectUsingDynamicMethodCall");
        }

        private void SetFieldValueUsingReflection(int loops)
        {
            StartTest("Begin SetFieldValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                fieldInfo.SetValue(simpleClass, "test");
            }

            EndTest("End SetFieldValueUsingReflection");
        }

        // SetValueUsingDynamicMethodCall
        private void SetFieldValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin SetFieldValueUsingDynamicMethodCall");
            SetHandler setHandler = new CacheResolver.MemberMap(fieldInfo).Setter;

            for (int i = 0; i < loops; i++)
            {
                setHandler(simpleClass, "test");
            }

            EndTest("End SetFieldValueUsingDynamicMethodCall");
        }

        private void SetPropertyValueUsingReflection(int loops)
        {
            StartTest("Begin SetPropertyValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                propertyInfo.SetValue(simpleClass, "test", null);
            }

            EndTest("End SetPropertyValueUsingReflection");
        }

        // SetValueUsingDynamicMethodCall
        private void SetPropertyValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin SetPropertyValueUsingDynamicMethodCall");
            SetHandler setHandler = new CacheResolver.MemberMap(propertyInfo).Setter;

            for (int i = 0; i < loops; i++)
            {

                setHandler(simpleClass, "test");
            }

            EndTest("End SetPropertyValueUsingDynamicMethodCall");
        }

        // GetValueUsingReflection
        private void GetFieldValueUsingReflection(int loops)
        {
            StartTest("Begin GetFieldValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)fieldInfo.GetValue(simpleClass);
            }

            EndTest("End GetFieldValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private void GetFieldValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetFieldValueUsingDynamicMethodCall");
            GetHandler getHandler = new CacheResolver.MemberMap(fieldInfo).Getter;

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetFieldValueUsingDynamicMethodCall");
        }

        // GetValueUsingReflection
        private void GetPropertyValueUsingReflection(int loops)
        {
            StartTest("Begin GetPropertyValueUsingReflection");

            for (int i = 0; i < loops; i++)
            {
                string value = (string)propertyInfo.GetValue(simpleClass, null);
            }

            EndTest("End GetPropertyValueUsingReflection");
        }

        // GetValueUsingDynamicMethodCall
        private void GetPropertyValueUsingDynamicMethodCall(int loops)
        {
            StartTest("Begin GetPropertyValueUsingDynamicMethodCall");
            GetHandler getHandler = new CacheResolver.MemberMap(propertyInfo).Getter;

            for (int i = 0; i < loops; i++)
            {
                string value = (string)getHandler(simpleClass);
            }

            EndTest("End GetPropertyValueUsingDynamicMethodCall");
        }

        // StartTest
        private void StartTest(string message)
        {
            lastTestStartTime = DateTime.Now;
            result.Text += message + Environment.NewLine;
            //Console.WriteLine(message);
        }

        // EndTest
        private void EndTest(string message)
        {
            result.Text += message + Environment.NewLine;
            result.Text += (DateTime.Now - lastTestStartTime) + Environment.NewLine;
            result.Text += Environment.NewLine;
            //Console.WriteLine(message);
            //Console.WriteLine(DateTime.Now - lastTestStartTime);
            //Console.WriteLine("");
        }
          
        */
    }
}
