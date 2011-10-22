﻿namespace BTDB.KVDBLayer
{
    public enum KVReplayOperation
    {
        Open,
        StartTransaction,
        StartWritingTransaction,
        KeyValueDBDispose,
        TransactionDispose,
        SetKeyPrefix,
        InvalidateCurrentKey,
        FindFirstKey,
        FindLastKey,
        FindPreviousKey,
        FindNextKey,
        FindKey,
        CreateOrUpdateKeyValue,
        GetKeyValueCount,
        GetKeyIndex,
        GetKeySize,
        GetValueSize,
        PeekKey,
        ReadKey,
        PeekValue,
        ReadValue,
        WriteValue,
        SetValueSize,
        SetValue,
        EraseCurrent,
        EraseAll,
        EraseRange,
        Commit,
        CalculateStats,
        SetHumanReadableDescriptionInHeader,
        SetKeyIndex
    }
}