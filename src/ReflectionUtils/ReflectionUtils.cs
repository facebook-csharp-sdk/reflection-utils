#define REFLECTION_UTILS_REFLECTIONEMIT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if REFLECTION_UTILS_REFLECTIONEMIT
using System.Reflection.Emit;
#endif

namespace ReflectionUtils
{
#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 class ReflectionUtils
    {
        public static Attribute GetAttribute(MemberInfo info, Type type)
        {
            if (info == null || type == null || !Attribute.IsDefined(info, type))
                return null;

            return Attribute.GetCustomAttribute(info, type);
        }

        public static Attribute GetAttribute(Type objectType, Type attributeType)
        {
            if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
                return null;

            return Attribute.GetCustomAttribute(objectType, attributeType);
        }

        public static bool IsTypeGenericeCollectionInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            Type genericDefinition = type.GetGenericTypeDefinition();

            return (genericDefinition == typeof(IList<>) || genericDefinition == typeof(ICollection<>) || genericDefinition == typeof(IEnumerable<>));
        }

        public static bool IsTypeDictionary(Type type)
        {
            if (typeof(IDictionary).IsAssignableFrom(type))
                return true;

            if (!type.IsGenericType)
                return false;

            Type genericDefinition = type.GetGenericTypeDefinition();
            return genericDefinition == typeof(IDictionary<,>);
        }
    }

#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 delegate object GetHandler(object source);

#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 delegate void SetHandler(object source, object value);

#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 delegate void MemberMapLoader(Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps);

#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 class CacheResolver
    {
        private readonly MemberMapLoader _memberMapLoader;
        private readonly SafeDictionary<Type, SafeDictionary<string, MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, MemberMap>>();

        delegate object CtorDelegate();
        readonly static SafeDictionary<Type, CtorDelegate> ConstructorCache = new SafeDictionary<Type, CtorDelegate>();

        public CacheResolver(MemberMapLoader memberMapLoader)
        {
            _memberMapLoader = memberMapLoader;
        }

        public static object GetNewInstance(Type type)
        {
            CtorDelegate c;
            if (ConstructorCache.TryGetValue(type, out c))
                return c();
#if REFLECTION_UTILS_REFLECTIONEMIT
                DynamicMethod dynamicMethod = new DynamicMethod("Create" + type.FullName, typeof(object), Type.EmptyTypes, type, true);
                dynamicMethod.InitLocals = true;
                ILGenerator generator = dynamicMethod.GetILGenerator();
                if (type.IsValueType)
                {
                    generator.DeclareLocal(type);
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Box, type);
                }
                else
                {
                    ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                    if (constructorInfo == null)
                        throw new Exception(string.Format("Could not get constructor for {0}.", type));
                    generator.Emit(OpCodes.Newobj, constructorInfo);
                }
                generator.Emit(OpCodes.Ret);
                c = (CtorDelegate)dynamicMethod.CreateDelegate(typeof(CtorDelegate));
                ConstructorCache.Add(type, c);
                return c();
#else
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            c = delegate { return constructorInfo.Invoke(null); };
            ConstructorCache.Add(type, c);
            return c();
#endif
        }

        public SafeDictionary<string, MemberMap> LoadMaps(Type type)
        {
            if (type == null || type == typeof(object))
                return null;
            SafeDictionary<string, MemberMap> maps;
            if (_memberMapsCache.TryGetValue(type, out maps))
                return maps;
            maps = new SafeDictionary<string, MemberMap>();
            _memberMapLoader(type, maps);
            _memberMapsCache.Add(type, maps);
            return maps;
        }

#if REFLECTION_UTILS_REFLECTIONEMIT
            static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
            {
                DynamicMethod dynamicMethod = !owner.IsInterface
                  ? new DynamicMethod(name, returnType, parameterTypes, owner, true)
                  : new DynamicMethod(name, returnType, parameterTypes, (Module)null, true);

                return dynamicMethod;
            }
