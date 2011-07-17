﻿using System;
using BTDB.IL;
using BTDB.KVDBLayer.ReaderWriters;

namespace BTDB.ODBLayer.FieldHandlerImpl
{
    public class DateTimeFieldHandler : SimpleFieldHandlerBase
    {
        public DateTimeFieldHandler(): base(
            EmitHelpers.GetMethodInfo(() => ((AbstractBufferedReader)null).ReadDateTime()),
            EmitHelpers.GetMethodInfo(() => ((AbstractBufferedReader)null).SkipDateTime()),
            EmitHelpers.GetMethodInfo(() => ((AbstractBufferedWriter)null).WriteDateTime(new DateTime())))
        {
        }

        public override string Name
        {
            get { return "DateTime"; }
        }
    }
}