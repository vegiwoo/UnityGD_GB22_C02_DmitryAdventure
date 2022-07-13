using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace DmitryAdventure
{
    public class SwitchVCam : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private int priorityBoostAmount = 10;
        
        private InputAction _aimAction;
        private CinemachineVirtualCamera _virtualCamera;
        
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _aimAction = playerInput.actions["Aim"];
        }

        private void OnEnable()
        {
            _aimAction.performed += StartAim;
            _aimAction.canceled += CancelAim;
        }

        private void OnDisable()
        {
            _aimAction.performed -= StartAim;
            _aimAction.canceled -= CancelAim;
        }

        private  void StartAim(InputAction.CallbackContext context)
        {
            _virtualCamera.Priority += priorityBoostAmount;
        }
        
        private  void CancelAim(InputAction.CallbackContext context)
        {
            _virtualCamera.Priority -= priorityBoostAmount;
        }
    }
}

