using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DmitryAdventure.Args;
using UnityEngine;
using UnityEngine.Events;
using DmitryAdventure.Characters;
using Events;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Controls movement of enemies on routes.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        #region Links
        [field: SerializeField] private EnemyEvent EnemyEvent { get; set; }

        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private EnemyRoute[] routes;
        
        /// <summary>
        /// Inner collection of enemies.
        /// </summary>
        private List<Enemy> _enemies;

        private int _unitsRemovedSum;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _enemies = new List<Enemy>(32);
            _unitsRemovedSum = 0;
            
            StartCoroutine(CheckingEnemiesOnRoutes());
        }

        #endregion

        #region Functionality

        /// <summary>
        /// Checks number of active enemies on route.
        /// </summary>
        private IEnumerator CheckingEnemiesOnRoutes()
        {
            while (routes.Length > 0)
            {
                yield return StartCoroutine(RemoveKilledEnemies());
                
                foreach (var route in routes)
                {
                    var enemiesOnRouteCount = _enemies.Count(en => en.Route == route);
                
                    if (enemiesOnRouteCount == route.MaxNumberEnemies || route.SpawnTimer != 0) continue;

                    var spawnPoint = route.FirstWaypoint;
                    var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                    newEnemy.Route = route;

                    _enemies.Add(newEnemy);

                    // Notify
                    var args = new EnemyArgs(route.RouteNumber, 1, 0, _unitsRemovedSum);
                    EnemyEvent.Notify(args);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Removes killed enemies from routes.
        /// </summary>
        /// <remarks>
        /// Sends an event to game manager to keep track of killed enemies.
        /// </remarks>>
        private IEnumerator RemoveKilledEnemies()
        {
            // Dictionary of killed enemies (key - route number, value - number of killed)
            var killedEnemies = new Dictionary<int, int>();

            for (var i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].CurrentHp > 0) continue;
                
                var routeNumber = _enemies[i].Route.RouteNumber;
                killedEnemies[routeNumber] = killedEnemies.ContainsKey(routeNumber) ? 
                        killedEnemies[routeNumber] + 1 : 
                        1;

                _enemies.RemoveAt(i);
            }

            // Notify
            foreach (var killedOnRoute in killedEnemies)
            {
                _unitsRemovedSum += killedOnRoute.Value;
                var args = new EnemyArgs(killedOnRoute.Key, 0, killedOnRoute.Value, _unitsRemovedSum);
                EnemyEvent.Notify(args);
            }

            yield return null;
        }

        #endregion
    }
}