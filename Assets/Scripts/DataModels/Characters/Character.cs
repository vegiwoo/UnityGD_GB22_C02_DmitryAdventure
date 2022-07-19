using UnityEngine;
using UnityEngine.InputSystem;

namespace DmitryAdventure
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties

        /// <summary>
        /// Unique ID of character.
        /// </summary>
        public GUI CharacterID = new ();
        [SerializeField] public CharacterType characterType;
        [SerializeField, Tooltip("Character's chosen weapon")] protected Weapon currentWeapon;

        [SerializeField, Tooltip("Color of crosshair beam to display in editor")]
        private Color aimingColor;

        /// <summary>
        /// Aiming point for weapon.
        /// </summary>
        protected Vector3 AimingPoint;
        
        /// <summary>
        /// Current hit points.
        /// </summary>
        public float CurrentHp { get; protected set; }

        /// <summary>
        /// Character current movement speed.
        /// </summary>
        protected float CurrentSpeed { get;  set; }

        protected float MovementSpeedDelta => CurrentSpeed / 3;
        
        protected float RotationSpeedDelta => CurrentSpeed / 10;
        
        protected const float GravityValue = -9.81f;

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
            
            Gizmos.color = aimingColor;
            Gizmos.DrawLine(currentWeapon.ShotPoint.position, AimingPoint!);
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
        protected void ShootWeapon(InputAction.CallbackContext _)
        {
            currentWeapon.Fire(AimingPoint);
        }
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        protected void ShootWeapon()
        {
            currentWeapon.Fire(AimingPoint);
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