using System.Collections;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents item of an enemy.
    /// </summary>
    [RequireComponent(typeof(Blinked))]
    public class Enemy : Character
    {
        #region Сonstants, variables & properties

        [SerializeField] public EnemyStats enemyStats;
        [SerializeField, Tooltip("Array of discoverable types for trigger")] 
        public DiscoveryType [] discoveryTypes;
        public EnemyRoute Route { get; set; }

        /// <summary>
        /// Current destination of route.
        /// </summary>
        private Vector3 _currentWaypoint;

        /// <summary>
        /// Flag of enemy's movement forward along route.
        /// </summary>
        private bool _isMovingForward = true;
        
        private Rigidbody _enemyRigidbody;
        private DiscoveryTrigger _discoveryTrigger;

        private Coroutine _enemyPatrolCoroutine;
        private Coroutine _enemyAttackCoroutine;

        private EnemyState _enemyState;
        private Transform _discoveryTarget;

        private Blinked _blinkEffect;
        
        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _enemyRigidbody = transform.GetComponent<Rigidbody>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _blinkEffect = GetComponent<Blinked>();
        }

        private void Start()
        {
            characterType = CharacterType.EnemyType01;
            CurrentHp = enemyStats.MaxHp;
            CurrentSpeed = enemyStats.BaseMovementSpeed;
            _enemyRigidbody.mass = 30;
            _currentWaypoint = Route[PositionsRouteType.Next, 0];

            _discoveryTrigger.DiscoverableTypes = discoveryTypes;
            _discoveryTrigger.DiscoveryTriggerNotify += OnFindingTarget;
            
            ToggleEnemyState(EnemyState.Patrol);
            
            // TODO: Weapon for enemy!
            
            
        }

        private void OnDestroy()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= OnFindingTarget;
        }

        #endregion

        #region Functionality
        #region Coroutines
        
        /// <summary>
        /// Coroutine for patrolling the enemy.
        /// </summary>
        private IEnumerator EnemyPatrolCoroutine()
        {
            while (_enemyState == EnemyState.Patrol)
            {
                if (Vector3.Distance(transform.position, _currentWaypoint) > enemyStats.PointContactDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        _currentWaypoint, MovementSpeedDelta * Time.deltaTime);
                    
                    var rotateDir = _currentWaypoint - transform.position;
                    var rotation = Vector3.RotateTowards(transform.forward,
                        new Vector3(rotateDir.x, 0, rotateDir.z),
                        enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                    transform.rotation = Quaternion.LookRotation(rotation);
                }
                else
                {
                    var result = Route.ChangeWaypoint(_isMovingForward, _currentWaypoint);
                    _isMovingForward = result.isMovingForward;
                    _currentWaypoint = result.currentWayPoint;
                }

                if (_enemyState == EnemyState.Patrol)
                    yield return null;
                else
                {          
                    _enemyPatrolCoroutine = null;
                    yield break;
                }
            }
        }

        /// <summary>
        /// Coroutine chasing and attacking enemy.
        /// </summary>
        private IEnumerator EnemyAttackCoroutine()
        {
            while (_enemyState == EnemyState.Attack && _discoveryTarget != null)
            {
                var distanceToTarget = Vector3.Distance(transform.position, _discoveryTarget.position);
                if (distanceToTarget <= enemyStats.AttentionRadius)
                {
                    var rotateDir = _discoveryTarget.position - transform.position;
                    var rotation = Vector3.RotateTowards(transform.forward,
                        new Vector3(rotateDir.x, 0, rotateDir.z),
                        enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                    transform.rotation = Quaternion.LookRotation(rotation);
                    
                    // TODO: Стрелять в цель 

                    if (distanceToTarget > enemyStats.MinAttackDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position,
                            _discoveryTarget.position, (MovementSpeedDelta / 2 * Time.deltaTime));
                    }
                    yield return null;
                } else
                {          
                    ToggleEnemyState(EnemyState.Patrol);
                    _enemyAttackCoroutine = null;
                    yield break;
                }
            }
        }
        #endregion

        #region Event handlers
        
        /// <summary>
        /// Moves enemy when attacking.
        /// </summary>
        private void OnFindingTarget(DiscoveryType type, Transform targetTransform, bool _)
        {
            _discoveryTarget = targetTransform;
            
            switch (type)
            {
                case DiscoveryType.Player:
                    ToggleEnemyState(EnemyState.Attack);
                    break;
                default:
                    break;
            }
        }
        
        #endregion

        #region Other methods

        protected override void OnMovement()
        {
            // Do something...
        }

        protected override void OnTakeAim()
        {
            // Прицеливается когда атакует
        }


        /// <summary>
        /// Changes state of enemy.
        /// </summary>
        /// <param name="state">New state of enemy.</param>
        private void ToggleEnemyState(EnemyState state)
        {
            _enemyState = state;

            switch (_enemyState)
            {
                case EnemyState.Patrol:
                    _discoveryTarget = null;
                    _enemyPatrolCoroutine = StartCoroutine(EnemyPatrolCoroutine());
                    break;
                case EnemyState.Attack:
                    _enemyAttackCoroutine = StartCoroutine(EnemyAttackCoroutine());
                    break;
                default:
                    break;
            }
        }
        
        public override void OnHit(float damage)
        {
            CurrentHp -= damage;
            _blinkEffect.StartBlink();
        }
        
        #endregion
        #endregion
    }
}
