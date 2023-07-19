namespace FishingCactus
{
    public static class ULongExtensions
    {
        public static uint GetToggledFlagsCount(
            this ulong flags
            )
        {
            uint count = 0;

            while( flags != 0 )
            {
                flags &= flags - 1;

                ++count;
            }

            return count;
        }
    }
}
