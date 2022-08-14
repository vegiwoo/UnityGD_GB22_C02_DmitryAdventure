using System;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents level manager.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private GameObject stopSignPrefab;
        [SerializeField] private GameObject targetSignPrefab;
        [SerializeField] private Transform[] stopSignPoints;
        [SerializeField] private Transform[] targetSignPoints;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            foreach (var point in stopSignPoints)
            {
                Instantiate(stopSignPrefab, point.position, point.rotation);
            }
            
            foreach (var point in targetSignPoints)
            {
                Instantiate(targetSignPrefab, point.position, point.rotation);
            }
        }

        #endregion

        #region Functionality
        // ... 
        #endregion
    }
}