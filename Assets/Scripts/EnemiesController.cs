using System;
using UnityEngine;

namespace DmitryAdventure
{
    [CreateAssetMenu]
    public class EnemiesController : ScriptableObject
    {
        [Tooltip("Коллекция маршрутов врагов")]
        public Route[] routes;

        [Tooltip("Префаб врага")]
        public Enemy enemyPrefab;
    }
}