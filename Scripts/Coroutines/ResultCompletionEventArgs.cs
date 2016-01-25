using UnityEngine;
using System;

namespace FishingCactus
{
    public class ResultCompletionEventArgs : EventArgs
    {
        public ResultCompletionEventArgs()
        {

        }

        public ResultCompletionEventArgs( bool is_successfull )
        {
            WasSuccessfull = is_successfull;
        }

        public bool WasSuccessfull = true;
    }
}