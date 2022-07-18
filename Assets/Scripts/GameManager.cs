using System;
using UnityEngine;
using UnityEngine.UI;

namespace DmitryAdventure
{
    /// <summary>
    /// Organizes game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables & constants
        [Header("Game")] 
        [SerializeField, Range(5,20)] private int goalToKillEnemies = 0;
        
        [Header("UI")] [SerializeField] private Slider hpBar;
        
        [Header("Links")] 
        [SerializeField] private Player player;
        [SerializeField] private AiminngColorize[] aimingColorizes;
        #endregion

        #region Monobehavior methods
        private void Start()
        {
            hpBar.minValue = 0;
            hpBar.maxValue = player.playerStats.MaxHp;
            hpBar.value = player.CurrentHp;
            
           // hero.HeroAimingNotify += PlayerIsAiming;
            player.CharacterNotify += PlayerOnCharacterNotify;
        }
        
        private void OnDestroy()
        {
            //hero.HeroAimingNotify -= PlayerIsAiming;
            player.CharacterNotify -= PlayerOnCharacterNotify;
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
        
        /// <summary>
        /// Receives an event from Player.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        private void PlayerOnCharacterNotify(CharacterEventArgs e)
        {
            if (e.CurrentHp > 0)
            {
                hpBar.value = e.CurrentHp;
            }
            else
            {
                Debug.Log("You lost");
                UnityEditor.EditorApplication.isPaused = true;
            }
        }

        #endregion

        #region Other methods
        #endregion
        // ...
        #endregion
    }
}
