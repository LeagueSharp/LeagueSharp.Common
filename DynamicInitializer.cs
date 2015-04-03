using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;

namespace LeagueSharp.Common
{
    public class DynamicActivator
    {
        public delegate object DynamicCreationDelegate(object[] arguments);

        private static DynamicMethod CreateDynamicMethod(Type returnType, Type[] types)
        {
            var constructor = returnType.GetConstructor(types);

            if (constructor == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Could not find constructor matching signature {0}({1})", returnType.FullName,
                    string.Join(",", from argument in types select argument.FullName)));


            var constructorParams = constructor.GetParameters();
            var method = new DynamicMethod(string.Empty, returnType, new[] { typeof(object[]) }, constructor.DeclaringType);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Nop);
            for (var i = 0; i < constructorParams.Length; i++)
            {
                var paramType = constructorParams[i].ParameterType;
                il.Emit(OpCodes.Ldarg_0);
                switch (i)
                {
                    case 0:
                        il.Emit(OpCodes.Ldc_I4_0);
                        break;
                    case 1:
                        il.Emit(OpCodes.Ldc_I4_1);
                        break;
                    case 2:
                        il.Emit(OpCodes.Ldc_I4_2);
                        break;
                    case 3:
                        il.Emit(OpCodes.Ldc_I4_3);
                        break;
                    case 4:
                        il.Emit(OpCodes.Ldc_I4_4);
                        break;
                    case 5:
                        il.Emit(OpCodes.Ldc_I4_5);
                        break;
                    case 6:
                        il.Emit(OpCodes.Ldc_I4_6);
                        break;
                    case 7:
                        il.Emit(OpCodes.Ldc_I4_7);
                        break;
                    case 8:
                        il.Emit(OpCodes.Ldc_I4_8);
                        break;
                    default:
                        il.Emit(OpCodes.Ldc_I4_S, i);
                        break;
                }
                il.Emit(OpCodes.Ldelem_Ref);
                il.Emit(paramType.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, paramType);
            }
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);
            return method;
        }

        public static object New(Type returnType, object[] args)
        {
            var types = args.Select(a => a.GetType()).ToArray();
            var creator = CreateDynamicMethod(returnType, types);
            return ((DynamicCreationDelegate)creator.CreateDelegate(typeof(DynamicCreationDelegate)))(args);
        }

        public static T New<T>(object[] args)
        {
            var returnType = typeof(T);
            var types = args.Select(a => a.GetType()).ToArray();
            var creator = CreateDynamicMethod(returnType, types);
            return (T)((DynamicCreationDelegate)creator.CreateDelegate(typeof(DynamicCreationDelegate)))(args);
        }
    }
}
