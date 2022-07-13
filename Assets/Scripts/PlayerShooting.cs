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
        
        private PlayerInput playerInput;
        private InputAction fireAction;
        
        private Camera _camera;
     
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            fireAction = playerInput.actions["Fire"];
            
            _camera = Camera.main;
        }
        
        private void OnEnable()
        {
            fireAction.performed += ShootWeapon;
        }

        private void OnDisable()
        {
            fireAction.performed -= ShootWeapon;
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


