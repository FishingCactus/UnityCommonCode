using UnityEngine;
using System;
using System.Collections.Generic;

public class CoroutineHelper : MonoBehaviour
{
    public static CoroutineHelper Get { get; private set; }

    private List<Routine> activeCoroutines = new List<Routine>();

    void Awake()
    {
        Get = this;
    }

    void Update()
    {
        var copy = new List<Routine>( activeCoroutines );

        foreach ( var coroutine in copy )
        {
            coroutine.Update();
        }
    }

    public void BeginExecute( IEnumerable<Routine> coroutine, EventHandler<ResultCompletionEventArgs> callback = null )
    {
        BeginExecute( coroutine.GetEnumerator(), callback );
    }

    public void BeginExecute( IEnumerator<Routine> coroutine, EventHandler<ResultCompletionEventArgs> callback = null )
    {
        var seq_result = new SequentialRoutine()
        {
            Enumerator = coroutine
        };

        activeCoroutines.Add( seq_result );

        if ( callback != null )
        {
            ExecuteOnCompleted( seq_result, callback );
        }

        ExecuteOnCompleted( seq_result, OnCompleted );
        seq_result.Execute();
    }

    public void Log( string message )
    {
        Debug.Log( message );
    }

    public void LogError( string message )
    {
        Debug.LogError( message );
    }

    private void OnCompleted( object sender, ResultCompletionEventArgs e )
    {
        Routine routine = sender as Routine;

        if ( routine.CanBeLogged )
        {
            Debug.Log( "Completed routine : " + routine.ToString() );
        }

        routine.Dispose();
        activeCoroutines.Remove( routine );
    }

    private void ExecuteOnCompleted( Routine result, EventHandler<ResultCompletionEventArgs> handler )
    {
        EventHandler<ResultCompletionEventArgs> onCompleted = null;
        onCompleted = ( s, e ) =>
        {
            result.Completed -= onCompleted;
            handler( s, e );
        };
        result.Completed += onCompleted;
    }
}