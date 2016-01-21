using System;

public abstract class Routine : IDisposable
{
    public virtual bool CanBeLogged
    {
        get { return true; }
    }

    public virtual void Execute()
    {
        CoroutineHelper.Get.Log( "Execute routine : " + ToString() );
    }

    public virtual void Update()
    {

    }

    public virtual void Dispose()
    {

    }

    public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

    protected void CallOnCompleted( bool it_is_successfull )
    {
        if ( Completed != null )
        {
            Completed( this, new ResultCompletionEventArgs( it_is_successfull ) );
        }
    }
}