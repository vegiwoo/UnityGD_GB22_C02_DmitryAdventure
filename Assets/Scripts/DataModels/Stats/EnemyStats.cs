using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents characteristics of enemy.
    /// </summary>
    [CreateAssetMenu]
    public class EnemyStats : CharacterStats
    {
        [field: SerializeField, Tooltip("Contact distance with waypoint."), Range(0.1f, 1.0f)]
        public float PointContactDistance { get; set; }

        [field:SerializeField,Tooltip("Distance from target at which enemy keeps when attacking."), Range(1,5)]
        public float MinAttackDistance { get; set; }
    }
}