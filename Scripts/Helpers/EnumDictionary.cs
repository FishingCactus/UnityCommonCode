﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnumDictionary<TEnum, TObject> where TEnum : struct,
    System.IConvertible
{
    // -- PUBLIC

    public EnumDictionary()
    {
        ResetTables();
    }

    public TObject this[string element_name]
    {
        get { return this[_EnumNameTable.IndexOf(element_name)]; }
        set { this[_EnumNameTable.IndexOf(element_name)] = value; }
    }

    public TObject this[ int element_index ]
    {
        get { return _ValueTable[ element_index ]; }
        set { _ValueTable[element_index] = value; }
    }

#if UNITY_EDITOR
    public void CheckEnumSizeChange()
    {
        TObject[] backup_value_array = _ValueTable;
        List<string> backup_enum_name_table = _EnumNameTable;

        ResetTables();

        foreach( string enum_name in _EnumNameTable )
        {
            if( backup_enum_name_table.Contains( enum_name ) )
            {
                _ValueTable[_EnumNameTable.IndexOf( enum_name )] = backup_value_array[backup_enum_name_table.IndexOf( enum_name )];
            }
        }
    }
#endif

    // -- PRIVATE

    [SerializeField]
    private TObject[] _ValueTable;
    [SerializeField]
    public List<string> _EnumNameTable;

    private void ResetTables()
    {
        _EnumNameTable = new List<string>();
        _EnumNameTable.AddRange( System.Enum.GetNames( typeof( TEnum ) ) );

        _EnumNameTable.Remove( "Count" );
        _EnumNameTable.Remove( "None" );

        _ValueTable = new TObject[_EnumNameTable.Count];
    }
}

