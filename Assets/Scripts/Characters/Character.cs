using UnityEngine;
using DmitryAdventure.Args;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties

        protected internal CharacterType CharacterType { get; set; }

        [field: SerializeField, ReadonlyField, Tooltip("Current hit points.")]
        public float CurrentHp { get; protected set; }
        
        [field: SerializeField, ReadonlyField, Tooltip("Character's current movement speed")]
        protected float CurrentSpeed { get;  set; }

        protected Blinked BlinkEffect;
        private CharacterInventory _characterInventory;

        protected Animator CharacterAnimator;
        protected static readonly int AnimatorSpeed = Animator.StringToHash("Speed");
        protected static readonly int RotationAngle = Animator.StringToHash("Rotation");
        
        // Events 
        public delegate void CharacterHandler(CharacterEventArgs e);
        public event CharacterHandler CharacterNotify;

        #endregion

        #region Monobehavior methods

        protected virtual void Awake()
        {
            CharacterAnimator = gameObject.GetComponentInChildren<Animator>();
            BlinkEffect = GetComponent<Blinked>();
            _characterInventory = GetComponent<CharacterInventory>();
        }

        protected virtual void Update()
        {
            OnMovement();
            
            if (CurrentHp <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        #endregion

        #region Functionality

        #region Event handlers
        /// <summary>
        /// Character event for interested subscribers.
        /// </summary>
        /// <param name="e">Notification arguments.</param>
        protected void OnCharacterNotify(CharacterEventArgs e)
        {
            CharacterNotify?.Invoke(e);
        }
        #endregion

        #region Other methods

        /// <summary>
        /// Character movement method.
        /// </summary>
        protected abstract void OnMovement();

        /// <summary>
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public abstract void OnHit(float damage);
        
        /// <summary>
        /// Finds an in-game value in character's inventory.
        /// </summary>
        /// <param name="nameKey">Game value name as a search key.</param>
        /// <returns>Found game value or null.</returns>
        public GameValue FindItemInInventory(string nameKey)
        {
            return _characterInventory == null ? null : _characterInventory.PopFromInventory(nameKey);
        }
        
        #endregion
        #endregion
    }
}