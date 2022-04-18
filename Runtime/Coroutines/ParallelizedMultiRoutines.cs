using System.Collections.Generic;

namespace FishingCactus
{
    public class ParallelizedMultiRoutines : Routine
    {
        private readonly List<IEnumerable<Routine>> routineList;
        private int executionCounter;

        public override bool CanBeLogged
        {
            get { return false; }
        }

        public ParallelizedMultiRoutines( List<IEnumerable<Routine>> routine_list )
        {
            routineList = routine_list;
            executionCounter = routineList.Count;
        }

        public override void Execute()
        {
            base.Execute();

            if ( routineList.Count == 0 )
            {
                CallOnCompleted( true );
                return;
            }

            foreach ( var routine_enumerable in routineList )
            {
                CoroutineHelper.Get.BeginExecute( routine_enumerable, RoutineCompleted );
            }
        }

        private void RoutineCompleted( object sender, ResultCompletionEventArgs args )
        {
            executionCounter--;

            if ( executionCounter == 0 )
            {
                CallOnCompleted( true );
            }
        }
    }
}