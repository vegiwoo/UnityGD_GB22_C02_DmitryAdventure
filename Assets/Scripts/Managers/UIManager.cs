using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DmitryAdventure.Args;
using UnityEngine;
using UnityEngine.UI;
using Events;
using GameDevLib.Args;
using GameDevLib.Interfaces;
using GameDevLib.Managers;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents manager to work with UI.
    /// </summary>
    public class UIManager : GameManager, GameDevLib.Interfaces.IObserver<InventoryArgs>, GameDevLib.Interfaces.IObserver<EnemyArgs>
    {
        #region Links
        [field: SerializeField] private InventoryEvent InventoryEvent { get; set; }
        [field: SerializeField] private EnemyEvent EnemyEvent { get; set; }
        
        [Header("UI Elements")]
        [SerializeField] private Slider hpBar;
        [SerializeField] private Text enemiesLabel;
        [SerializeField] private Text keyLabel;
        [SerializeField] private Text mineLabel;
        [SerializeField] private Text medicineLabel;
        
        #endregion
        
        #region Fileds
        
        private readonly Dictionary<string, Text> _gameValueMarkers = new();

        private bool _isMarkersAreAvailableForUpdating;
        
        #endregion
        
        #region Monobehavior methods
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            InitialUI();
            InventoryEvent.Attach(this);
            EnemyEvent.Attach(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InventoryEvent.Detach(this);
        }

        #endregion
        
        #region Functionality
        
        /// <summary>
        /// Fills UI with initial data.
        /// </summary>
        private void InitialUI()
        {
            enemiesLabel.text =  $"{GameData.EnemiesKey}: {0: 00} / {GameStats.GameHighScore: 00}".ToUpper();

            _gameValueMarkers[GameData.KeysKey] = keyLabel;
            _gameValueMarkers[GameData.MineKey] = mineLabel;
            _gameValueMarkers[GameData.MedicineKey] = medicineLabel;

            foreach (var (key, value) in _gameValueMarkers)
            {
                value.text = $"{key}: {0: 00}".ToUpper();
            }
        }
        
        #endregion

        public override void OnEventRaised(ISubject<UnitArgs> subject, UnitArgs args)
        {
            // Hp bar update
            hpBar.minValue = 0;
            hpBar.maxValue = args.Hp.max;
            hpBar.value = args.Hp.current <= 0 ? 0 : args.Hp.current ;
        }

        private IEnumerator UpdateGameValueMarkers(IReadOnlyCollection<GameValueItem> items)
        {
            foreach (var (key, value) in _gameValueMarkers)
            {
                var item = items
                    .FirstOrDefault(el => string.Equals(el.Key, key, StringComparison.CurrentCultureIgnoreCase));

                if (item != null)
                {
                    value.text = $"{key}: {item.Count: 00}".ToUpper();
                }
            }

            yield return null;
        }
        
                
        // Event handler for InventoryEvent
        public void OnEventRaised(ISubject<InventoryArgs> subject, InventoryArgs args)
        {
            // Game item labels update 
            StartCoroutine(UpdateGameValueMarkers(args.GameValues));
        }


        // Event handler for EnemyEvent
        public void OnEventRaised(ISubject<EnemyArgs> subject, EnemyArgs args)
        {
            enemiesLabel.text =  $"{GameData.EnemiesKey}: {args.UnitsRemovedSum: 00} / {GameStats.GameHighScore: 00}".ToUpper();
        }
    }
}