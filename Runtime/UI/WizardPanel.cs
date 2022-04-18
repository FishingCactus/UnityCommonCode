using UnityEngine;

namespace FishingCactus
{
    public class WizardPanel : MonoBehaviour
    {
        public delegate void OnPanelValueChangedDelegate( WizardPanel panel );
        public OnPanelValueChangedDelegate OnPanelValueChanged;

        public virtual bool CanGoToNextPanel { get { return true; } }

        protected void RefreshWizardButtons()
        {
            if ( OnPanelValueChanged != null )
            {
                OnPanelValueChanged( this );
            }
        }
    }

}