using System;
using UnityEngine;
using DmitryAdventure.Armament;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Represents character's shooting functionality.
    /// </summary>
    public class CharacterShooting : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private Mine minePrefab;

        [field:SerializeField, Tooltip("Character's chosen weapon")] 
        private Weapon CurrentWeapon { get; set; }

        [field: SerializeField, Tooltip("Sight beam color (for Gismo)")]
        private Color AimingColor { get; set; }

        private Vector3 _aimingPoint;
        
        private Camera _camera;
        private Character _character;
        private CharacterInventory _characterInventory;
        
        private PlayerInput _playerInput;
        private InputAction _characterFireAction;
        private InputAction _characterMineAction;
        #endregion
        
        #region Monobehavior methods

        private void Awake()
        {
            _camera = Camera.main;
            _character = gameObject.GetComponent<Character>();

            if (_character.CharacterType != CharacterType.Player) return;
            
            _characterInventory = gameObject.GetComponent<CharacterInventory>();
            _playerInput = gameObject.GetComponent<PlayerInput>();
        }
        
        private void Start()
        {
            _aimingPoint = Vector3.zero;

            if (_character.CharacterType != CharacterType.Player) return;
            
            _characterFireAction = _playerInput.actions["Fire"];
            _characterFireAction.performed += ShootWeapon;
            
            _characterMineAction = _playerInput.actions["Mine"];
            _characterMineAction.performed += MineActionOnPerformed;
        }

        private void Update()
        {
            OnTakeAim();
        }

        private void OnDrawGizmos()
        {
            if (_aimingPoint == Vector3.zero) return;
            
            Gizmos.color = AimingColor;
            Gizmos.DrawLine(CurrentWeapon.ShotPoint.position, _aimingPoint!);
        }

        private void OnDestroy()
        {
            if (_character.CharacterType != CharacterType.Player) return;
            
            _characterFireAction.performed -= ShootWeapon;
            _characterMineAction.performed -= MineActionOnPerformed;
        }

        #endregion

      


        #region Event handlers

        /// <summary>
        /// Handler for selecting a mine from inventory.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        private void MineActionOnPerformed(InputAction.CallbackContext context)
        {
            if(_characterInventory == null) return;
            
            var popMine = _characterInventory.PopFromInventory(GameData.MineKey);
            if (popMine == null) return;

            Instantiate(minePrefab, new Vector3(_aimingPoint.x,_aimingPoint.y + 0.2f,_aimingPoint.z), Quaternion.identity);
        }
        
        #endregion

        #region Other methods

        /// <summary>
        /// Aiming character.
        /// </summary>
        private void OnTakeAim()
        {
            switch (_character.CharacterType)
            {
                case CharacterType.Player:
                {
                    var camTransform = _camera.transform;
            
                    if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit)) 
                        return;

                    var targetDistance = Vector3.Distance( CurrentWeapon.ShotPoint.position, hit.point);
                    if (targetDistance < CurrentWeapon.weaponStats.ShotRange)
                    {
                        _aimingPoint = hit.point;
                    }
                    else
                    {
                        _aimingPoint = camTransform.position + camTransform.forward * CurrentWeapon.weaponStats.ShotRange;
                    }
                    break;
                }
                case CharacterType.EnemyType01:
                    
                    var enemy = _character as Enemy;
                    if (enemy == null) return;
                    
                    if (enemy.CurrentEnemyState == EnemyState.Attack)
                    {
                        var eObject = enemy.gameObject;
                        var ePosition = eObject.transform.position;
                        var eForward = eObject.transform.forward;
                        var layerMask = GameData.PlayerLayerMask;
                        
                        if (Physics.Raycast(
                                ePosition,
                                eForward,
                                out var playerHit,
                                enemy.enemyStats.AttentionRadius,
                                layerMask))
                        {
                            var bounds = playerHit.collider.bounds;
                            
                            _aimingPoint = new Vector3(
                                Random.Range(bounds.min.x, bounds.max.x),
                                Random.Range(bounds.min.y, bounds.max.y),
                                Random.Range(bounds.min.z, bounds.max.z)
                            );
                            ShootWeapon();
                        }
                    }

                    break;
            }
        }
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <param name="context">CallbackContext for more info.</param>
        /// <remarks>For player.</remarks>>
        private void ShootWeapon(InputAction.CallbackContext context)
        {
            CurrentWeapon.Fire(_aimingPoint);
        }
        
        /// <summary>
        /// Fires a shot from a weapon.
        /// </summary>
        /// <remarks>For NPC.</remarks>>
        protected void ShootWeapon()
        {
            CurrentWeapon.Fire(_aimingPoint);
        }

        #endregion
  
    }
}