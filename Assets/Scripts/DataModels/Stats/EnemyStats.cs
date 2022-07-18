using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents characteristics of enemy.
    /// </summary>
    [CreateAssetMenu]
    public class EnemyStats : CharacterStats
    {
        [SerializeField,Tooltip("Contact distance with waypoint."), Range(0.1f, 1.0f)]
        private float pointContactDistance;
        public float PointContactDistance => pointContactDistance;
        
        [SerializeField,Tooltip("Distance from target at which enemy keeps when attacking."), Range(1,5)]
        private float minAttackDistance;
        public float MinAttackDistance => minAttackDistance;
    }
}
