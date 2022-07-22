using UnityEngine;
using DmitryAdventure.Armament;
using DmitryAdventure.Characters;

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

        // HACK: Testing AudioSource.
        private AudioSource AudioSource { get; set; }

        #endregion

        #region Monobehavior methods
        protected override void Start()
         {
             base.Start();
             BulletSpeed = 15;
             BulletRange = 50;
             
             // HACK: Testing AudioSource.
             // Здесь по сути повтооряю настройки как если бы создавал компонент в редакторе
             AudioSource = gameObject.AddComponent<AudioSource>();
             AudioSource.clip = effectSoundClip;
             AudioSource.playOnAwake = false;
             AudioSource.pitch = Random.Range(0.85f, 1.0f);
             AudioSource.panStereo = AudioSource.spatialBlend = AudioSource.spread = 0;
             AudioSource.volume = AudioSource.reverbZoneMix = AudioSource.dopplerLevel = 1;
             AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
             AudioSource.minDistance = 1;
             AudioSource.maxDistance = 500;
         }

        private void FixedUpdate()
        {
            if (Vector3.Distance(PointOfShoot.position, transform.position) > BulletRange)
            {
                Destroy(gameObject);
            }

            AmmunitionRigidbody.velocity = (TargetPosition - PointOfShoot.position) * BulletSpeed / 2;
        }
        
        // HACK: Testing AudioSource.
        protected override void OnCollisionEnter(Collision collision)
        {
            var character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.OnHit(Damage);
            }
  
            // Звук не проигрывается, но если перенести в Start - проигрывается, но глуховатый и
            // кажется обрезается (можно сравнить с оригиналом в директории Audio окна Project)
            AudioSource.Play();
            //AudioSource.PlayClipAtPoint(effectSoundClip, transform.position);

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