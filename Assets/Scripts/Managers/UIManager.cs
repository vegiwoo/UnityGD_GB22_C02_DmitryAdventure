using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using DmitryAdventure.Args;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents the manager to work with UI
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private Slider hpBar;
        [SerializeField] private Text enemiesLabel;
        [SerializeField] private Text keyLabel;
        [SerializeField] private Text mineLabel;
        [SerializeField] private Text medicineLabel;

        private Dictionary<string, Text> uiMarkers = new();

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            InitialUI();
        }

        #endregion
        
        #region Event handlers
        
        /// <summary>
        /// UI update event handler.
        /// </summary>
        /// <param name="e">Arguments for updating.</param>
        public void UIUpdateEventHandler(UIManagerArgs e)
        {
            hpBar.minValue = 0;
            hpBar.maxValue = e.MaxHp;
            hpBar.value = e.IsHeroDie ? 0 : e.CurrentHp ;

            UpdateEnemiesLabel(e.GoalToKillEnemies, e.CurrentKillEnemiesCount);
            UpdateValuesLabel(e.GameValues);
        }
        
        #endregion

        #region Other methods
        
        /// <summary>
        /// Fills UI with initial data.
        /// </summary>
        private void InitialUI()
        {
            uiMarkers[GameData.EnemiesKey] = enemiesLabel;
            uiMarkers[GameData.KeysKey] = keyLabel;
            uiMarkers[GameData.MineKey] = mineLabel;
            uiMarkers[GameData.MedicineKey] = medicineLabel;

            foreach (var marker in uiMarkers)
            {
                if (marker.Key == GameData.EnemiesKey)
                {
                    UpdateEnemiesLabel(0, 0);
                }
                else
                {
                    UpdateValuesLabel(marker.Key, 0);
                }
            }
        }
        
        private void UpdateEnemiesLabel(int goal, int current)
        {
            const string key = GameData.EnemiesKey;
            
            if (!uiMarkers.ContainsKey(key)) return; 
            uiMarkers[key].text = $"{key}: {current: 00} / {goal: 00}";
            
        }
        
        /// <summary>
        /// Updates game value labels from collection.
        /// </summary>
        /// <param name="values">Collection of game values&</param>
        private void UpdateValuesLabel([CanBeNull] List<(GameValue value, int count)> values)
        {
            if (values == null) return;
            
            foreach (var value in values)
            {
                var key = value.value.ValueKey;
                if (!uiMarkers.ContainsKey(key)) continue;
                
                uiMarkers[key].text =  $"{key}: {value.count: 00}";
            }
        }
        
        /// <summary>
        /// Updates game value label.
        /// </summary>
        /// <param name="key">Key for update.</param>
        /// <param name="value">Value for update.</param>
        private void UpdateValuesLabel(string key, int value)
        {
            if (!uiMarkers.ContainsKey(key)) return;
            uiMarkers[key].text =  $"{key}: {value: 00}";
        }

        #endregion
    }
}