using System.Collections;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents item of an enemy.
    /// </summary>
    [RequireComponent(typeof(EnemyStats))]
    public class Enemy : Character
    {
        #region Сonstants, variables & properties

        [SerializeField] public EnemyStats enemyStats;

        public int RouteNumber { get; set; }
        
        /// <summary>
        /// Current destination of route.
        /// </summary>
        public Vector3 CurrentWaypoint { get; set; }

        /// <summary>
        /// Flag of enemy's movement forward along route.
        /// </summary>
        public bool IsMovingForward { get; set; }
        
        private Rigidbody _enemyRigidbody;
        private DiscoveryTrigger _discoveryTrigger;
        private Coroutine _enemyAttackCoroutine;

        public EnemyState State { get; private set; }
        
        #endregion

        #region Monobehavior methods

        private void Awake()
        {
             enemyStats = transform.GetComponent<EnemyStats>();
            _enemyRigidbody = transform.GetComponent<Rigidbody>();
            _discoveryTrigger = GetComponentInChildren<DiscoveryTrigger>();
        }

        protected override void Start()
        {
            base.Start();
            
            _enemyRigidbody.mass = 30;
            State = EnemyState.Patrol;
            
            _discoveryTrigger.DiscoveryTriggerNotify += OnAttackMovement;
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
            var distance = Vector3.Distance(transform.position, targetTransform.position);
            
            while (distance <= enemyStats.attentionRadius)
            {
                var enemyTransform = transform;
                var enemyPosition = enemyTransform.position;
                var targetPosition = targetTransform.position;
                var direction = targetPosition - enemyPosition;
                var rotation = Vector3.RotateTowards(enemyTransform.forward, direction, enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(rotation);
                
                if(distance > enemyStats.distanceFromTarget)
                    transform.position = Vector3.MoveTowards(enemyPosition, targetPosition, MovementSpeedDelta);
                
                distance = Vector3.Distance(enemyPosition, targetPosition);
                
                yield return null;
                // TODO: стрелять в игрока
            }

            State = EnemyState.Patrol;
            _enemyAttackCoroutine = null;
            yield break;


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
        

        /// <summary>
        /// Moves enemy when attacking.
        /// </summary>
        private void OnAttackMovement(DiscoveryType type, Transform targetTransform)
        {
            if (type != DiscoveryType.Player || State == EnemyState.Attack) return;
            State = EnemyState.Attack;

            _enemyAttackCoroutine = StartCoroutine(EnemyAttackCoroutine(targetTransform));
        }

        #endregion
        #endregion
    }
}