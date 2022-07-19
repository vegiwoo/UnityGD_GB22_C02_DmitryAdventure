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
    [RequireComponent(typeof(CharacterInventory))]
    public class Player : Character
    {
        #region Сonstants, variables & properties

        [SerializeField] public PlayerStats playerStats;
        [SerializeField] private Mine minePrefab;
        
        private CharacterController _controller;
        private PlayerInput _playerInput;
        private Vector3 _playerVelocity;
        private bool _groundedPlayer;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _runAction;
        private InputAction _fireAction;
        private InputAction _mineAction;
        
        private Camera _camera;

        private static CharacterInventory _characterInventory;

        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _controller = GetComponent<CharacterController>();
            _characterInventory = GetComponent<CharacterInventory>();
            
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _runAction = _playerInput.actions["Run"];
            _fireAction = _playerInput.actions["Fire"];
            _mineAction = _playerInput.actions["Mine"];

            _camera = Camera.main;
        }

        private void Start()
        {
            CurrentHp = playerStats.MaxHp;
            Cursor.lockState = CursorLockMode.Locked;
            
            _fireAction.performed += ShootWeapon;
            _mineAction.performed += MineActionOnPerformed;
        }
        
        private void OnDestroy()
        {
            _fireAction.performed -= ShootWeapon;
            _mineAction.performed -= MineActionOnPerformed;
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers
        
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
            
            CurrentSpeed = playerStats.BaseMovementSpeed;
            if (_runAction.inProgress)
                CurrentSpeed += playerStats.AccelerationFactor;
            _controller.Move(move * (Time.deltaTime * CurrentSpeed));
            
            if (_jumpAction.triggered && _groundedPlayer)
            {
                _playerVelocity.y += Mathf.Sqrt(playerStats.JumpHeight * -3.0f * GravityValue);
            }

            _playerVelocity.y += GravityValue * Time.deltaTime;
            _controller.Move(_playerVelocity * Time.deltaTime);
            
            // Rotate towards camera direction 
            if (_camera == null) return;
            var rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, playerStats.BaseRotationSpeed * Time.deltaTime);
        }
        
        // Shooting
        protected override void OnTakeAim()
        {
            var camTransform = _camera.transform;
            
            if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit)) 
                return;

            var targetDistance = Vector3.Distance(currentWeapon.ShotPoint.position, hit.point);
            if (targetDistance < currentWeapon.ShotRange)
            {
                AimingPoint = hit.point;
            }
            else
                AimingPoint = camTransform.position + camTransform.forward * currentWeapon.ShotRange;
        }
        
        public override void OnHit(float damage)
        {
            CurrentHp = -damage;
            var args = new CharacterEventArgs(CharacterType.Player, CurrentHp);
            OnCharacterNotify(args);
        }

        private void MineActionOnPerformed(InputAction.CallbackContext context)
        {
            var popMine = _characterInventory.PopFromInventory(GameData.MineKey);
            if (popMine == null) return;

            Instantiate(minePrefab, new Vector3(AimingPoint.x,AimingPoint.y + 0.2f,AimingPoint.z), Quaternion.identity);
        }

        #endregion
        #endregion
    }
}