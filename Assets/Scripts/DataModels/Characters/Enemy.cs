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
    [RequireComponent(typeof(CharacterShooting), typeof(Blinked))]
    public class Enemy : Character
    {
        #region Сonstants, variables & properties

        //private Rigidbody _enemyRigidbody;
        private NavMeshAgent _navMeshAgent;
        private DiscoveryTrigger _discoveryTrigger;
        private Blinked _blinkEffect;
        
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
        
        private Transform _aimPoint;
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _navMeshAgent = transform.GetComponent<NavMeshAgent>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            _blinkEffect = GetComponent<Blinked>();
        }

        private void Start()
        {
            CharacterType = enemyStats.CharacterType;
            CurrentHp = enemyStats.MaxHp;
            CurrentSpeed = enemyStats.BaseMoveSpeed;

            _navMeshAgent.speed = enemyStats.BaseMoveSpeed;
            _navMeshAgent.stoppingDistance = enemyStats.StopDistanceForWaypoints;
            
            _isMovingForward = true;
            _currentWaypointIndex = 0;
     
            _discoveryTrigger.DiscoverableTypes = enemyStats.DiscoverableTypes;
            _discoveryTrigger.DiscoveryTriggerNotify += DiscoveryTriggerHandler;
            
            ToggleEnemyState(EnemyState.Patrol);
        }

        protected override void Update()
        {
            base.Update();
            _agentOnOffMeshLink = _navMeshAgent.isOnOffMeshLink switch
            {
                true when !_agentOnOffMeshLink => true,
                false when _agentOnOffMeshLink => false,
                _ => _agentOnOffMeshLink
            };
        }

        private void OnDestroy()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= DiscoveryTriggerHandler;
        }

        #endregion

        #region Functionality
        #region Coroutines
        
        /// <summary>
        /// Coroutine for patrolling the enemy.
        /// </summary>
        private IEnumerator EnemyPatrolCoroutine()
        {

            while (true)
            {
                var currentWaypoint = Route[PositionsRouteType.Current, _currentWaypointIndex];

                _navMeshAgent.SetDestination(currentWaypoint);

               var stopDistance = _navMeshAgent.stoppingDistance;
               
               if (Math.Abs(transform.position.x - currentWaypoint.x) < stopDistance &&
                   Math.Abs(transform.position.z - currentWaypoint.z) < stopDistance)
               {
                   var result = Route.ChangeWaypoint(_isMovingForward, _currentWaypointIndex);
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

                if (distanceToTarget < enemyStats.AttentionRadius)
                {
                    if (distanceToTarget > enemyStats.MinAttackDistance)
                    {
                        _navMeshAgent.SetDestination(_aimPoint.position);
                    }
                    
                    yield return null;
                }
                else
                { ;
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
            _blinkEffect.StartBlink();
        }
        
        #endregion
        #endregion
    }
}
