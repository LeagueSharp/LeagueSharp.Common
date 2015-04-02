using System;
using System.Reflection.Emit;

namespace LeagueSharp.Common
{
    public class DynamicInitializer
    {
        public static TV NewInstance<TV>() where TV : class
        {
            return ObjectGenerator(typeof (TV)) as TV;
        }

        public static object NewInstance(Type type)
        {
            return ObjectGenerator(type);
        }

        private static object ObjectGenerator(Type type)
        {
            var target = type.GetConstructor(Type.EmptyTypes);
            var dynamic = new DynamicMethod(string.Empty, type, new Type[0], target.DeclaringType);
            var il = dynamic.GetILGenerator();
            il.DeclareLocal(target.DeclaringType);
            il.Emit(OpCodes.Newobj, target);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            var method = (Func<object>) dynamic.CreateDelegate(typeof (Func<object>));
            return method();
        }
    }
}