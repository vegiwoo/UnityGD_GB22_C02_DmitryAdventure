using System.Collections;
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
        public UnityEvent<(int routeNumber, int createCount)> createEnemiesEvent;
        public UnityEvent<Dictionary<int, int>> killedEnemiesEvent;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _enemies = new List<Enemy>(32);
            StartCoroutine(CheckingEnemiesOnRoutes());
        }

        private void OnEnable()
        {
            foreach (var route in routes)
            {
                createEnemiesEvent.AddListener(route.OnCreateEnemiesEvent);
                killedEnemiesEvent.AddListener(route.OnRemoveKilledEnemies);
            }
        }

        private void OnDisable()
        {
            createEnemiesEvent.RemoveAllListeners();
            killedEnemiesEvent.RemoveAllListeners();
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
                    createEnemiesEvent.Invoke((route.RouteNumber, 1));
                }

                yield return null;
            }
        }

        /// <summary>
        /// Removes killed enemies from routes.
        /// </summary>
        private IEnumerator RemoveKilledEnemies()
        {
            // Dictionary of killed enemies (key - route number, value - number of killed)
            var killedEnemies = new Dictionary<int, int>();

            for (var i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].CurrentHp > 0) continue;
                
                var routeNumber = _enemies[i].Route.RouteNumber;
                if (killedEnemies.ContainsKey(routeNumber))
                {
                    killedEnemies[routeNumber] ++;
                }
                else
                {
                    killedEnemies[routeNumber] = 1;
                }

                _enemies.RemoveAt(i);
            }
            
            killedEnemiesEvent.Invoke(killedEnemies);
            
            yield return null;
        }

        #endregion
    }
}