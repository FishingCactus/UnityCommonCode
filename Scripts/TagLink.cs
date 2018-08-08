using UnityEngine;

namespace FishingCactus
{
    [System.Serializable]
    public class TagLink
    {
        // -- PUBLIC

        public string TagName { get { return _TagName; } }

        public TagLink(
            string tag_name
            )
        {
            _TagName = tag_name;
        }

        // -- PRIVATE

        [SerializeField]
        private string _TagName;
    }
}
