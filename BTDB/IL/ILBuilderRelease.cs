using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace BTDB.IL
{
    public class ILBuilderRelease : IILBuilder
    {
        public IILDynamicMethod NewMethod(string name, Type @delegate)
        {
            if (!@delegate.IsDelegate()) throw new ArgumentException("Generic paramater T must be Delegate");
            return new ILDynamicMethodImpl(name, @delegate, null);
        }

        public IILDynamicMethod<TDelegate> NewMethod<TDelegate>(string name) where TDelegate : class
        {
            var t = typeof(TDelegate);
            if (!t.IsDelegate()) throw new ArgumentException("Generic paramater T must be Delegate");
            return new ILDynamicMethodImpl<TDelegate>(name);
        }

        public IILDynamicMethodWithThis NewMethod(string name, Type @delegate, Type thisType)
        {
            if (thisType == null) throw new ArgumentNullException(nameof(thisType));
            if (!@delegate.IsDelegate()) throw new ArgumentException("Generic paramater T must be Delegate");
            return new ILDynamicMethodImpl(name, @delegate, thisType);
        }

        public IILDynamicType NewType(string name, Type baseType, Type[] interfaces)
        {
            return new ILDynamicTypeImpl(name, baseType, interfaces);
        }

        public Type NewEnum(string name, Type baseType, IEnumerable<KeyValuePair<string, object>> literals)
        {
            name = ILDynamicTypeDebugImpl.UniqueName(name);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(name), AssemblyBuilderAccess.RunAndCollect);
            var mb = ab.DefineDynamicModule(name, true);
            var enumBuilder = mb.DefineEnum(name, TypeAttributes.Public, baseType);
            foreach (var pair in literals)
            {
                enumBuilder.DefineLiteral(pair.Key, pair.Value);
            }
            return enumBuilder.CreateType();
        }
    }
}