using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using DmitryAdventure.Stats;
using UnityEngine.AI;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Characters
{
    /// <summary>
    /// Represents item of an enemy.
    /// </summary>
    [RequireComponent(typeof(EnemyShooting), typeof(Blinked))]
    public class Enemy : Character
    {
        #region Сonstants, variables & properties

        private Rigidbody _rb;
        private NavMeshAgent _navMeshAgent;
        private DiscoveryTrigger _discoveryTrigger;

        [SerializeField] public EnemyStats enemyStats;
        public EnemyRoute Route { get; set; }

        /// <summary>
        /// Index of current waypoint.
        /// </summary>
        private int _currentWaypointIndex;

        /// <summary>
        /// Flag of enemy's movement forward along route.
        /// </summary>
        private bool _isMovingForward;
    
        /// <summary>
        /// Flag that determines begin and end of agent's movement through OffMeshLink.
        /// </summary>
        private bool _agentOnOffMeshLink;
        
        [field: SerializeField, Tooltip("Current state of enemy"), ReadonlyField] 
        public EnemyState CurrentEnemyState { get; private set; }
        
        private Coroutine _enemyPatrolCoroutine;
        private Coroutine _enemyAttackCoroutine;
        
        /// <summary>
        /// Character aiming point.
        /// </summary>
        private Transform _aimPoint;
        
        /// <summary>
        /// Character wait timer at checkpoint.
        /// </summary>
        private float currentCountdownValue;

        #endregion

        #region Monobehavior methods

        protected override void Awake()
        {
            base.Awake();
            
            _rb = GetComponent<Rigidbody>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
        }

        protected override void Start()
        {
            base.Start();
            
            CharacterType = enemyStats.CharacterType;
            CurrentHp = enemyStats.MaxHp;
            CurrentSpeed = enemyStats.BaseMoveSpeed;

            _rb.isKinematic = true;
            
            _navMeshAgent.speed = enemyStats.BaseMoveSpeed;
            _navMeshAgent.stoppingDistance = enemyStats.StopDistanceForWaypoints;
            
            _isMovingForward = true;
            _currentWaypointIndex = 0;
     
            _discoveryTrigger.Init(enemyStats.DiscoverableTypes);
            
            gameObject.tag = GameData.EnemyTag;

            currentCountdownValue = 0;
            
            ToggleEnemyState(EnemyState.Patrol);
        }

        private void OnEnable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify += DiscoveryTriggerHandler;
        }

        private void OnDisable()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= DiscoveryTriggerHandler;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #endregion
        
        #region Coroutines
        
        /// <summary>
        /// Coroutine for patrolling enemy.
        /// </summary>
        private IEnumerator EnemyPatrolCoroutine()
        {
            while (true)
            {
                var currentWaypoint = Route[PositionsRouteType.Current, _currentWaypointIndex];

                if (CurrentHp > 0 && _navMeshAgent.isActiveAndEnabled)
                {
                    _navMeshAgent.SetDestination(currentWaypoint);
                }

                var stopDistance = _navMeshAgent.stoppingDistance;
               
                // Change waypoint
               if (Math.Abs(transform.position.x - currentWaypoint.x) < stopDistance &&
                   Math.Abs(transform.position.z - currentWaypoint.z) < stopDistance)
               {
                   var result = Route.ChangeWaypoint(_isMovingForward, _currentWaypointIndex);
                   
                   // Waiting if point is checkpoint
                   if (result.isControlPoint)
                   {
                       yield return StartCoroutine(WaitingCoroutine(result.isAttentionIsIncreased, Route.WaitTime));
                   }
                   
                   _isMovingForward = result.isMoveForward;
                   _currentWaypointIndex = result.index;
               }
               
               if (CurrentEnemyState == EnemyState.Patrol)
               {
                   yield return null;
               }
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
            while (CurrentEnemyState == EnemyState.Attack && _aimPoint != null)
            {
                var distanceToTarget = Vector3.Distance(transform.position, _aimPoint.position);

                if (distanceToTarget < enemyStats.AttentionRadius && gameObject != null && _navMeshAgent.isActiveAndEnabled)
                {
                    if (distanceToTarget > enemyStats.MinAttackDistance)
                    {
                        _navMeshAgent.SetDestination(_aimPoint.position);
                    }
                    
                    yield return null;
                }
                else
                {
                    _navMeshAgent.ResetPath();
                    yield return new WaitForSeconds(Route.WaitTime);
                    
                    ToggleEnemyState(EnemyState.Patrol);
                    _enemyAttackCoroutine = null;
                    yield break;
                }
            }
        }

        /// <summary>
        /// Coroutine waiting for enemy at point.
        /// </summary>
        /// <param name="changeSizeOfDiscoveryTrigger">Need to change size of discovery trigger.</param>
        /// <param name="countdownValue">Wait timer value.</param>
        /// <returns></returns>
        private IEnumerator WaitingCoroutine(bool changeSizeOfDiscoveryTrigger, float countdownValue = 5)
        {
            currentCountdownValue = countdownValue;
            
            if (changeSizeOfDiscoveryTrigger)
            {
                _discoveryTrigger.ChangeSizeOfDiscoveryTrigger(true, Route.attentionIncreaseFactor);
            }

            while (currentCountdownValue > 0)
            {
                yield return new WaitForSeconds(1.0f);
                currentCountdownValue--;
            }
            
            if (changeSizeOfDiscoveryTrigger)
            {
                _discoveryTrigger.ChangeSizeOfDiscoveryTrigger(false);
            }
            
            currentCountdownValue = 0;
        }
        
        #endregion

        #region Event handlers
        
        /// <summary>
        /// Moves enemy when attacking.
        /// </summary>
        private void DiscoveryTriggerHandler(DiscoveryType type, Transform targetTransform, bool _)
        {
            if(!enemyStats.DiscoverableTypes.Contains(type)) return;
            
            switch (type)
            {
                case DiscoveryType.Player:
                    _aimPoint = targetTransform;
                    ToggleEnemyState(EnemyState.Attack);
                    break;
                default:
                    break;
            }
        }
        
        #endregion

        #region Other methods
        
        /// <summary>
        /// Changes state of enemy.
        /// </summary>
        /// <param name="state">New state of enemy.</param>
        private void ToggleEnemyState(EnemyState state)
        {
            CurrentEnemyState = state;

            switch (CurrentEnemyState)
            {
                case EnemyState.Patrol:
                    _aimPoint = null;
                    _enemyPatrolCoroutine = StartCoroutine(EnemyPatrolCoroutine());
                    _discoveryTrigger.DiscoveryTriggerNotify += DiscoveryTriggerHandler;
                    break;
                case EnemyState.Attack:
                     _enemyAttackCoroutine = StartCoroutine(EnemyAttackCoroutine());
                    _discoveryTrigger.DiscoveryTriggerNotify -= DiscoveryTriggerHandler;
                    break;
                default:
                    break;
            }
        }

        protected override void OnMovement()
        {
            // Do something
        }

        public override void OnHit(float damage)
        {
            CurrentHp -= damage;
            BlinkEffect.StartBlink();
        }
        
        #endregion
    }
}