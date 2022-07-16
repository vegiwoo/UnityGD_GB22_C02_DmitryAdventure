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
    /// <summary>
    /// Represents main character.
    /// </summary>
    [RequireComponent(typeof(PlayerStats))]
    public class Player : Character
    {
        #region Сonstants, variables & properties

        [SerializeField] public PlayerStats playerStats;
        
        private CharacterController _controller;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _runAction;
        
        private Camera _camera;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _runAction = _playerInput.actions["Run"];

            _camera = Camera.main;
        }

        protected override void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
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

        protected override void OnMovement()
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
            
            CurrentSpeed = playerStats.baseMovementSpeed;
            if (_runAction.inProgress)
                CurrentSpeed += playerStats.accelerationFactor;
            _controller.Move(move * (Time.deltaTime * CurrentSpeed));
            
            if (_jumpAction.triggered && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(playerStats.jumpHeight * -3.0f * GravityValue);
            }

            _playerVelocity.y += GravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            
            // Rotate towards camera direction 
            if (_camera == null) return;
            var rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, playerStats.baseRotationSpeed * Time.deltaTime);
        }

        #endregion

        #endregion
    }
}