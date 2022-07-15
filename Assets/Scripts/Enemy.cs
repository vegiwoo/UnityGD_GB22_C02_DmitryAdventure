using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents item of an enemy.
    /// </summary>
    public class Enemy : Character
    {
        #region Сonstants, variables & properties
        
        private Rigidbody _enemyRigidbody;

        /// <summary>
        /// Enemy route number.
        /// </summary>
        public int RouteNumber { get; set; }

        /// <summary>
        /// Current waypoint enemy is moving towards.
        /// </summary>
        public Vector3 CurrentWaypoint { get; set; }

        /// <summary>
        /// Flag of enemy moving forward along route.
        /// </summary>
        public bool IsMovingForward { get; set; }
        
        /// <summary>
        /// Enemy Attack Range.
        /// </summary>
        [Tooltip("Enemy Attack Range")] [SerializeField, Range(10f, 30f)]
        private float enemyAttackRange;

        /// <summary>
        /// Target of enemy attack.
        /// </summary>
        private Transform _targetEnemyAttack;

        /// <summary>
        /// Current enemy state.
        /// </summary>
        public EnemyState State { get; private set; }

        #endregion

        #region Monobehavior methods

        private void Awake()
        {
            _enemyRigidbody = transform.GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();
            _enemyRigidbody.mass = 30;
            enemyAttackRange = 30f;
            State = EnemyState.Patrol;
        }

        protected override void Update()
        {
            base.Update();
            
            if (State != EnemyState.Attack || _targetEnemyAttack == null) return;

            var direction = _targetEnemyAttack.position - transform.position;
            var rotation = Vector3.RotateTowards(transform.forward, direction, 10f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(rotation);

            if (Vector3.Distance(transform.position, _targetEnemyAttack.position) > enemyAttackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    _targetEnemyAttack.position, 0.05f);
            }
            // TODO: Стрельба в героя
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        // ...

        #endregion

        #region Other methods
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() == null) return;

            State = EnemyState.Attack;
            _targetEnemyAttack = other.gameObject.transform;
        }

        #endregion

        #endregion
    }
}