using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DmitryAdventure
{
    public class PlayerShooting : MonoBehaviour
    {
        #region Variables & Constants
        
        public delegate void HeroAimingEvent(bool isAiming);
        public event HeroAimingEvent? HeroAimingNotify;
        
        [SerializeField, Tooltip("Character's current weapon")] 
        private Gun gun;
        
        private PlayerInput _playerInput;
        private InputAction _fireAction;
        private Camera _camera;
        private Vector3 _aimingPoint;

        #endregion
        
        #region Monobehavior methods
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _fireAction = _playerInput.actions["Fire"];
            
            _camera = Camera.main;
            _aimingPoint = Vector3.zero;
        }

        private void Update()
        {
            TakeAim();
        }
        
        private void OnEnable()
        {
            _fireAction.performed += ShootWeapon;
        }

        private void OnDisable()
        {
            _fireAction.performed -= ShootWeapon;
        }
        
        private void OnDrawGizmos()
        {
            if (_aimingPoint == Vector3.zero) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gun.Barrel.position, _aimingPoint!);
        }
        
        #endregion

        #region Other methods
        
        /// <summary>
        /// Performs weapon aiming.
        /// </summary>
        private void TakeAim()
        {
            var camTransform = _camera.transform;
            
            if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit)) 
                return;

            var isAiming = false;

            var targetDistance = Vector3.Distance(gun.Barrel.position, hit.point);
            if (targetDistance > gun.LowFiringRange && targetDistance < gun.UpFiringRange)
            {
                isAiming = true;
                _aimingPoint = hit.point;
            }
            else
                _aimingPoint = camTransform.position + camTransform.forward * gun.UpFiringRange;

            OnNotify(isAiming);
        }
        
        /// <summary>
        /// Performs a shot from a weapon.
        /// </summary>
        /// <remarks>
        /// Shot is fired only if  aiming point is not equal to zero. 
        /// </remarks>
        private void ShootWeapon(InputAction.CallbackContext _)
        {
            gun.Fire(_aimingPoint);
        }
        #endregion

        private void OnNotify(bool isAiming)
        {
            HeroAimingNotify?.Invoke(isAiming);
        }
    }
}


