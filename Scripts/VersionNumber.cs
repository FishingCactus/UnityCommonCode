using System.Reflection;
using UnityEngine;

public class VersionNumber : MonoBehaviour
{
    public float ShowVersionForSeconds = -1;

    private string version;
    private Rect position = new Rect( 0, 0, 100, 20 );

    public string Version
    {
        get
        {
            if ( version == null )
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return version;
        }
    }

    void Start()
    {
        DontDestroyOnLoad( this );

        // Log current version in log file
        Debug.Log( string.Format( "Currently running version is {0}", Version ) );

        if ( ShowVersionForSeconds >= 0.0f )
        {
            Destroy( this, ShowVersionForSeconds );
        }

        position.x = 10f;
        position.y = Screen.height - position.height - 10f;

    }

    void OnGUI()
    {
        if ( ShowVersionForSeconds == 0.0f )
        {
            return;
        }

        GUI.contentColor = Color.gray;
        GUI.Label( position, string.Format( "v{0}", Version ) );
    }
}