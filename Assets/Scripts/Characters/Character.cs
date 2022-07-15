using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField, Tooltip("Name on character")]
        protected string characterName;

        [SerializeField, Tooltip("Hit points")]
        public float hp;

        [SerializeField, Tooltip("Character movement speed"), Range(1f,5f)]
        protected float movementSpeed;
        
        #endregion

        #region Monobehavior methods

        protected virtual void Start()
        {
            hp = 100f;
            movementSpeed = 1f;
        }

        protected virtual void Update()
        {
            if (hp <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"Character {characterName} is killed!");
            }
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


        /// <summary>
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public bool OnHit(float damage)
        {
            hp -= damage;
            Debug.Log($"Character {characterName} wounded {damage}, hp - {hp}");
            return hp <= 0;
        }
        
        #endregion

        #endregion
    }
}