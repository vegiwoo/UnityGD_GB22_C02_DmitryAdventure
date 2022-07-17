using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Implements a temporary highlight effect on hit.
    /// </summary>
    public class Blinked : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField, Tooltip("List of objects with renders that need to be blinked")] 
        private List<Renderer> renderers;

        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        #endregion

        #region Monobehavior methods
        //...
        #endregion

        #region Functionality

        #region Coroutines
        private IEnumerator ShowBlinkEffectCoroutine()
        {
            for (float t = 0; t < 1; t += Time.deltaTime)
            {
                foreach (var mat in renderers.SelectMany(ren => ren.materials))
                    mat.SetColor(EmissionColor, new Color(Mathf.Sin(t * 30) * 0.5f + 0.5f, 0, 0, 1));

                if (t >= 0.98f)
                    yield break;
                
                yield return null;
            }
        }

        #endregion

        #region Event handlers
        // ...
        #endregion

        #region Other methods
        public void StartBlink()
        {
            StartCoroutine(ShowBlinkEffectCoroutine());
        }
        #endregion
        #endregion
    }
}