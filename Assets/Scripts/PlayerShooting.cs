using System.Collections;
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
        private GameObject _currentWeapon;
        
        private CharacterInventory _characterInventory;
        
        private InputAction _aimAction;
        private InputAction _fireAction;
        private InputAction _mineAction;

        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _camera = Camera.main;
            _characterInventory = gameObject.GetComponent<CharacterInventory>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
            _playerAnimator = gameObject.GetComponentInChildren<Animator>();
            _currentWeapon = gameObject.GetComponentInChildren<Weapon>().gameObject;
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
            StartCoroutine(ChangeAnimatorLayerWeight(ShootingLayerName, 0, 1));
        }
        
        /// <summary>
        /// Callback to handle player's aiming.
        /// </summary>
        /// <param name="context">CallbackContext as a source of additional info.</param>
        private void OnTakesAimCancelled(InputAction.CallbackContext context)
        {
            StartCoroutine(ChangeAnimatorLayerWeight(ShootingLayerName, 1, 0));
        }

        private IEnumerator ChangeAnimatorLayerWeight(string layerName, float currentValue, float sourceValue)
        {
            var current = currentValue;
            const float step = 0.05f;
            
            if (current < sourceValue)
            {
                while (current < sourceValue)
                {
                    current += step;
                    _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(layerName), current);
                    yield return null;
                }
            }
            else
            {
                while (current > sourceValue)
                {
                    current -= step;
                    _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(layerName),current);
                    yield return null;
                }
            }
            
            _playerAnimator.SetLayerWeight(_playerAnimator.GetLayerIndex(layerName),sourceValue);
            _currentWeapon.SetActive(currentValue < sourceValue);
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