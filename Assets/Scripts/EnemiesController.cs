using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Контроллер, предоставлющий функционал работы с врагами.
    /// </summary>
    [CreateAssetMenu]
    public class EnemiesController : ScriptableObject
    {
        [Tooltip("Коллекция маршрутов врагов")]
        public Route[] routes;

        [Tooltip("Префаб врага")]
        public Enemy enemyPrefab;
    }
}