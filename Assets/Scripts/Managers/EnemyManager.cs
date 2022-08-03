using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DmitryAdventure.Characters;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Controls movement of enemies on routes.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private EnemyRoute[] routes;
        
        /// <summary>
        /// Inner collection of enemies.
        /// </summary>
        private List<Enemy> _enemies;

        // Events 
        public UnityEvent<int> killedEnemiesEvent;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _enemies = new List<Enemy>(32);
        }

        private void Update()
        {
            CheckingEnemiesOnRoutes();
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Checks number of active enemies on route.
        /// </summary>
        private void CheckingEnemiesOnRoutes()
        {
            // Remove killed enemies.
            var killed = _enemies.RemoveAll(ch => ch.CurrentHp < 0);
            if (killed > 0)
            {
                killedEnemiesEvent.Invoke(killed);
            }
            
            foreach (var route in routes)
            {
                var enemiesOnRouteCount = _enemies.Count(en => en.Route == route);
                
                if (enemiesOnRouteCount == route.MaxNumberEnemies) continue;

                var spawnPoint = route.FirstWaypoint;
                var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                newEnemy.Route = route;

                _enemies.Add(newEnemy);
            }
        }

        #endregion
    }
}