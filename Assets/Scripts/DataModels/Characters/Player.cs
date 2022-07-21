using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DmitryAdventure.Stats;
using JetBrains.Annotations;

/*
 * Refs:
 * - https://docs.unity3d.com/ru/2019.4/Manual/class-CharacterController.html
 * - https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
 * - https://youtu.be/SeBEvM2zMpY
 */

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Represents main character.
    /// </summary>
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(CharacterShooting))]
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
        private InputAction _therapyAction;
        
        private Camera _camera;
        
        private Blinked _blinkEffect;

        [field: SerializeField] private AudioClip eatingSound;
        [field: SerializeField] private AudioClip errorSound;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _controller = gameObject.GetComponent<CharacterController>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
            CharacterInventory = gameObject.GetComponent<CharacterInventory>();
            
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _runAction = _playerInput.actions["Run"];
            _therapyAction = _playerInput.actions["Therapy"];

            _camera = Camera.main;

            _blinkEffect = GetComponent<Blinked>();
        }

        private void Start()
        {
            CurrentHp = playerStats.MaxHp;

            _therapyAction.performed += TherapyActionOnPerformed;
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            _therapyAction.performed -= TherapyActionOnPerformed;
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers
        /// <summary>
        /// Handler for selecting a mine from inventory.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        private void TherapyActionOnPerformed(InputAction.CallbackContext context)
        {
            var medicine = FindItemInInventory(GameData.MedicineLabelText);
            if (medicine == null)
            {
                AudioSource.PlayClipAtPoint(errorSound, gameObject.transform.position);
            }
            else
            {
                CurrentHp += medicine.HpBoostRate;
                AudioSource.PlayClipAtPoint(eatingSound, gameObject.transform.position);

                var args = new CharacterEventArgs(CharacterType.Player, CurrentHp);
                OnCharacterNotify(args);
            }
        }
        #endregion

        #region Other methods

        // Movement
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
            
            CurrentSpeed = playerStats.BaseMoveSpeed;
            if (_runAction.inProgress)
            {
                CurrentSpeed += playerStats.AccelerationFactor;
            }
            _controller.Move(move * (Time.deltaTime * CurrentSpeed));
            
            if (_jumpAction.triggered && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(playerStats.JumpHeight * -3.0f * GameData.Gravity);
            }

            _playerVelocity.y += GameData.Gravity * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            
            // Rotate towards camera direction 
            if (_camera == null) return;
            var rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, playerStats.BaseRotationSpeed * Time.deltaTime);
        }

        public override void OnHit(float damage)
        {
            CurrentHp -= damage;
            _blinkEffect.StartBlink();
            
            var args = new CharacterEventArgs(CharacterType.Player, CurrentHp);
            OnCharacterNotify(args);
        }
        
        #endregion
        #endregion
    }
}