using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Data for working with enemies.
    /// </summary>
    [CreateAssetMenu]
    public class EnemiesData : ScriptableObject
    {
        [Tooltip("Collection of enemy routes")]
        public Route[] routes;

        [Tooltip("Enemy Prefab")]
        public Enemy enemyPrefab;
    }
}