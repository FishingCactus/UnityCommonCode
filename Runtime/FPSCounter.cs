using System;
using System.Collections.Generic;
using UnityEngine;

namespace FishingCactus
{
    public class FPSCounter : MonoBehaviour
    {
        protected void Awake()
        {
            enabled = Debug.isDebugBuild;

            if ( !enabled )
            {
                return;
            }

#if UNITY_EDITOR ||  UNITY_STANDALONE
            targetFrameRate = 60.0f;
#elif UNITY_ANDROID || UNITY_IOS
            targetFrameRate = 30.0f;
#else
            targetFrameRate = 60.0f;
#endif

            fpsColors.Sort();

            for ( int i = 0; i < fpsColors.Count; i++ )
            {
                var fps_color = fpsColors[ i ];

                fps_color.FPS = targetFrameRate * fps_color.TargetFrameRatePercentage;
            }
        }

        protected void Update()
        {
            deltaTime += ( Time.deltaTime - deltaTime ) * 0.1f;
        }

        protected void OnGUI()
        {
            if ( !enabled )
            {
                return;
            }

            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect( 0, 0, w, h * 2 / 100 );
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;

            Color text_color = Color.white;

            for ( int i = fpsColors.Count - 1; i > 0; i-- )
            {
                var fps_color = fpsColors[ i ];

                if ( fps > fps_color.FPS )
                {
                    text_color = fps_color.TextColor;
                    break;
                }
            }

            style.normal.textColor = text_color;

            string text = string.Format( "{0:0.0} ms ({1:0.} fps)", msec, fps );
            GUI.Label( rect, text, style );
        }

        [Serializable]
        private struct FpsColor : IComparable<FpsColor>
        {
            #pragma warning disable 0649
            [Range( 0.0f, 1.0f )]
            public float TargetFrameRatePercentage;
            public Color TextColor;
            #pragma warning restore

            [HideInInspector, NonSerialized]
            public float FPS;

            public int CompareTo( FpsColor other )
            {
                return TargetFrameRatePercentage.CompareTo( other.TargetFrameRatePercentage );
            }
        }

        #pragma warning disable 0649
        [SerializeField]
        private List<FpsColor> fpsColors;
        #pragma warning restore
        private float deltaTime = 0.0f;
        private float targetFrameRate;
    }
}