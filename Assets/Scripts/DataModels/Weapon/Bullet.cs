using UnityEngine;
using DmitryAdventure.Characters;
using DmitryAdventure.Armament;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents item of projectile.
    /// </summary>
    public class Bullet : Ammunition
    {
        #region Сonstants, variables & properties
        private float BulletSpeed { get; set; }
        private float BulletRange { get; set; }
        private Transform PointOfShoot { get; set; }
        private Vector3 TargetPosition { get; set; }

        private string _targetTag;
        
        #endregion

        #region Monobehavior methods

        protected override void Start()
        {
            base.Start();
            BulletSpeed = 15;
            BulletRange = 50;
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(PointOfShoot.position, transform.position) > BulletRange)
            {
                Destroy(gameObject);
            }

            AmmunitionRigidbody.velocity = (TargetPosition - PointOfShoot.position) * BulletSpeed / 2;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (character == null) return;
  
            if (character.gameObject.CompareTag(_targetTag))
            {
                character.OnHit(Damage);
            }

            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Assigning parameters for a bullet from a weapon.
        /// </summary>
        public void Init(string targetTag, Transform pointOfShoot, Vector3 targetPosition, float bulletSpeed, float bulletRange,
            int damage)
        {
            _targetTag = targetTag;
            PointOfShoot = pointOfShoot;
            TargetPosition = targetPosition;
            BulletSpeed = bulletSpeed;
            BulletRange = bulletRange;
            Damage = damage;
        }

        #endregion
    }
}