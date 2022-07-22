using UnityEngine;
using DmitryAdventure.Stats;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Armament
{
    /// <summary>
    /// Represents general type of weapon.
    /// </summary>
    public sealed class Weapon : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] public WeaponStats weaponStats;
        
        [field:SerializeField, Tooltip("Firing point at end of gun barrel")] 
        public Transform ShotPoint { get; set; }

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
            {
                _shotDelayTimer -= Time.deltaTime;
            }
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
            if (_shotDelayTimer > 0) return;

            // TODO: Implement as object pool
            var newBullet = Instantiate(weaponStats.BulletPrefab, ShotPoint.position, ShotPoint.rotation);
            newBullet.SetParams(ShotPoint, targetPosition, CalculateBulletSpeed(targetPosition), weaponStats.ShotRange,
                weaponStats.DamagePerShot);
            
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
            var v2 = GameData.Gravity * Mathf.Pow(x, 2) /
                     (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
            return Mathf.Sqrt(Mathf.Abs(v2));
        }

        #endregion
        #endregion
    }
}