#endif

        static GetHandler CreateGetHandler(FieldInfo fieldInfo)
        {
#if REFLECTION_UTILS_REFLECTIONEMIT
                Type type = fieldInfo.FieldType;
                DynamicMethod dynamicGet = CreateDynamicMethod("Get" + fieldInfo.Name, fieldInfo.DeclaringType, new Type[] { typeof(object) }, fieldInfo.DeclaringType);
                ILGenerator getGenerator = dynamicGet.GetILGenerator();

                getGenerator.Emit(OpCodes.Ldarg_0);
                getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
                if (type.IsValueType)
                    getGenerator.Emit(OpCodes.Box, type);
                getGenerator.Emit(OpCodes.Ret);

                return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
#else
            return delegate(object instance) { return fieldInfo.GetValue(instance); };
#endif
        }

        static SetHandler CreateSetHandler(FieldInfo fieldInfo)
        {
            if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
                return null;
#if REFLECTION_UTILS_REFLECTIONEMIT
                Type type = fieldInfo.FieldType;
                DynamicMethod dynamicSet = CreateDynamicMethod("Set" + fieldInfo.Name, null, new Type[] { typeof(object), typeof(object) }, fieldInfo.DeclaringType);
                ILGenerator setGenerator = dynamicSet.GetILGenerator();

                setGenerator.Emit(OpCodes.Ldarg_0);
                setGenerator.Emit(OpCodes.Ldarg_1);
                if (type.IsValueType)
                    setGenerator.Emit(OpCodes.Unbox_Any, type);
                setGenerator.Emit(OpCodes.Stfld, fieldInfo);
                setGenerator.Emit(OpCodes.Ret);

                return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
#else
            return delegate(object instance, object value) { fieldInfo.SetValue(instance, value); };
#endif
        }

        static GetHandler CreateGetHandler(PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            if (getMethodInfo == null)
                return null;
#if REFLECTION_UTILS_REFLECTIONEMIT
                Type type = propertyInfo.PropertyType;
                DynamicMethod dynamicGet = CreateDynamicMethod("Get" + propertyInfo.Name, propertyInfo.DeclaringType, new Type[] { typeof(object) }, propertyInfo.DeclaringType);
                ILGenerator getGenerator = dynamicGet.GetILGenerator();

                getGenerator.Emit(OpCodes.Ldarg_0);
                getGenerator.Emit(OpCodes.Call, getMethodInfo);
                if (type.IsValueType)
                    getGenerator.Emit(OpCodes.Box, type);
                getGenerator.Emit(OpCodes.Ret);

                return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
#else
            return delegate(object instance) { return getMethodInfo.Invoke(instance, Type.EmptyTypes); };
#endif
        }

        static SetHandler CreateSetHandler(PropertyInfo propertyInfo)
        {
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            if (setMethodInfo == null)
                return null;
#if REFLECTION_UTILS_REFLECTIONEMIT
                Type type = propertyInfo.PropertyType;
                DynamicMethod dynamicSet = CreateDynamicMethod("Set" + propertyInfo.Name, null, new Type[] { typeof(object), typeof(object) }, propertyInfo.DeclaringType);
                ILGenerator setGenerator = dynamicSet.GetILGenerator();

                setGenerator.Emit(OpCodes.Ldarg_0);
                setGenerator.Emit(OpCodes.Ldarg_1);
                if (type.IsValueType)
                    setGenerator.Emit(OpCodes.Unbox_Any, type);
                setGenerator.Emit(OpCodes.Call, setMethodInfo);
                setGenerator.Emit(OpCodes.Ret);
                return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
#else
            return delegate(object instance, object value) { setMethodInfo.Invoke(instance, new[] { value }); };
#endif
        }

#if REFLECTION_UTILS_INTERNAL
    internal
#else
        public
#endif
 sealed class MemberMap
        {
            public readonly MemberInfo MemberInfo;
            public readonly Type Type;
            public readonly GetHandler Getter;
            public readonly SetHandler Setter;

            public MemberMap(PropertyInfo propertyInfo)
            {
                MemberInfo = propertyInfo;
                Type = propertyInfo.PropertyType;
                Getter = CreateGetHandler(propertyInfo);
                Setter = CreateSetHandler(propertyInfo);
            }

            public MemberMap(FieldInfo fieldInfo)
            {
                MemberInfo = fieldInfo;
                Type = fieldInfo.FieldType;
                Getter = CreateGetHandler(fieldInfo);
                Setter = CreateSetHandler(fieldInfo);
            }
        }
    }

#if REFLECTION_UTILS_INTERNAL
    internal
#else
    public
#endif
 class SafeDictionary<TKey, TValue>
    {
        private readonly object _padlock = new object();
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            lock (_padlock)
            {
                if (_dictionary.ContainsKey(key) == false)
                    _dictionary.Add(key, value);
            }
        }
    }
}