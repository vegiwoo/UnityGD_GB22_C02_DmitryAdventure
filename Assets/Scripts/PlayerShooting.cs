#nullable enable
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DmitryAdventure
{
    public class PlayerShooting : MonoBehaviour
    {
        #region Variables & Constants
        
        public delegate void HeroAimingEvent(Vector3 targetShooting);
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
            if (Camera.main != null) _camera = Camera.main;
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
            if (!Physics.Raycast(gun.Barrel.position, _camera.transform.forward, out var hit)) 
                return;
            
            var targetDistance = Vector3.Distance(gun.Barrel.position, hit.point);
            if (targetDistance > gun.LowFiringRange && targetDistance < gun.UpFiringRange)
                _aimingPoint = hit.point;
            else 
                _aimingPoint = Vector3.zero;
            
            OnNotify(_aimingPoint);
        }
        
        /// <summary>
        /// Performs a shot from a weapon.
        /// </summary>
        /// <remarks>
        /// Shot is fired only if  aiming point is not equal to zero. 
        /// </remarks>
        private void ShootWeapon(InputAction.CallbackContext _)
        {
            if (_aimingPoint != Vector3.zero)
                gun.Fire(_aimingPoint);
            
            else
            {
                // TODO: Выстрел в воздух (когда цель не захвачена)
            }
        }
        #endregion

        private void OnNotify(Vector3 targetShooting)
        {
            HeroAimingNotify?.Invoke(targetShooting);
        }
    }
}


