using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents a blocked object.
    /// </summary>
    public class Blocked : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private Renderer[] renderers;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        // ...

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}