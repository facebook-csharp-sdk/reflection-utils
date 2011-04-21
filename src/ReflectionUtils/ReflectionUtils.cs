#define REFLECTION_EMIT

using System;
#if REFLECTION_EMIT
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace ReflectionUtils
{
    internal delegate object GetHandler(object source);

    internal delegate void SetHandler(object source, object value);

    internal class ReflectionUtils
    {
        public static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", typeof(object), new[] { typeof(object) }, type, true);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            if (type.IsValueType)
                getGenerator.Emit(OpCodes.Box, type);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        public static SetHandler CreateSetHandler(Type type, FieldInfo fieldInfo)
        {
            if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
                return null;

            DynamicMethod dynamicSet = new DynamicMethod("DynamicSet", typeof(void), new[] { typeof(object), typeof(object) }, type, true);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            if (type.IsValueType)
                setGenerator.Emit(OpCodes.Unbox_Any, type);
            setGenerator.Emit(OpCodes.Stfld, fieldInfo);
            setGenerator.Emit(OpCodes.Ret);

            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }

        public static GetHandler CreateGetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod(true);
            if (getMethodInfo == null)
                return null;

            DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", typeof(object), new[] { typeof(object) }, type, true);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Call, getMethodInfo);
            if (type.IsValueType)
                getGenerator.Emit(OpCodes.Box, type);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }

        public static SetHandler CreateSetHandler(Type type, PropertyInfo propertyInfo)
        {
            MethodInfo setMethodInfo = propertyInfo.GetSetMethod(true);
            if (setMethodInfo == null)
                return null;

            DynamicMethod dynamicSet = new DynamicMethod("DynamicSet", typeof(void), new[] { typeof(object), typeof(object) }, type, true);
            ILGenerator setGenerator = dynamicSet.GetILGenerator();

            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            if (type.IsValueType)
                setGenerator.Emit(OpCodes.Unbox_Any, type);
            setGenerator.Emit(OpCodes.Call, setMethodInfo);
            setGenerator.Emit(OpCodes.Ret);
            return (SetHandler)dynamicSet.CreateDelegate(typeof(SetHandler));
        }
    }

    public class ResolverCache
    {
#if REFLECTION_EMIT
        delegate object CtorDelegate();
#endif

#if REFLECTION_EMIT
        readonly SafeDictionary<Type, CtorDelegate> _constructorCache = new SafeDictionary<Type, CtorDelegate>();
#endif

        public object GetNewInstance(Type type)
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
    }

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
            Getter = ReflectionUtils.CreateGetHandler(Type, propertyInfo);
            Setter = ReflectionUtils.CreateSetHandler(Type, propertyInfo);
        }

        public MemberMap(FieldInfo fieldInfo)
        {
            MemberInfo = fieldInfo;
            Type = fieldInfo.FieldType;
            Getter = ReflectionUtils.CreateGetHandler(Type, fieldInfo);
            Setter = ReflectionUtils.CreateSetHandler(Type, fieldInfo);
        }
    }
}