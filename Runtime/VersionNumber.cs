using UnityEngine;

public class VersionNumber : MonoBehaviour
{
    // -- PUBLIC

    public string Version
    {
        get
        {
            if ( VersionInformation == null )
            {
                if( ItMustAddCompanyName )
                {
                    VersionInformation = $"{Application.companyName} - {Application.productName} - {Application.version}";
                }
                else
                {
                    VersionInformation = $"{Application.productName} - {Application.version}";
                }
            }
            return VersionInformation;
        }
    }

    // -- PRIVATE

    [SerializeField]
    private float ShowVersionForSeconds = -1;
    [SerializeField]
    private bool ItMustAddCompanyName = true;

    private string VersionInformation;
    private Rect Position = new Rect( 0, 0, 300, 20 );

    // -- UNITY

    void Start()
    {
        DontDestroyOnLoad( this );

        Debug.Log( string.Format( "Currently running version is {0}", Version ) );

        if ( ShowVersionForSeconds >= 0.0f )
        {
            Destroy( this, ShowVersionForSeconds );
        }

        Position.x = 10f;
        Position.y = Screen.height - Position.height - 10f;
    }

    void OnGUI()
    {
        if ( ShowVersionForSeconds == 0.0f )
        {
            return;
        }

        GUI.contentColor = Color.gray;
        GUI.Label( Position, Version );
    }
}
