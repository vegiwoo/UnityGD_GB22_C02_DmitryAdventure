using UnityEngine;

namespace DmitryAdventure
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject effectPrefab;
        private Rigidbody rb;
        private float bulletSpeed = 20f;

        /// <summary>
        /// Точка выстрела
        /// </summary>
        private Vector3 PointOfShoot { get; set; }

        /// <summary>
        /// Расстояние промаха пули от точки выстрела.
        /// </summary>
        private float MissDistance { get; set; } = 50f;
        
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            PointOfShoot = transform.position;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(PointOfShoot, transform.position) > MissDistance)
                Destroy(gameObject);
            else
                rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }
            
            Destroy(gameObject);
        }
    }
}

