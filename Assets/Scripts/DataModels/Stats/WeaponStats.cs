using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// General stats of weapon.
    /// </summary>
    [CreateAssetMenu]
    public class WeaponStats : ScriptableObject
    {
        [Header("Required entities")]
        [SerializeField] private Bullet bulletPrefab;
        public Bullet BulletPrefab => bulletPrefab;

        [Header("Type")] 
        [SerializeField] private WeaponType weaponType;
        
        [Header("Weapon stats")] 
        [SerializeField, Tooltip("Shot range in meters"), Range(20f,50f)]
        private float shotRange;
        /// <summary>
        /// Shot range in meters.
        /// </summary>
        public float ShotRange => shotRange;

        [SerializeField, Tooltip("Shot delay in seconds"), Range(0.1f, 5.0f)]
        private float shotDelay;
        public float ShotDelay => shotDelay;

        [SerializeField, Tooltip("Weapon tilt angle in degrees"), Range(0f, 45f)]
        private int tiltAngleInDeg;
        /// <summary>
        /// Weapon tilt angle in degrees.
        /// </summary>
        public int TiltAngleInDeg => tiltAngleInDeg;
    }
}