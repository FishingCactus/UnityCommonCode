using UnityEngine;

namespace FishingCactus
{
    public class ExposedScriptableObjectAttribute : PropertyAttribute
    {
        // -- FIELDS

        public bool ItMustDisplayObjectSelector = true;

        // -- METHODS

        public ExposedScriptableObjectAttribute(
            bool it_must_display_object_selector = true
            )
        {
            ItMustDisplayObjectSelector = it_must_display_object_selector;
        }
    }
}
