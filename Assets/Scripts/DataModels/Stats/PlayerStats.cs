using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents characteristics of player.
    /// </summary>
    [CreateAssetMenu]
    public class PlayerStats : CharacterStats
    {
        #region Сonstants, variables & properties

        [SerializeField,Tooltip("Running acceleration factor"), Range(2,5)]
        private float accelerationFactor;
        public float AccelerationFactor => accelerationFactor;

        [SerializeField, Range(0.1f,3.0f)]
        private float jumpHeight;
        public float JumpHeight => jumpHeight;

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