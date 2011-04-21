#define REFLECTION_EMIT

using System;
#if REFLECTION_EMIT
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace ReflectionUtils
{
#if REFLECTION_EMIT
    delegate object CtorDelegate();
#endif

    public delegate object GetHandler(object source);

    public delegate void SetHandler(object source, object value);

    public class ReflectionUtils
    {
        public readonly static ReflectionUtils Instance = new ReflectionUtils();

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

        internal static GetHandler CreateGetHandler(Type type, FieldInfo fieldInfo)
        {
            DynamicMethod dynamicGet = new DynamicMethod("DynamicGet", typeof(object), new [] { typeof(object) }, type, true);
            ILGenerator getGenerator = dynamicGet.GetILGenerator();

            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            if (type.IsValueType)
                getGenerator.Emit(OpCodes.Box, type);
            getGenerator.Emit(OpCodes.Ret);

            return (GetHandler)dynamicGet.CreateDelegate(typeof(GetHandler));
        }
    }
}