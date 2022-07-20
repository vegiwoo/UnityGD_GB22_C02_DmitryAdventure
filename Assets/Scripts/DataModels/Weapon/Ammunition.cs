using System;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents ammunition
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Ammunition : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] protected GameObject effectPrefab;
        [SerializeField] protected AudioClip effectClip;
        protected Rigidbody AmmunitionRigidbody;
        private AudioSource _effectSound;
        
        /// <summary>
        /// Damage dealt by ammunition.
        /// </summary>
        [field: SerializeField, Tooltip("Ammo damage"), Range(10f,100f)] 
        protected int Damage { get; set; }
        
        #endregion

        #region Monobehavior methods

        protected virtual void Start()
        {
            AmmunitionRigidbody = GetComponent<Rigidbody>();
            _effectSound = new AudioSource { clip = effectClip, volume = 0.8f, playOnAwake = false};
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (character == null) return;
            
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }
            
            if (_effectSound != null)
            {
                _effectSound.Play();
            }
            
            character.OnHit(Damage);
            Destroy(gameObject, 0.1f);
        }

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