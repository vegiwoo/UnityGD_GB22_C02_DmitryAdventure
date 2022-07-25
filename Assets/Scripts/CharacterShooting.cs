using UnityEngine;
using DmitryAdventure.Armament;
using UnityEngine.InputSystem;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Represents character's shooting functionality.
    /// </summary>
    public abstract class CharacterShooting : MonoBehaviour
    {
        #region Сonstants, variables & properties
        
        [field:SerializeField, ReadonlyField, Tooltip("Character's chosen weapon")] 
        protected Weapon CurrentWeapon { get; set; }
        
        protected Vector3 AimPoint;
        
        [field: SerializeField, Tooltip("Sight beam color (for Gismo)")]
        private Color AimColor { get; set; }
        
        protected Character Character;

        #endregion
        
        #region Monobehavior methods

        protected virtual void Awake()
        {
            CurrentWeapon = GetComponentInChildren<Weapon>();
            Character = gameObject.GetComponent<Character>();
        }
        
        protected virtual void Start()
        {
            AimPoint = Vector3.zero;
        }

        private void Update()
        {
            OnTakeAim();
        }

        private void OnDrawGizmos()
        {
            if (AimPoint == Vector3.zero) return;
            
            Gizmos.color = AimColor;
            Gizmos.DrawLine(CurrentWeapon.ShotPoint.position, AimPoint!);
        }
        #endregion

        #region Other methods

        protected abstract void OnTakeAim();

        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        /// <remarks>For player.</remarks>>
        protected void ShootWeapon(InputAction.CallbackContext context)
        {
            CurrentWeapon.Fire(AimPoint);
        }
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <remarks>For NPC.</remarks>>
        protected void ShootWeapon()
        {
            CurrentWeapon.Fire(AimPoint);
        }

        #endregion
    }
}