using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FishingCactus
{
    public abstract class Wizard : MonoBehaviour
    {
        public int PanelIndex
        {
            get
            {
                return panelIndex;
            }
            private set
            {
                panelIndex = Mathf.Clamp( value, 0, panels.Count - 1 );
            }
        }

        private List<WizardPanel> panels;
        private Button previousButton;
        private Button nextButton;
        private Button finishButton;
        private int panelIndex;

        void Awake()
        {
            previousButton = transform.Find( "NavigationButtonPanel/PreviousButton" ).GetComponent<Button>();
            nextButton = transform.Find( "NavigationButtonPanel/NextButton" ).GetComponent<Button>();
            finishButton = transform.Find( "NavigationButtonPanel/FinishButton" ).GetComponent<Button>();

            panels = new List<WizardPanel>( GetComponentsInChildren<WizardPanel>() );

            foreach ( var panel in panels )
            {
                panel.OnPanelValueChanged += OnPanelValueChanged;
            }

            previousButton.onClick.AddListener( OnPreviousButtonClicked );
            nextButton.onClick.AddListener( OnNextButtonClicked );
            finishButton.onClick.AddListener( OnFinishButtonClicked );
        }

        void Start()
        {
            RefreshPanelVisibility();
        }

        private void OnPanelValueChanged( WizardPanel panel )
        {
            RefreshButtons();
        }

        private void OnPreviousButtonClicked()
        {
            PanelIndex--;

            RefreshPanelVisibility();
        }

        private void OnNextButtonClicked()
        {
            PanelIndex++;
            RefreshPanelVisibility();
        }

        protected abstract void OnFinishButtonClicked();

        private void RefreshPanelVisibility()
        {
            for ( int i = 0; i < panels.Count; i++ )
            {
                panels[ i ].gameObject.SetActive( i == PanelIndex );
            }

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            previousButton.gameObject.SetActive( PanelIndex > 0 );
            nextButton.gameObject.SetActive( PanelIndex < ( panels.Count - 1 ) && panels[ PanelIndex ].CanGoToNextPanel );
            finishButton.gameObject.SetActive( PanelIndex == panels.Count - 1 && panels[ PanelIndex ].CanGoToNextPanel );
        }
    }
}