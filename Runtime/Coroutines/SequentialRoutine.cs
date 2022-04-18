using System.Collections.Generic;

namespace FishingCactus
{
    public class SequentialRoutine : Routine
    {
        public IEnumerator<Routine> Enumerator;

        public SequentialRoutine()
        {
        }

        public override bool CanBeLogged
        {
            get { return false; }
        }

        public override void Update()
        {
            Enumerator.Current.Update();
        }

        public override void Execute()
        {
            base.Execute();
            ChildCompleted( null, new ResultCompletionEventArgs() );
        }

        private void ChildCompleted( object sender, ResultCompletionEventArgs args )
        {
            var previous = ( Routine )sender;
            if ( previous != null )
            {
                if ( previous.CanBeLogged )
                {
                    CoroutineHelper.Get.Log( "Completed : " + previous.ToString() );
                }

                previous.Dispose();
                previous.Completed -= ChildCompleted;
            }

            if ( !args.WasSuccessfull )
            {
                OnComplete( false );
                return;
            }

            if ( Enumerator == null )
            {
                CoroutineHelper.Get.LogError( "SequentialRoutine has no Enumerator attached!!!" );
            }

            var moveNextSucceeded = Enumerator.MoveNext();

            if ( !moveNextSucceeded )
            {
                OnComplete( true );
            }
            else
            {
                var next = Enumerator.Current;
                next.Completed += ChildCompleted;
                next.Execute();
            }
        }

        private void OnComplete( bool it_is_successfull )
        {
            Enumerator.Dispose();
            CallOnCompleted( it_is_successfull );
        }
    }
}