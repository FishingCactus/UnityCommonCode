using System.Collections.Generic;

[System.Serializable]
public class EnumDictionary<TEnum, TObject> where TEnum : struct,
    System.IConvertible
{
    // -- PUBLIC

    public TObject[] ValueTable;
    public List<string> EnumNameTable;

    public EnumDictionary()
    {
        ResetTables();
    }

    public TObject this[string element_name]
    {
        get { return this[EnumNameTable.IndexOf(element_name)]; }
        set { this[EnumNameTable.IndexOf(element_name)] = value; }
    }

    public TObject this[ int element_index ]
    {
        get { return ValueTable[ element_index ]; }
        set { ValueTable[element_index] = value; }
    }

#if UNITY_EDITOR
    public void CheckEnumSizeChange()
    {
        TObject[] backup_value_array = ValueTable;
        List<string> backup_enum_name_table = EnumNameTable;

        ResetTables();

        foreach( string enum_name in EnumNameTable )
        {
            if( backup_enum_name_table.Contains( enum_name ) )
            {
                ValueTable[EnumNameTable.IndexOf( enum_name )] = backup_value_array[backup_enum_name_table.IndexOf( enum_name )];
            }
        }
    }
#endif

    // -- PRIVATE

    private void ResetTables()
    {
        EnumNameTable = new List<string>();
        EnumNameTable.AddRange( System.Enum.GetNames( typeof( TEnum ) ) );

        EnumNameTable.Remove( "Count" );
        EnumNameTable.Remove( "None" );

        ValueTable = new TObject[EnumNameTable.Count];
    }
}

