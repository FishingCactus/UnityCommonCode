using System.Collections.Generic;
using UnityEngine;

namespace FishingCactus
{
    public class GUIConsole : MonoBehaviour
    {
        public static GUIConsole Get { get; private set; }

        void Awake()
        {
            Get = this;

            showAutomaticallyOnError &= Debug.isDebugBuild;
        }

        void OnEnable()
        {
            if ( !Debug.isDebugBuild )
            {
                return;
            }

            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            if ( !Debug.isDebugBuild )
            {
                return;
            }

            Application.logMessageReceived -= HandleLog;
        }

        void Update()
        {
            if ( !Debug.isDebugBuild )
            {
                return;
            }

            if ( UnityEngine.Input.GetKeyDown( toggleKey ) )
            {
                Toggle();
            }
        }

        void OnGUI()
        {
            if ( !show || !Debug.isDebugBuild )
            {
                return;
            }

            windowRect = GUILayout.Window( 123456, windowRect, ConsoleWindow, "Console" );
        }

        public void Toggle()
        {
            show = !show;
        }

        void ConsoleWindow( int windowID )
        {
            scrollPosition = GUILayout.BeginScrollView( scrollPosition );

            for ( int i = 0; i < logs.Count; i++ )
            {
                var log = logs[ i ];

                if ( collapse )
                {
                    var messageSameAsPrevious = i > 0 && log.message == logs[ i - 1 ].message;

                    if ( messageSameAsPrevious )
                    {
                        continue;
                    }
                }

                GUI.contentColor = logTypeColors[ log.type ];
                GUILayout.Label( log.message );
            }

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();

            if ( GUILayout.Button( "Hide" ) )
            {
                show = false;
            }

            if ( GUILayout.Button( clearLabel ) )
            {
                logs.Clear();
            }

            collapse = GUILayout.Toggle( collapse, collapseLabel, GUILayout.ExpandWidth( false ) );

            GUILayout.EndHorizontal();

            GUI.DragWindow( titleBarRect );
        }

        void HandleLog( string message, string stackTrace, LogType type )
        {
            if ( type <= logTypeFilter
                || type == LogType.Exception
                )
            {
                logs.Add( new Log()
                {
                    message = message,
                    stackTrace = stackTrace,
                    type = type,
                } );
            }

#if !UNITY_EDITOR
            if ( showAutomaticallyOnError
                && ( type < LogType.Warning || type == LogType.Exception )
                )
            {
                show = true;
            }
#endif
        }

        struct Log
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }

        [SerializeField]
        private KeyCode toggleKey = KeyCode.BackQuote;
        [SerializeField]
        private LogType logTypeFilter = LogType.Warning;
        [SerializeField]
        private bool showAutomaticallyOnError = false;

        private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        private const int margin = 20;

        private List<Log> logs = new List<Log>();
        private Vector2 scrollPosition;
        private bool show;
        private bool collapse;

        private Rect windowRect = new Rect( margin, margin, Screen.width - ( margin * 2 ), Screen.height - ( margin * 2 ) );
        private Rect titleBarRect = new Rect( 0, 0, 10000, 20 );
        private GUIContent clearLabel = new GUIContent( "Clear", "Clear the contents of the console." );
        private GUIContent collapseLabel = new GUIContent( "Collapse", "Hide repeated messages." );
    }
}