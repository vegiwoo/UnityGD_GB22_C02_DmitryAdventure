using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Refs:
 * - https://docs.unity3d.com/ru/2019.4/Manual/class-CharacterController.html
 * - https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
 * - https://youtu.be/SeBEvM2zMpY
 */

namespace DmitryAdventure
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField, Tooltip("Скорость персонажа")]
        private float playerSpeed;
        
        [SerializeField, Tooltip("Скорость вращения"), Range(1f,5f)]
        private float rotationSpeed;
        
        [SerializeField, Tooltip("Высота прыжка персонажа")]
        private float jumpHeight;

        private const float GravityValue = -9.81f;

        private CharacterController _controller;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];

            playerSpeed = 2f;
            jumpHeight = 0.7f;
            rotationSpeed = 4f;

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            var moveInput = _moveAction.ReadValue<Vector2>();
            var move = new Vector3(moveInput.x, 0, moveInput.y);

            if (_camera != null)
            {
                var camTransform = _camera.transform;
                move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
                move.y = 0f;
            }
            
            _controller.Move(move * Time.deltaTime * playerSpeed);
            
            if (_jumpAction.triggered && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * GravityValue);
            }

            _playerVelocity.y += GravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            
            // Rotate towards camera direction 
            if (_camera == null) return;
            var rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }
}