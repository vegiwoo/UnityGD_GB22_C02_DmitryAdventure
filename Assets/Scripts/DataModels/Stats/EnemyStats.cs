using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents characteristics of enemy.
    /// </summary>
    [CreateAssetMenu]
    public class EnemyStats : CharacterStats
    {
        [Tooltip("Contact distance with waypoint."), Range(0.1f, 1.0f)]
        public float pointContactDistance;
        
        [Tooltip("Distance from target at which enemy keeps when attacking."), Range(1,5)]
        public float distanceFromTarget;
    }
}
