using System.Collections.Generic;
using DmitryAdventure.Args;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Props
{
    /// <summary>
    /// Represents a loot box.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class LootBox : MonoBehaviour, IDiscovering
    {
        #region Сonstants, variables & properties

        public DiscoveryType[] DiscoveryTypes { get; set; } = { DiscoveryType.Player };
        public DiscoveryTrigger DiscoveryTrigger { get; set; }
        
        private AudioIsPlaying _audioIsPlaying;
        
        /// <summary>
        /// Collection of game objects in a box.
        /// </summary>
        [field: SerializeField, ReadonlyField]
        private GameValue[] ObjectsInBox { get; set; }

        public delegate void HeroFindValuesHandler(IEnumerable<GameValue> values);
        public event HeroFindValuesHandler HeroFindValuesNotify;

        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            DiscoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
        }

        private void OnEnable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
        }

        private void OnDisable()
        {
            DiscoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
        }

        #endregion
        
        #region Functionality
        public void Init(LootBoxArgs args)
        {
            ObjectsInBox = args.GameValues;
        }
        
        public void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            if(ObjectsInBox == null || discoveryType != DiscoveryType.Player) return;
            
            _audioIsPlaying.PlaySound(SoundType.Positive);

            if (ObjectsInBox.Length > 0)
            {
                HeroFindValuesNotify?.Invoke(ObjectsInBox);
            }
            else
            {
                Debug.Log($"LootBox empty");
            }

            ObjectsInBox = null;
        }
        #endregion
    }
}
