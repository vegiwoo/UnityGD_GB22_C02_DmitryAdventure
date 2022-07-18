using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents general type of weapon.
    /// </summary>
    public sealed class Weapon : MonoBehaviour
    {
        #region Сonstants, variables & properties

        private static readonly float Gravity = Physics.gravity.y;

        [SerializeField] private WeaponStats weaponStats;
        /// <summary>
        /// Shot range in meter.
        /// </summary>
        public float ShotRange => weaponStats.ShotRange;

        [SerializeField, Tooltip("Firing point at end of gun barrel")] private Transform shotPoint;
       
        /// <summary>
        /// Firing point at end of gun barrel.
        /// </summary>
        public Transform ShotPoint => shotPoint; 
            
        [SerializeField] private AudioSource shotSound;

        /// <summary>
        /// A timer counting down time until next shot is possible.
        /// </summary>
        private float _shotDelayTimer;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _shotDelayTimer = 0f;
        }

        private void Update()
        {
            transform.localEulerAngles = new Vector3(-weaponStats.TiltAngleInDeg, 0, 0);
            
            if (_shotDelayTimer > 0)
                _shotDelayTimer -= Time.deltaTime;
        }

        #endregion

        #region Functionality
        #region Coroutines
        // ...
        #endregion

        #region Event handlers
        // ..
        #endregion

        #region Other methods

        /// <summary>
        /// Gets command to fire weapon.
        /// </summary>
        /// <param name="targetPosition">Target position to hit.</param>
        public void Fire(Vector3 targetPosition)
        {
            if(_shotDelayTimer > 0) return;

            // TODO: Implement as object pool
            var newBullet = Instantiate(weaponStats.BulletPrefab, shotPoint.position, shotPoint.rotation);
            
            newBullet.PointOfShoot = shotPoint;
            newBullet.TargetPosition = targetPosition;
            newBullet.BulletSpeed = CalculateBulletSpeed(targetPosition);;
            newBullet.BulletRange = weaponStats.ShotRange;

            shotSound.pitch = Random.Range(0.8f, 1.2f); 
            shotSound.Play();
            
            _shotDelayTimer = weaponStats.ShotDelay;
        }

        /// <summary>
        /// Calculates speed of a bullet along a ballistic trajectory.
        /// </summary>
        /// <param name="targetPosition">Target position to hit.</param>
        /// <returns>Speed of bullet.</returns>
        /// <remarks>https://youtu.be/lXSzdGBIPkg</remarks>
        private float CalculateBulletSpeed(Vector3 targetPosition)
        {
            var fromShooterToTarget = targetPosition - transform.position;
            var fromShooterToTargetXZ = new Vector3(fromShooterToTarget.x, 0, fromShooterToTarget.z);
            transform.rotation = Quaternion.LookRotation(fromShooterToTargetXZ, Vector3.up);
            
            var x = fromShooterToTargetXZ.magnitude;
            var y = fromShooterToTarget.y;

            var angleInRadians = weaponStats.TiltAngleInDeg * Mathf.PI / 180;
            var v2 = Gravity * Mathf.Pow(x, 2) /
                     (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
            return Mathf.Sqrt(Mathf.Abs(v2));
        }

        #endregion
        #endregion
    }
}