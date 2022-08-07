using System.Collections;
using System.Linq;
using DmitryAdventure.Characters;
using UnityEngine;
using UnityEngine.AI;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.WeaponsAndAmmunition
{
    /// <summary>
    /// Represents a mine.
    /// </summary>
    [RequireComponent(typeof(AudioIsPlaying))]
    public class Mine : Ammunition, IDiscovering
    {
        #region Сonstants, variables & properties

        [field: SerializeField, Tooltip("Collection of types tracked by DiscoveryTrigger")]
        public DiscoveryType[] DiscoveryTypes { get; set; }

        [field:SerializeField, Tooltip("Tracking/discovery trigger")]
        public DiscoveryTrigger DiscoveryTrigger { get; set; }
        private GameObject _discoveryTriggerGo;
        
        [SerializeField] 
        private GameObject[] models;

        [SerializeField, Range(1,10)] private int numberTargetsToHit;
        
        [SerializeField, Range(5,10)] 
        private float explosionRadius;
        
        [SerializeField, Range(1,10)] 
        private float explosionPower;
        
        [SerializeField, Tooltip("Layer mask for no hitting objects")] 
        private LayerMask layerForNoHitting;
        
        private AudioIsPlaying _audioIsPlaying;

        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _discoveryTriggerGo = DiscoveryTrigger.gameObject;
            _audioIsPlaying = GetComponent<AudioIsPlaying>();
        }

        private void OnEnable()
        {
            numberTargetsToHit = 10;
            explosionRadius = 7;
            explosionPower = 2;
            DiscoveryTrigger.DiscoveryTriggerNotify += OnDiscoveryTriggerHandler;
            _audioIsPlaying.AudioTriggerNotify += AudioTriggerHandler;
        }

        private void OnDisable()
        {
            _audioIsPlaying.AudioTriggerNotify -= AudioTriggerHandler;
            DiscoveryTrigger.DiscoveryTriggerNotify -= OnDiscoveryTriggerHandler;
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

        public void OnDiscoveryTriggerHandler(DiscoveryType discoveryType, Transform discoveryTransform, bool isObjectEnters)
        {
            if (DiscoveryTypes.Length == 0 || !DiscoveryTypes.Contains(discoveryType)) return;
            
            // Play particle.
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            // Tossing objects
            HitObjectsAroundExplosion(_discoveryTriggerGo.transform.position, explosionRadius);

            // Destroy mine objects.
            foreach (var model in models)
            {
                Destroy(model);
            }

            // Hitting enemy
            if (discoveryType == DiscoveryType.Enemy &&
                discoveryTransform.gameObject.TryGetComponent<Enemy>(out var enemy))
            {
                // TODO: Toss the enemy up and then deal damage
                //enemy.OnHit(Damage);
                StartCoroutine(DamageCoroutine(enemy));
            }

            // Play sound.
            _audioIsPlaying.PlaySound(SoundType.Positive);
        }

        /// <summary>
        /// It hits objects when it explodes in a certain radius.
        /// </summary>
        /// <param name="center">Explosion center</param>
        /// <param name="radius">Damage radius</param>
        private void HitObjectsAroundExplosion(Vector3 center, float radius)
        {
            var hitColliders = new Collider[numberTargetsToHit];
            var numColliders = Physics.OverlapSphereNonAlloc(center, radius, hitColliders, ~layerForNoHitting);
            for (var i = 0; i < numColliders; i++)
            {
                if (!hitColliders[i].TryGetComponent<Rigidbody>(out var rb)) continue;

                if (hitColliders[i].TryGetComponent<NavMeshAgent>(out var nma))
                {
                    nma.enabled = false;
                }
   
                rb.isKinematic = false;
                var t = hitColliders[i].gameObject.transform;
                rb.AddForce(t.up * explosionPower, ForceMode.VelocityChange);
                rb.AddRelativeTorque(t.right * explosionPower / 2, ForceMode.VelocityChange);
            }
        }

        private IEnumerator DamageCoroutine(Character character)
        {
            var damageDone = 0;
            var damageStep = Damage / 35;
            while (damageDone < Damage)
            {
                character.OnHit(damageStep);
                damageDone += damageStep;
                yield return null;
            }
        }
        
        #endregion
    }
}