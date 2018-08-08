using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public class TagLink
    {
        // -- PUBLIC

        public string TagName { get { return _TagName; } }

        // -- PRIVATE

        [SerializeField]
        private string _TagName;
    }
}
