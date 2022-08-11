using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.Events;
using DmitryAdventure.Characters;
using DmitryAdventure.Args;
using DmitryAdventure.Props;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Organizes game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables & constants

        [Header("Game")] 
        [SerializeField, Range(1,20)] private int goalToKillEnemiesCount = 1;
        private int _currentKillEnemiesCount;
        
        [Header("Links")] 
        [SerializeField] private Player player;

        public UnityEvent<UIManagerArgs> uiUpdateEventNotify;
        
        #endregion

        #region Monobehavior methods
        private void Start()
        {
            _currentKillEnemiesCount = 0;
            player.CharacterNotify += OnCharacterHandler;
            GameValuesUpdateEvent();
        }

        private void OnDestroy()
        {
            player.CharacterNotify -= OnCharacterHandler;
        }

        #endregion
        
        #region Event handlers
        /// <summary>
        /// Receives an event from Player.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        private void OnCharacterHandler(CharacterEventArgs e)
        {
            GameValuesUpdateEvent();

            if (!e.Die) return;
            
            Debug.Log(GameData.LoseMessage);
            UnityEditor.EditorApplication.isPaused = true;
        }

        /// <summary>
        /// Handles an event about killed enemies.
        /// </summary>
        /// <param name="killed">Dictionary of killed enemies (key - route number, value - number of killed).</param>
        public void OnKilledEnemiesHandler(Dictionary<int, int> killed)
        {
            var numberKilled = killed.Select(el => el.Value).Sum();

            _currentKillEnemiesCount += numberKilled;
            
            GameValuesUpdateEvent();
            
            if (_currentKillEnemiesCount < goalToKillEnemiesCount) return;
            
            Debug.Log(GameData.WinMessage);
            UnityEditor.EditorApplication.isPaused = true;
        }

        /// <summary>
        /// Handles user inventory update event.
        /// </summary>
        /// <param name="items"></param>
        public void OnInventoryUpdateHandler(IEnumerable<(string,int)> items)
        {
            GameValuesUpdateEvent(items);
        }

        /// <summary>
        /// Forms UI update arguments and dispatches an event.
        /// </summary>
        /// <param name="items">Collection of game values.</param>
        private void GameValuesUpdateEvent([CanBeNull] IEnumerable<(string,int)> items = null)
        {
            var args = new UIManagerArgs(
                player.playerStats.MaxHp, 
                player.CurrentHp, 
                goalToKillEnemiesCount,
                _currentKillEnemiesCount, 
                items);
            uiUpdateEventNotify.Invoke(args);
        }
        #endregion
    }
}


