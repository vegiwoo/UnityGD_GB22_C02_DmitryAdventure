using System;
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

        private Rigidbody rb;
        
        [Tooltip("Радиус атаки")]
        [SerializeField, Range(5,15)] private float attackRadius;

        private Transform attackTarget;
        
        public EnemyState State { get; private set; }
        
        private void Awake()
        {
            rb = transform.GetComponent<Rigidbody>();
            rb.mass = 30;

            attackRadius = 5f;
        }

        private void Start()
        {
            State = EnemyState.Patrol;
        }

        private void Update()
        {
            if (State == EnemyState.Attack && attackTarget != null)
            {
                var direction = attackTarget.position - transform.position;
                var rotation = Vector3.RotateTowards(transform.forward, direction, 10f * Time.deltaTime, 0f);
                transform.rotation = Quaternion.LookRotation(rotation);

                if (Vector3.Distance(transform.position, attackTarget.position) > attackRadius)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        attackTarget.position, 0.05f);
                }
                
                // Стреляет в героя
            }
        }

        public void OnHit()
        {
            Debug.Log("OnHit!");
        }

        private void OnTriggerEnter(Collider other)
        {
            var hero = other.gameObject.GetComponent<PlayerController>().gameObject;
            if (hero != null)
            {
                State = EnemyState.Attack;
                attackTarget = hero.transform;
            }
        }
    }
}
