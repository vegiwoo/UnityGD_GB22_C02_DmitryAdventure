using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        private List<Enemy> _enemies;
        
        private Coroutine _walkingEnemiesCoroutine;

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
            _enemies.RemoveAll(ch => ch.CurrentHp < 0);

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