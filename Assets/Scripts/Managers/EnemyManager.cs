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
        
        private Coroutine _walkingEnemiesCoroutine;

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
        #region Coroutines
        // ...
        #endregion

        #region Event handlers
        // ...
        #endregion

        #region Other methods

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
            
            for (var i = 0; i < routes.Length; i++)
            {
                if (_enemies.Count(en => en.Route.RouteNumber == i) == routes[i].MaxNumberEnemies) continue;

                var spawnPoint = routes[i].FirstWaypoint;
                var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                newEnemy.Route = routes[i];

                _enemies.Add(newEnemy);
            }
        }
        #endregion
        #endregion
    }
}