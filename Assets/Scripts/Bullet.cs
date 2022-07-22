using UnityEngine;
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