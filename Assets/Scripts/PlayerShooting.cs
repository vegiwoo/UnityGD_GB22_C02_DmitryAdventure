using DmitryAdventure.Characters;
using UnityEngine;
using UnityEngine.InputSystem;
using DmitryAdventure.WeaponsAndAmmunition;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents player's shooting functionality.
    /// </summary>
    public class PlayerShooting : CharacterShooting
    {
        #region Сonstants, variables & properties

        [SerializeField] private Mine minePrefab;
                
        private Camera _camera;
        private PlayerInput _playerInput;
        private Animator _playerAnimator;
        
        private CharacterInventory _characterInventory;
        
        private InputAction _aimAction;
        private InputAction _fireAction;
        private InputAction _mineAction;

        private static readonly int AnimatorAim = Animator.StringToHash("Aim");
        private static readonly int ShootingAim = Animator.StringToHash("Shooting");
        
        private const string AttackLayerName = "Attack Layer";
        
        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _camera = Camera.main;
            _characterInventory = gameObject.GetComponent<CharacterInventory>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _playerAnimator = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void Start()
        {
            base.Start();
            
            if (Character.CharacterType != CharacterType.Player) return;
        }

        private void OnEnable()
        {
            _aimAction = _playerInput.actions["Aim"];
            _fireAction = _playerInput.actions["Fire"];
            _mineAction = _playerInput.actions["Mine"];
            
            _aimAction.performed += OnTakesAimPerformed; 
            _aimAction.canceled += OnTakesAimCancelled;
            
            _fireAction.performed += ShootWeapon;
            _mineAction.performed += MineActionOnPerformed;
        }

        private void OnDisable()
        {
            _aimAction.performed -= OnTakesAimPerformed;
            _aimAction.canceled -= OnTakesAimCancelled; 
            
            _fireAction.performed -= ShootWeapon;
            _mineAction.performed -= MineActionOnPerformed;
        }

        #endregion

        #region Functionality

        protected override void OnTakeAim()
        {
            var camTransform = _camera.transform;

            var hit = AimingRaycast(camTransform.position, camTransform.forward, RaycastLayerType.Ignorance);
            var point = hit.point;
            if(point == default) return;
            
            var targetDistance = Vector3.Distance( CurrentWeapon.ShotPoint.position, point);
            if (targetDistance < CurrentWeapon.weaponStats.ShotRange)
            {
                AimPoint = point;
            }
            else
            {
                AimPoint = camTransform.position + camTransform.forward * CurrentWeapon.weaponStats.ShotRange;
            }
        }

        /// <summary>
        /// Callback to handle player's aiming.
        /// </summary>
        /// <param name="context">CallbackContext as a source of additional info.</param>
        private void OnTakesAimPerformed(InputAction.CallbackContext context)
        {
            _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(AttackLayerName),1);
            _playerAnimator.SetBool(AnimatorAim, true);
            _playerAnimator.SetFloat(ShootingAim, 0);
        }
        
        /// <summary>
        /// Callback to handle player's aiming.
        /// </summary>
        /// <param name="context">CallbackContext as a source of additional info.</param>
        private void OnTakesAimCancelled(InputAction.CallbackContext context)
        {
            _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(AttackLayerName),0);
            _playerAnimator.SetBool(AnimatorAim, false);
            _playerAnimator.SetFloat(ShootingAim, 0);
        }
        
        /// <summary>
        /// Handler for selecting a mine from inventory.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        private void MineActionOnPerformed(InputAction.CallbackContext context)
        {
            if(_characterInventory == null) return;
            
            var popMine = _characterInventory.PopFromInventory(GameData.MineKey);
            if (popMine == null) return;

            Instantiate(minePrefab, new Vector3(AimPoint.x,AimPoint.y + 0.2f,AimPoint.z), Quaternion.identity);
        }

        #endregion
    }
}