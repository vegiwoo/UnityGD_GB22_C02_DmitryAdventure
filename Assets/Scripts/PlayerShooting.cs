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
        private CharacterInventory _characterInventory;
        private PlayerInput _playerInput;
        private InputAction _characterFireAction;
        private InputAction _characterMineAction;

        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _camera = Camera.main;
            _characterInventory = gameObject.GetComponent<CharacterInventory>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
        }

        protected override void Start()
        {
            base.Start();
            if (Character.CharacterType != CharacterType.Player) return;
            _characterFireAction = _playerInput.actions["Fire"];
            _characterFireAction.performed += ShootWeapon;
            _characterMineAction = _playerInput.actions["Mine"];
            _characterMineAction.performed += MineActionOnPerformed;
        }

        private void OnDestroy()
        {
            _characterFireAction.performed -= ShootWeapon;
            _characterMineAction.performed -= MineActionOnPerformed;
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