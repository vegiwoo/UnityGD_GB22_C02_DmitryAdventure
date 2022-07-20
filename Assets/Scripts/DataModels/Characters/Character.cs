using UnityEngine;
using UnityEngine.InputSystem;
using DmitryAdventure.Armament;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties
        [field:SerializeField, Tooltip("Type of personage")] protected CharacterType CharacterType { get; set; }
        [field:SerializeField, Tooltip("Character's chosen weapon")] protected Weapon CurrentWeapon { get; set; }

        [field:SerializeField, Tooltip("Color of crosshair beam to display in editor")]
        private Color AimingColor { get; set; }
        
        /// <summary>
        /// Aiming point for weapon.
        /// </summary>
        protected Vector3 AimingPoint;
        
        [field: SerializeField, ReadonlyField, Tooltip("Current hit points.")] public float CurrentHp { get; protected set; }
        
        [field: SerializeField, ReadonlyField, Tooltip("Character's current movement speed")]protected float CurrentSpeed { get;  set; }

        protected float MovementSpeedDelta => CurrentSpeed / 3;

        // Events 
        public delegate void CharacterHandler(CharacterEventArgs e);
        public event CharacterHandler CharacterNotify;

        #endregion

        #region Monobehavior methods

        protected virtual void Awake()
        {
            AimingPoint = Vector3.zero;
        }

        protected virtual void Update()
        {
            OnMovement();
            OnTakeAim();

            if (CurrentHp <= 0)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnDrawGizmos()
        {
            if (AimingPoint == Vector3.zero) return;
            
            Gizmos.color = AimingColor;
            Gizmos.DrawLine(CurrentWeapon.ShotPoint.position, AimingPoint!);
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
        /// Character movement method.
        /// </summary>
        protected abstract void OnMovement();

        /// <summary>
        /// Aiming character.
        /// </summary>
        protected abstract void OnTakeAim();
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <param name="_">CallbackContext for more information.</param>
        /// <remarks>For player.</remarks>>
        protected void ShootWeapon(InputAction.CallbackContext _)
        {
            CurrentWeapon.Fire(AimingPoint);
        }
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <remarks>For NPC.</remarks>>
        protected void ShootWeapon()
        {
            CurrentWeapon.Fire(AimingPoint);
        }
        
        /// <summary>
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public abstract void OnHit(float damage);
        
        #endregion
        #endregion

        /// <summary>
        /// Character event for interested subscribers.
        /// </summary>
        /// <param name="e">Notification arguments.</param>
        protected void OnCharacterNotify(CharacterEventArgs e)
        {
            CharacterNotify?.Invoke(e);
        }
    }
}