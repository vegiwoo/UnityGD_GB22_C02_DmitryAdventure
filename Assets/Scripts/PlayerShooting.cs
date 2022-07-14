using UnityEngine;
using UnityEngine.InputSystem;

namespace DmitryAdventure
{
    public class PlayerShooting : MonoBehaviour
    {
        /// <summary>
        /// Оружие персонажа.
        /// </summary>
        [SerializeField] private Gun gun;
        
        private PlayerInput _playerInput;
        private InputAction _fireAction;
        
        private Camera _camera;
     
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _fireAction = _playerInput.actions["Fire"];
            
            _camera = Camera.main;
        }
        
        private void OnEnable()
        {
            _fireAction.performed += ShootWeapon;
        }

        private void OnDisable()
        {
            _fireAction.performed -= ShootWeapon;
        }
        
        private void ShootWeapon(InputAction.CallbackContext context)
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out var hit))
            {
                gun.Fire(hit.point);
            }
            else
            {
                // TODO: Выстрел в воздух (когда цель не захвачена)
            }
        }
    }
}


