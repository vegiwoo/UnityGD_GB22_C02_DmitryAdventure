using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Organizes game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables & constants
        [SerializeField] private PlayerShooting hero;
        [SerializeField] private AiminngColorize[] aimingColorizes;
        #endregion

        #region Monobehavior methods
        private void Start()
        {
            hero.HeroAimingNotify += PlayerIsAiming;
        }

        private void OnDestroy()
        {
            hero.HeroAimingNotify -= PlayerIsAiming;
        }

        #endregion

        #region Functionality
        #region Coroutines
        // ...
        #endregion

        #region Event handlers
        /// <summary>
        /// Gets a notification if player is aiming.
        /// </summary>
        /// <param name="isAiming">Did player aim.</param>
        private void PlayerIsAiming(bool isAiming)
        {
            if (!isAiming)
                foreach (var aiming in aimingColorizes)
                    aiming.Set(new Color32(237, 229,45,255)); 
            else
                foreach (var aiming in aimingColorizes)
                    aiming.Set(new Color32(121, 237,45,255)); 
        }

        #endregion

        #region Other methods
        #endregion
        // ...
        #endregion
    }
}
