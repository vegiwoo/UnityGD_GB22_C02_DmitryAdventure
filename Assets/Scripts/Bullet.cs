using System;
using UnityEngine;

namespace DmitryAdventure
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject effectPrefab;
        private Rigidbody _bulletRigidbody;

        /// <summary>
        /// Скорость пули.
        /// </summary>
        public float BulletVelocity { get; set; }

        /// <summary>
        /// Точка выстрела
        /// </summary>
        public Transform PointOfShoot { get; set; }

        /// <summary>
        /// Позиция цели.
        /// </summary>
        public Vector3 TargetPosition { get; set; }

        /// <summary>
        /// Расстояние промаха пули от точки выстрела.
        /// </summary>
        private float MissDistance { get; set; }
        
        private void Start()
        {
            _bulletRigidbody = GetComponent<Rigidbody>();
            MissDistance = 50;
        }

        private void FixedUpdate()
        {
            if (PointOfShoot == null) return; 
            
            if (Vector3.Distance(PointOfShoot.position, transform.position) > MissDistance)
                Destroy(gameObject);
            
            _bulletRigidbody.velocity = (TargetPosition - PointOfShoot.position) * BulletVelocity / 2;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }
            
            Destroy(this.gameObject);
        }
    }
}

