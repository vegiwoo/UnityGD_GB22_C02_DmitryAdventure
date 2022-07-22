using UnityEngine;
using DmitryAdventure.Characters;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Armament
{
    /// <summary>
    /// Represents ammunition
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Ammunition : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        protected GameObject effectPrefab;
        protected Rigidbody AmmunitionRigidbody;
        
        [SerializeField] 
        private AudioClip effectSoundClip;

        [field: SerializeField, Tooltip("Damage dealt by ammunition"), Range(10f,100f)] 
        protected int Damage { get; set; }
        
        #endregion

        #region Monobehavior methods
        
        protected virtual void Start()
        {
            AmmunitionRigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.OnHit(Damage);
            }
  
            AudioSource.PlayClipAtPoint(effectSoundClip, transform.position);

            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }
            
            Destroy(gameObject);
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