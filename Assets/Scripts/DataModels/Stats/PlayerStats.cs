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

        [Tooltip("Running acceleration factor"), Range(2,5)]
        public float accelerationFactor;

        [SerializeField]
        public float jumpHeight;

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