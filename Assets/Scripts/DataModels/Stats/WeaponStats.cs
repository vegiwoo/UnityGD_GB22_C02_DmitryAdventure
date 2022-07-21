using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Stats
{
    /// <summary>
    /// General stats of weapon.
    /// </summary>
    [CreateAssetMenu]
    public class WeaponStats : ScriptableObject
    {
        [field:Header("Required entities")]
        [field:SerializeField] public Bullet BulletPrefab { get; set; }

        [field:Header("Type")] 
        [field:SerializeField] public WeaponType WeaponType { get; set; }
        
        [field:Header("Weapon stats")] 
        [field:SerializeField, Tooltip("Shot range in meters"), Range(20f,50f)]
        public float ShotRange { get; set; }

        [field:SerializeField, Tooltip("Shot delay in seconds"), Range(0.1f, 5.0f)]
        public float ShotDelay { get; set; }

        [field:SerializeField, Tooltip("Weapon tilt angle in degrees"), Range(0f, 45f)]
        public int TiltAngleInDeg { get; set; }
        
        [field:SerializeField, Tooltip("Damage per shot"), Range(5, 15)]
        public int DamagePerShot { get; set; }
    }
}