﻿using System;
using System.Reflection.Emit;
using BTDB.IL;
using BTDB.ODBLayer;
using BTDB.StreamLayer;

namespace BTDB.FieldHandler
{
    public class DBObjectFieldHandler : IFieldHandler
    {
        readonly IObjectDB _objectDB;
        readonly byte[] _configuration;
        readonly string _typeName;
        readonly Type _type;

        public DBObjectFieldHandler(IObjectDB objectDB, Type type)
        {
            _objectDB = objectDB;
            _type = type;
            _typeName = _objectDB.RegisterType(type);
            var writer = new ByteArrayWriter();
            writer.WriteString(_typeName);
            _configuration = writer.Data;
        }

        public DBObjectFieldHandler(IObjectDB objectDB, byte[] configuration)
        {
            _objectDB = objectDB;
            _configuration = configuration;
            _typeName = new ByteArrayReader(configuration).ReadString();
            _type = _objectDB.TypeByName(_typeName);
        }

        public static string HandlerName
        {
            get { return "Object"; }
        }

        public string Name
        {
            get { return HandlerName; }
        }

        public byte[] Configuration
        {
            get { return _configuration; }
        }

        public static bool IsCompatibleWith(Type type)
        {
            return (!type.IsInterface && !type.IsValueType && !type.IsGenericType);
        }

        bool IFieldHandler.IsCompatibleWith(Type type, FieldHandlerOptions options)
        {
            if (options.HasFlag(FieldHandlerOptions.Orderable)) return false;
            return IsCompatibleWith(type);
        }

        public Type HandledType()
        {
            return _type ?? typeof(object);
        }

        public bool NeedsCtx()
        {
            return true;
        }

        public void Load(ILGenerator ilGenerator, Action<ILGenerator> pushReaderOrCtx)
        {
            var localResultOfObject = ilGenerator.DeclareLocal(typeof(object));
            object fake;
            ilGenerator
                .Do(pushReaderOrCtx)
                .Ldloca(localResultOfObject)
                .Callvirt(() => ((IReaderCtx)null).ReadObject(out fake))
                .Pop();
            ilGenerator.Ldloc(localResultOfObject);
            var type = HandledType();
            ilGenerator.Do(_objectDB.TypeConvertorGenerator.GenerateConversion(typeof(object), type));
        }

        public void Skip(ILGenerator ilGenerator, Action<ILGenerator> pushReaderOrCtx)
        {
            ilGenerator
                .Do(pushReaderOrCtx)
                .Callvirt(() => ((IReaderCtx)null).SkipObject())
                .Pop();
        }

        public void Save(ILGenerator ilGenerator, Action<ILGenerator> pushWriterOrCtx, Action<ILGenerator> pushValue)
        {
            ilGenerator
                .Do(pushWriterOrCtx)
                .Do(pushValue)
                .Do(_objectDB.TypeConvertorGenerator.GenerateConversion(HandledType(), typeof(object)))
                .Callvirt(() => ((IWriterCtx)null).WriteObject(null))
                .Pop();
        }

        public IFieldHandler SpecializeLoadForType(Type type)
        {
            return this;
        }

        public IFieldHandler SpecializeSaveForType(Type type)
        {
            return this;
        }
    }
}