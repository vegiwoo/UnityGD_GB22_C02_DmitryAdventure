using UnityEngine;
using DmitryAdventure.WeaponsAndAmmunition;
using UnityEngine.InputSystem;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    public enum RaycastLayerType
    {
        Ignorance, Interaction
    }
    
    /// <summary>
    /// Represents character's shooting functionality.
    /// </summary>
    public abstract class CharacterShooting : MonoBehaviour
    {
        #region Сonstants, variables & properties
        
        [field:SerializeField, ReadonlyField, Tooltip("Character's chosen weapon")] 
        protected Weapon CurrentWeapon { get; set; }

        [field: SerializeField, Tooltip("Layer taken into account when aiming")]
        protected LayerMask LayerMask { get; set; }

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
        /// Performs a raycast of character's aiming.
        /// </summary>
        /// <param name="origin">Initial aiming point.</param>
        /// <param name="direction">Aiming direction</param>
        /// <param name="layerType">Type of interaction with passed LayerMask (interaction or ignorance LayerMask).</param>
        /// <param name="maxDistance">Maximum aiming distance.</param>
        /// <returns>Result of aiming.</returns>
        protected RaycastHit AimingRaycast(Vector3 origin, Vector3 direction, RaycastLayerType layerType,
            float maxDistance = Mathf.Infinity)
        {
            Physics.Raycast(
                origin, 
                direction, 
                out var hit, 
                maxDistance, 
                layerType == RaycastLayerType.Ignorance ? ~LayerMask : LayerMask
                );
            
            return hit;
        }

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