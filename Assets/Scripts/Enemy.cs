using UnityEngine;

// hp = 100;

namespace DmitryAdventure 
{
    /// <summary>
    /// Противник.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Номер маршрута врага
        /// </summary>
        public int RouteNumber { get;  set; }
        /// <summary>
        /// Текущая точка маршрута, к которой движется враг
        /// </summary>
        public Vector3 TargetPoint { get; set; }
        /// <summary>
        /// Скорость дивжения.
        /// </summary>
        public float MovingSpeed { get; set; } = 1f;
        /// <summary>
        /// Флаг движения врага вперед по маршруту.
        /// </summary>
        public bool IsMovingForward { get;  set; }

        private Rigidbody _enemyRigidbody;
        
        [Tooltip("Радиус атаки")]
        [SerializeField, Range(10f,20f)] private float attackRadius;

        /// <summary>
        /// Цель атаки
        /// </summary>
        private Transform _attackTarget;
        
        public EnemyState State { get; private set; }
        
        private void Awake()
        {
            _enemyRigidbody = transform.GetComponent<Rigidbody>();
            _enemyRigidbody.mass = 30;
            attackRadius = 15f;
        }

        private void Start()
        {
            State = EnemyState.Patrol;
        }

        private void Update()
        {
            if (State != EnemyState.Attack || _attackTarget == null) return;
            
            var direction = _attackTarget.position - transform.position;
            var rotation = Vector3.RotateTowards(transform.forward, direction, 10f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(rotation);

            if (Vector3.Distance(transform.position, _attackTarget.position) > attackRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    _attackTarget.position, 0.05f);
            }
            // TODO: Стрельба в героя
        }

        public void OnHit()
        {
            Debug.Log("OnHit!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() == null) return;
            
            State = EnemyState.Attack;
            _attackTarget = other.gameObject.transform;
        }
    }
}
