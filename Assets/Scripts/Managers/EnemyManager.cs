using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DmitryAdventure
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
        public UnityEvent<int> killedEnemies;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _enemies = new List<Enemy>(12);
        }

        private void Update()
        {
            CheckingEnemiesOnRoutes();
        }

        #endregion

        #region Functionality

        #region Coroutines

   
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
                killedEnemies.Invoke(killed);
            }
            
            for (var i = 0; i < routes.Length; i++)
            {
                if (_enemies.Count(en => en.Route.Number == i) == routes[i].MaxNumberEnemies) continue;

                var spawnPoint = routes[i].StartPoint;
                var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                newEnemy.Route = routes[i];

                _enemies.Add(newEnemy);
            }
        }
    }
        #endregion
        #endregion
}