using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DmitryAdventure.Characters;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
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
        
        [Header("UI")] 
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text enemiesLabel;
        [SerializeField] private Text keyLabel;
        [SerializeField] private Text mineLabel;
        [SerializeField] private Text medicineLabel;
        
        [Header("Links")] 
        [SerializeField] private Player player;
        
        #endregion

        #region Monobehavior methods
        private void Start()
        {
            hpBar.minValue = 0;
            hpBar.maxValue = player.playerStats.MaxHp;
            hpBar.value = player.CurrentHp;

            enemiesLabel.text = $"{GameData.EnemiesLabelText}: {_currentKillEnemiesCount:00} / {goalToKillEnemiesCount: 00}";

            player.CharacterNotify += PlayerOnCharacterHandler;
        }

        private void OnDestroy()
        {
            player.CharacterNotify -= PlayerOnCharacterHandler;
        }

        #endregion

        #region Functionality
        #region Coroutines
        // ...
        #endregion

        #region Event handlers
        /// <summary>
        /// Receives an event from Player.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        private void PlayerOnCharacterHandler(CharacterEventArgs e)
        {
            if (e.CurrentHp > 0)
            {
                hpBar.value = e.CurrentHp;
            }
            else
            {
                Debug.Log(GameData.LoseMessage);
                UnityEditor.EditorApplication.isPaused = true;
            }
        }

        /// <summary>
        /// Handles an event about killed enemies.
        /// </summary>
        /// <param name="numberKilled">Number of enemies killed.</param>
        public void OnKilledEnemies(int numberKilled)
        {
            _currentKillEnemiesCount += numberKilled;
            enemiesLabel.text = $"{GameData.EnemiesLabelText}: {_currentKillEnemiesCount:00} / {goalToKillEnemiesCount: 00}";

            if (_currentKillEnemiesCount < goalToKillEnemiesCount) return;
            
            Debug.Log(GameData.WinMessage);
            UnityEditor.EditorApplication.isPaused = true;
        }

        public void OnInventoryContent(List<(GameValue value, int count)> items)
        {
            foreach (var item in items)
            {
                switch (item.value.Name)
                {
                    case GameData.KeyLabelText:
                        keyLabel.text = $"KEYS: {item.count:00}";
                        break;
                    case GameData.MineLabelText:
                        mineLabel.text = $"MINES: {item.count:00}";
                        break;
                    case GameData.MedicineLabelText:
                        medicineLabel.text = $"MEDICINE: {item.count:00}";
                        break;
                }
            }
        }
        
        #endregion

        #region Other methods
        #endregion
        // ...
        #endregion
    }
}
