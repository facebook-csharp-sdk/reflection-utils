#define REFLECTION_EMIT

using System;
using System.Collections.Generic;
using System.Reflection;
#if REFLECTION_EMIT
using System.Reflection.Emit;
#endif

namespace ReflectionUtils
{
    internal delegate object GetHandler(object source);

    internal delegate void SetHandler(object source, object value);

    internal delegate void MemberMapLoader(Type type, SafeDictionary<string, ResolverCache.MemberMap> memberMaps);

    internal class ResolverCache
    {
        private readonly MemberMapLoader _memberMapLoader;
        private readonly SafeDictionary<Type, SafeDictionary<string, MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, MemberMap>>();

#if REFLECTION_EMIT
        delegate object CtorDelegate();
#endif

#if REFLECTION_EMIT
        readonly static SafeDictionary<Type, CtorDelegate> _constructorCache = new SafeDictionary<Type, CtorDelegate>();
#endif

        public ResolverCache(MemberMapLoader memberMapLoader)
        {
            _memberMapLoader = memberMapLoader;
        }

        public static object GetNewInstance(Type type)
        {
#if REFLECTION_EMIT
            CtorDelegate c;
            if (_constructorCache.TryGetValue(type, out c))
                return c();
            DynamicMethod dynMethod = new DynamicMethod("_", type, null);
            ILGenerator ilGen = dynMethod.GetILGenerator();

            ilGen.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            ilGen.Emit(OpCodes.Ret);
            c = (CtorDelegate)dynMethod.CreateDelegate(typeof(CtorDelegate));
            _constructorCache.Add(type, c);
            return c();
#else
            return Activator.CreateInstance(type);
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

        internal static void PocoMapLoader(Type type, SafeDictionary<string, MemberMap> memberMaps)
        {
            foreach (PropertyInfo info in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                memberMaps.Add(info.Name, new MemberMap(info));
            foreach (FieldInfo info in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                memberMaps.Add(info.Name, new MemberMap(info));
        }

        static GetHandler CreateGetHandler(FieldInfo fieldInfo)
        {
#if REFLECTION_EMIT
            Type type = fieldInfo.FieldType;
            DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", typeof(object), new[] { typeof(object) }, type, true);
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
#if REFLECTION_EMIT
            Type type = fieldInfo.FieldType;
            DynamicMethod dynamicSet = new DynamicMethod("DynamicSet", typeof(void), new[] { typeof(object), typeof(object) }, type, true);
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
#if REFLECTION_EMIT
            Type type = propertyInfo.PropertyType;
            DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", typeof(object), new[] { typeof(object) }, type, true);
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
#if REFLECTION_EMIT
            Type type = propertyInfo.PropertyType;
            DynamicMethod dynamicSet = new DynamicMethod("DynamicSet", typeof(void), new[] { typeof(object), typeof(object) }, type, true);
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

        internal sealed class MemberMap
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

    internal class SafeDictionary<TKey, TValue>
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