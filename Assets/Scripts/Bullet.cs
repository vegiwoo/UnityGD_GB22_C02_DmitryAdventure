using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents item of projectile.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private GameObject effectPrefab;
        private Rigidbody _bulletRigidbody;
        
        public float BulletSpeed { get; set; }
        public float BulletRange { get; set; }
        public Transform PointOfShoot { get; set; }
        public Vector3 TargetPosition { get; set; }
        
        private int Damage { get; set; }

        #endregion

        #region Monobehavior methods

        private void Start()
         {
             _bulletRigidbody = GetComponent<Rigidbody>();
             BulletSpeed = 15;
             BulletRange = 50;
         }

         private void FixedUpdate()
         {
             if (Vector3.Distance(PointOfShoot.position, transform.position) > BulletRange)
             {
                 Destroy(gameObject);
             }

             _bulletRigidbody.velocity = (TargetPosition - PointOfShoot.position) * BulletSpeed / 2;
         }

         private void OnCollisionEnter(Collision collision)
         {
             Instantiate(effectPrefab, transform.position, Quaternion.identity);
             
             var character = collision.gameObject.GetComponent<Character>();
             if (character != null)
             {
                 character.OnHit(Damage);
             }

             Destroy(gameObject);
         }
        
         #endregion

        #region Functionality

        /// <summary>
        /// Assigning parameters for a bullet from a weapon.
        /// </summary>
        public void SetParams(Transform pointOfShoot, Vector3 targetPosition, float bulletSpeed, float bulletRange, int damage)
        {
            PointOfShoot = pointOfShoot;
            TargetPosition = targetPosition;
            BulletSpeed = bulletSpeed;
            BulletRange = bulletRange;
            Damage = damage;
        }
        #endregion
        
    }
}
