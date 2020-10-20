using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnumDictionary<TEnum, TObject> where TEnum : struct,
    System.IConvertible
{
    // -- FIELDS

    [SerializeField] private TObject[] ValueTable;
    [SerializeField] public List<string> EnumNameTable;

    // --PROPERTIES

    public int Count{ get{ return EnumNameTable.Count; } }
    public TObject[] Values{ get{ return ValueTable; } }

    // -- METHODS

    public EnumDictionary()
    {
        ResetTables();
    }

    public TObject this[ TEnum enum_value ]
    {
        get
        {
            return ValueTable[ EnumNameTable.IndexOf( enum_value.ToString() ) ];
        }
    }

#if UNITY_EDITOR
    public void CheckEnumSizeChange()
    {
        TObject[] backup_value_array = ValueTable;
        List<string> backup_enum_name_table = EnumNameTable;

        ResetTables();

        foreach( string enum_name in EnumNameTable )
        {
            int
                backup_element_index = backup_enum_name_table.IndexOf( enum_name );

            if( backup_element_index != -1 )
            {
                ValueTable[ EnumNameTable.IndexOf( enum_name ) ] = backup_value_array[backup_element_index];
            }
        }
    }
#endif

    private void ResetTables()
    {
        EnumNameTable = new List<string>();
        EnumNameTable.AddRange( System.Enum.GetNames( typeof( TEnum ) ) );

        EnumNameTable.Remove( "Count" );
        EnumNameTable.Remove( "None" );

        ValueTable = new TObject[EnumNameTable.Count];
    }
}

