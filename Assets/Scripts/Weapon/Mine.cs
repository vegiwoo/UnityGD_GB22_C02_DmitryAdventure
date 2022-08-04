// ReSharper disable once CheckNamespace

using DmitryAdventure.Characters;
using UnityEngine;

namespace DmitryAdventure.Armament
{
    /// <summary>
    /// Represents a mine.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class Mine : Ammunition
    {
        #region Сonstants, variables & properties

        [field: SerializeField] private GameObject[] Models { get; set; }
        
        
        private AudioIsPlaying _audioIsPlaying;
        private DiscoveryTrigger _discoveryTrigger;

        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
        }

        protected override void Start()
        {
            base.Start();
            _audioIsPlaying.AudioTriggerNotify += AudioTriggerHandler;
            _discoveryTrigger.DiscoveryTriggerNotify += DiscoveryTriggerHandler;
        }

      
        // 

        private void OnDestroy()
        {
            _audioIsPlaying.AudioTriggerNotify -= AudioTriggerHandler;
            _discoveryTrigger.DiscoveryTriggerNotify -= DiscoveryTriggerHandler;
        }
        #endregion

        #region Functionality

        private void AudioTriggerHandler(bool isAudioPlayed)
        {
            if (isAudioPlayed)
            {
                Destroy(gameObject);
            }
        }

        private void DiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool entry)
        {
            var enemy = discoveryTransform.gameObject.GetComponent<Enemy>();
            if (discoveryType != DiscoveryType.Enemy || enemy == null) return;
            
            enemy.OnHit(Damage);
            
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            foreach (var model in Models)
            {
                Destroy(model);
            }

            _audioIsPlaying.PlaySound(SoundType.Positive);
        }
        
        #endregion
    }
}