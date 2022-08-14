using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Implements a temporary highlight effect on hit.
    /// </summary>
    public class Blinked : MonoBehaviour
    {
        #region Сonstants, variables & properties
        private readonly List<Material> materials = new ();
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        private bool isLoopEffect;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            var renderers = gameObject.transform.GetComponentsInChildren<Renderer>();
            foreach (var ren in renderers)
            {
                if (ren.materials.Length == 0) continue;

                foreach (var mat in ren.materials)
                {
                    if (!materials.Contains(mat))
                    {
                        materials.Add(mat);
                    }
                }
            }
        }

        #endregion
        
        #region Functionality

        public void Init(bool loopEffect)
        {
            isLoopEffect = loopEffect;
            if (isLoopEffect) StartBlink();
        }
        
        public void StartBlink()
        {
            StartCoroutine(ShowBlinkEffectCoroutine());
        }
        
        private IEnumerator ShowBlinkEffectCoroutine()
        {
            if(materials.Count == 0) yield break;
            
            var t = 0f;
            while (isLoopEffect || t >= 0.98f)
            {
                for (; t < 1; t += Time.deltaTime)
                {
                    foreach (var mat in materials)
                    {
                        mat.SetColor(EmissionColor, new Color(Mathf.Sin(t * 30) * 0.5f + 0.5f, 0, 0, 1));
                        Debug.Log("Blink!");
                    }
                }
                yield return null;
            }
            
            foreach (var mat in materials)
            {
                mat.SetColor(EmissionColor, new Color(0, 0, 0, 1));
            }
        }
        #endregion
    }
}