//-----------------------------------------------------------------------
// <copyright file="SimpleClass.cs" company="Prabir Shrestha">
//    Copyright 2011 Prabir Shrestha
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