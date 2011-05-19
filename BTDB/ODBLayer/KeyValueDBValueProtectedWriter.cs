﻿using BTDB.KVDBLayer;
using BTDB.KVDBLayer.Interface;
using BTDB.KVDBLayer.ReaderWriters;

namespace BTDB.ODBLayer
{
    internal class KeyValueDBValueProtectedWriter : KeyValueDBValueWriter
    {
        readonly KeyValueDBTransactionProtector _protector;

        internal KeyValueDBValueProtectedWriter(IKeyValueDBTransaction transaction, KeyValueDBTransactionProtector protector)
            : base(transaction)
        {
            _protector = protector;
        }

        public override void Dispose()
        {
            base.Dispose();
            _protector.Stop();
        }
    }
}