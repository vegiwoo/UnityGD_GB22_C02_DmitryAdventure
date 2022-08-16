using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DmitryAdventure.Stats;
using DmitryAdventure.Args;

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
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(PlayerShooting))]
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
        private InputAction _aimAction;
        private InputAction _mouse;
        
        private Camera _camera;

        // Audio
        [field: SerializeField] private AudioClip eatingSound;
        [field: SerializeField] private AudioClip errorSound;
        
        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _controller = gameObject.GetComponent<CharacterController>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _camera = Camera.main;
            
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _runAction = _playerInput.actions["Run"];
            _therapyAction = _playerInput.actions["Therapy"];
            _aimAction = _playerInput.actions["Aim"];
            _mouse = _playerInput.actions["Mouse"];
        }

        protected override void Start()
        {
            base.Start();
            
            CurrentHp = playerStats.MaxHp;
            CharacterType = playerStats.CharacterType;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _therapyAction.performed += OnTherapyPerformed;
            _aimAction.performed += OnTakesAimPerformed; 
            _aimAction.canceled += OnTakesAimCancelled;
        }

        private void OnDisable()
        {
            _therapyAction.performed -= OnTherapyPerformed;
            _aimAction.performed -= OnTakesAimPerformed; 
            _aimAction.canceled -= OnTakesAimCancelled;
        }
        
        #endregion

        #region Functionality
        
        protected override void OnMovement()
        {
            _groundedPlayer = _controller.isGrounded;
            if (_groundedPlayer && _playerVelocity.y < 0)
            {
                _playerVelocity.y = 0f;
            }

            var moveInput = _moveAction.ReadValue<Vector2>();
            var move = new Vector3(moveInput.x, 0, moveInput.y);
            var camTransform = _camera.transform;
            move = move.x * camTransform.right.normalized + move.z * camTransform.forward.normalized;
            move.y = 0f;

            CurrentSpeed = playerStats.BaseMoveSpeed;
            if (_runAction.inProgress)
            {
                CurrentSpeed += playerStats.AccelerationFactor;
            }

            _controller.Move(move * (Time.deltaTime * CurrentSpeed));

            if (_jumpAction.triggered && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(playerStats.JumpHeight * -3.0f * GameData.Gravity);
                // Passing speed value to animator
                CharacterAnimator.SetTrigger("Jump");
            }
            
            _playerVelocity.y += GameData.Gravity * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);

            var x = Screen.width / 2;
            var mouseX = _mouse.ReadValue<Vector2>().x;

            // Rotate towards camera direction 
            var rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
            var angle =  Quaternion.Angle(transform.rotation, rotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, playerStats.BaseRotationSpeed * Time.deltaTime);

            // Passing speed value to animator
            CharacterAnimator.SetFloat(
                AnimatorSpeed, 
                move == Vector3.zero ? 0 : _runAction.inProgress ? 1.0f : 0.5f, 
                0.1f, 
                Time.deltaTime);
         
           // Debug.Log($"{angleDelta > 0}, {angle}");
            CharacterAnimator.SetFloat("Rotation",  x > mouseX ? angle : -angle);
        }

        /// <summary>
        /// Handler for selecting a mine from inventory.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        private void OnTherapyPerformed(InputAction.CallbackContext context)
        {
            var medicine = FindItemInInventory(GameData.MedicineKey);
            if (medicine == null)
            {
                AudioSource.PlayClipAtPoint(errorSound, gameObject.transform.position);
            }
            else
            {
                CurrentHp = CurrentHp + medicine.HpBoostRate <= playerStats.MaxHp
                    ? CurrentHp += medicine.HpBoostRate
                    : playerStats.MaxHp;
                
                AudioSource.PlayClipAtPoint(eatingSound, gameObject.transform.position);

                var args = new CharacterEventArgs(CharacterType.Player, CurrentHp);
                OnCharacterNotify(args);
            }
        }
        
        private void OnTakesAimPerformed(InputAction.CallbackContext context)
        {
            IsCharacterCanMove = false;
        }

        private void OnTakesAimCancelled(InputAction.CallbackContext context)
        {
            IsCharacterCanMove = true;
        }

        public override void OnHit(float damage)
        {
            CurrentHp -= damage;
            BlinkEffect.StartBlink();
            
            var args = new CharacterEventArgs(CharacterType.Player, CurrentHp);
            OnCharacterNotify(args);
        }

        #endregion
    }
}