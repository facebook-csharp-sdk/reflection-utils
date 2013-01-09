//-----------------------------------------------------------------------
// <copyright file="ReflectionUtilsBenchmarks.cs" company="The Outercurve Foundation">
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
// <website>https://github.com/facebook-csharp-sdk/reflection-utils</website>
//-----------------------------------------------------------------------

namespace ReflectionUtils
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public class ReflectionUtilsBenchmarks
    {
        public const int Loops = 1000000;

        public static PropertyInfo SamplePropertyInfo = ReflectionUtils.GetProperties(typeof(SimpleClass)).Single(p => p.Name == "Prop");
        public static FieldInfo SampleFieldInfo = ReflectionUtils.GetFields(typeof(SimpleClass)).Single(p => p.Name == "Field");

        private static Action<string> _writer;

        public static void Run(Action<string> writer)
        {
            _writer = writer;

            GetConstructorByReflection();
            GetConstructorByExpression();

            GetPropertyByReflection();
            GetPropertyByExpression();

            SetPropertyByReflection();
            SetPropertyByExpression();

            GetFieldByReflection();
            GetFieldByExpression();

            SetFieldByReflection();
            SetFieldByExpression();
        }

        private static void GetConstructorByReflection()
        {
#if REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(
                    type => ReflectionUtils.GetConstructorByReflection(type));

            using (new Profiler("ctor method invoke", _writer))
            {
                var obj = cache[typeof(SimpleClass)]();
            }

            using (new Profiler(_writer))
            {
                var obj = cache[typeof(SimpleClass)]();
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var obj = cache[typeof(SimpleClass)]();
                }
            }
#endif
        }

        private static void GetConstructorByExpression()
        {
#if !REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<Type, ReflectionUtils.ConstructorDelegate>(
                    type => ReflectionUtils.GetConstructorByExpression(type));

            using (new Profiler("ctor expression", _writer))
            {
                var obj = cache[typeof(SimpleClass)]();
            }

            using (new Profiler(_writer))
            {
                var obj = cache[typeof(SimpleClass)]();
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var obj = cache[typeof(SimpleClass)]();
                }
            }
#endif
        }

        private static void GetPropertyByReflection()
        {
#if REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<PropertyInfo, ReflectionUtils.GetDelegate>(
                    ReflectionUtils.GetGetMethodByReflection);

            var obj = new SimpleClass();
            using (new Profiler("prop.get method invoke", _writer))
            {
                var getter = cache[SamplePropertyInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                var getter = cache[SamplePropertyInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var getter = cache[SamplePropertyInfo];
                    var value = getter(obj);
                }
            }
#endif
        }

        private static void GetPropertyByExpression()
        {
#if !REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<PropertyInfo, ReflectionUtils.GetDelegate>(
                    ReflectionUtils.GetGetMethodByExpression);

            var obj = new SimpleClass();
            using (new Profiler("prop.get expression", _writer))
            {
                var getter = cache[SamplePropertyInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                var getter = cache[SamplePropertyInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var getter = cache[SamplePropertyInfo];
                    var value = getter(obj);
                }
            }
#endif
        }

        private static void SetPropertyByReflection()
        {
#if REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<PropertyInfo, ReflectionUtils.SetDelegate>(
                    ReflectionUtils.GetSetMethodByReflection);

            var obj = new SimpleClass();
            using (new Profiler("prop.set method invoke", _writer))
            {
                var setter = cache[SamplePropertyInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                var setter = cache[SamplePropertyInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var setter = cache[SamplePropertyInfo];
                    setter(obj, "val");
                }
            }
#endif
        }

        private static void SetPropertyByExpression()
        {
#if !REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<PropertyInfo, ReflectionUtils.SetDelegate>(
                    ReflectionUtils.GetSetMethodByExpression);

            var obj = new SimpleClass();
            using (new Profiler("prop.set expression", _writer))
            {
                var setter = cache[SamplePropertyInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                var setter = cache[SamplePropertyInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var setter = cache[SamplePropertyInfo];
                    setter(obj, "val");
                }
            }
#endif
        }

        private static void GetFieldByReflection()
        {
#if REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<FieldInfo, ReflectionUtils.GetDelegate>(
                    ReflectionUtils.GetGetMethodByReflection);

            var obj = new SimpleClass();
            using (new Profiler("field.get method invoke", _writer))
            {
                var getter = cache[SampleFieldInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                var getter = cache[SampleFieldInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var getter = cache[SampleFieldInfo];
                    var value = getter(obj);
                }
            }
#endif
        }

        private static void GetFieldByExpression()
        {
#if !REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<FieldInfo, ReflectionUtils.GetDelegate>(
                    ReflectionUtils.GetGetMethodByExpression);

            var obj = new SimpleClass();
            using (new Profiler("field.get expression", _writer))
            {
                var getter = cache[SampleFieldInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                var getter = cache[SampleFieldInfo];
                var value = getter(obj);
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var getter = cache[SampleFieldInfo];
                    var value = getter(obj);
                }
            }
#endif
        }

        private static void SetFieldByReflection()
        {
#if REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<FieldInfo, ReflectionUtils.SetDelegate>(
                    ReflectionUtils.GetSetMethodByReflection);

            var obj = new SimpleClass();
            using (new Profiler("field.set method invoke", _writer))
            {
                var setter = cache[SampleFieldInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                var setter = cache[SampleFieldInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var setter = cache[SampleFieldInfo];
                    setter(obj, "val");
                }
            }
#endif
        }

        private static void SetFieldByExpression()
        {
#if !REFLECTION_UTILS_NO_LINQ_EXPRESSION
            var cache =
                new ReflectionUtils.ThreadSafeDictionary<FieldInfo, ReflectionUtils.SetDelegate>(
                    ReflectionUtils.GetSetMethodByExpression);

            var obj = new SimpleClass();
            using (new Profiler("field.set expression", _writer))
            {
                var setter = cache[SampleFieldInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                var setter = cache[SampleFieldInfo];
                setter(obj, "val");
            }

            using (new Profiler(_writer))
            {
                for (int i = 0; i < Loops; i++)
                {
                    var setter = cache[SampleFieldInfo];
                    setter(obj, "val");
                }
            }
#endif
        }
    }

    public class Profiler : IDisposable
    {
        private readonly Action<string> _writer;
        private readonly Stopwatch _stopwatch;

        public Profiler(string message, Action<string> writer)
        {
            _writer = writer;
            writer(message);
            _stopwatch = Stopwatch.StartNew();
        }

        public Profiler(Action<string> writer)
        {
            _writer = writer;
            _stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _writer(_stopwatch.Elapsed.ToString());
        }
    }

    public class SimpleClass
    {
        // SimpleClass
        public SimpleClass()
        {
        }

        private SimpleClass(Type t)
        {
        }

        // StringField
        public string stringField;

        public string stringProperty { get; set; }

        public string Prop { get; set; }

        public string Field;

        // CreateInstance
        internal static SimpleClass CreateInstance()
        {
            return new SimpleClass();
        }
    }
}