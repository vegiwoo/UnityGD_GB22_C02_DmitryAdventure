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

        private EnemyState _enemyState = EnemyState.Patrol;
        private Transform _attackTarget;

        private Blinked blinkEffect;
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _enemyRigidbody = transform.GetComponent<Rigidbody>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
            blinkEffect = GetComponent<Blinked>();
        }

        private void Start()
        {
            CurrentHp = enemyStats.MaxHP;
            CurrentSpeed = enemyStats.BaseMovementSpeed;

            _enemyRigidbody.mass = 30;
            _discoveryTrigger.DiscoveryTriggerNotify += OnAttackMovement;

            _currentWaypoint = Route[PositionType.Next, 0];

            //newEnemy.SetDiscoveryType(new [] { DiscoveryType.Player });
            
            _enemyPatrolCoroutine = StartCoroutine(EnemyPatrolCoroutine());
        }

        private void OnDestroy()
        {
            _discoveryTrigger.DiscoveryTriggerNotify -= OnAttackMovement;
        }

        #endregion

        #region Functionality
        #region Coroutines

        private IEnumerator EnemyAttackCoroutine(Transform targetTransform)
        {
            _enemyPatrolCoroutine = null;
            
            var distance = Vector3.Distance(transform.position, targetTransform.position);
            
            while (distance <= enemyStats.AttentionRadius)
            {
                var enemyTransform = transform;
                var enemyPosition = enemyTransform.position;
                var targetPosition = targetTransform.position;
                var direction = targetPosition - enemyPosition;
                var rotation = Vector3.RotateTowards(enemyTransform.forward, direction, enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(rotation);
                
                if(distance > enemyStats.MinAttackDistance)
                    transform.position = Vector3.MoveTowards(enemyPosition, targetPosition, MovementSpeedDelta);
                
                distance = Vector3.Distance(enemyPosition, targetPosition);
                
                yield return null;
                // TODO: стрелять в игрока
            }

            _enemyState = EnemyState.Patrol;
            _enemyAttackCoroutine = null;
            yield break;
        }

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
                yield return null;
            }
        }
        #endregion

        #region Event handlers

        // ...

        #endregion

        #region Other methods
        

        /// <summary>
        /// Assigns types of entities that will fall into field of view of current enemy.
        /// </summary>
        /// <param name="discoveryTypes">Entity types array.</param>
        public void SetDiscoveryType(DiscoveryType[] discoveryTypes)
        {
            _discoveryTrigger.SetDiscoveryTypes(discoveryTypes);
        }

        protected override void OnMovement()
        {
            // Do something...
        }

        public override void OnHit(float damage)
        {
            base.OnHit(damage);
            blinkEffect.StartBlink();
        }

        /// <summary>
        /// Moves enemy when attacking.
        /// </summary>
        private void OnAttackMovement(DiscoveryType type, Transform targetTransform)
        {
            _enemyState = EnemyState.Attack;
            _attackTarget = targetTransform;
        }

        #endregion
        #endregion
    }
}