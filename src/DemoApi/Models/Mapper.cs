using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DemoApi.Models
{
    public static class Mapper
    {
        private static readonly Dictionary<(Type, Type), MethodInfo> Cache = new();
        public static T Map<T>(object v)
        {
            var fromType = v.GetType();

            var toType = typeof(T);
            var key = (fromType, toType);

            if (!Cache.ContainsKey(key))
            {
                Cache[key] = CreateMapMethod(fromType, toType);
            }

            return (T)Cache[key].Invoke(null, new[] { v });
        }
        static MethodInfo CreateMapMethod(Type fromType, Type toType)
        {

            var assemblyName = new AssemblyName("InternalMapperAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

            var typeBuilder = moduleBuilder.DefineType("Mapper", TypeAttributes.NotPublic);
            var methodBuilder = typeBuilder.DefineMethod(
                "Map",
                MethodAttributes.Public | MethodAttributes.Static,
                toType,
                new[] { fromType });

            var gen = methodBuilder.GetILGenerator();
            gen.Emit(OpCodes.Newobj, toType.GetConstructor(Type.EmptyTypes));

            var properties = fromType.GetProperties();

            foreach (var property in properties)
            {
                var toProp = toType.GetProperty(property.Name);

                //Skip if MappingTo property does not have 
                if (toProp == null) continue;

                gen.Emit(OpCodes.Dup);
                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Callvirt, property.GetMethod);
                gen.Emit(OpCodes.Callvirt, toProp.SetMethod);
            }

            gen.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();
            return type.GetMethod("Map", BindingFlags.Public | BindingFlags.Static, new[] { fromType });
        }
    }
}