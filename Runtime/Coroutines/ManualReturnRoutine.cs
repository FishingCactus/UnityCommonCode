namespace FishingCactus
{
    public class ManualReturnRoutine : Routine
    {
        public void Return()
        {
            CallOnCompleted( true );
        }
    }
}