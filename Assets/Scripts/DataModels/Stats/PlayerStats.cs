using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents characteristics of player.
    /// </summary>
    [CreateAssetMenu]
    public class PlayerStats : CharacterStats
    {
        #region Сonstants, variables & properties

        [field:SerializeField,Tooltip("Running acceleration factor"), Range(2,5)]
        public float AccelerationFactor { get; set; }

        [field:SerializeField, Range(0.1f,3.0f)]
        public float JumpHeight { get; set; }

        #endregion

        #region Monobehavior methods

        // ...

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