using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Organizes game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables & constants

        [SerializeField] private PlayerShooting hero;
        [SerializeField] private EnemiesController enemiesController;
        [SerializeField] private AiminngColorize[] aimingColorizes;

        [Tooltip("Required number enemies on route")] [SerializeField, Range(1, 3)]
        private int numberEnemiesOnRoute = 1;

        private readonly List<Enemy> _enemies = new List<Enemy>(10);
        private Coroutine _walkingEnemiesCoroutine;

        private Vector3 _heroAimingPosition = Vector3.zero;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            _heroAimingPosition = Vector3.zero;
            hero.HeroAimingNotify += PlayerIsAiming;
        }

        private void Update()
        {
            if (enemiesController.routes.Length <= 0) return;
            CheckEnemies();
            _walkingEnemiesCoroutine = StartCoroutine(WalkingEnemiesCourutine());
        }

        private void OnDestroy()
        {
            hero.HeroAimingNotify -= PlayerIsAiming;
        }

        #endregion

        #region Functionality
        #region Coroutines

        /// <summary>
        /// Организует перемещние врагов по марштурам.
        /// </summary>
        /// <returns>Стандарный тип корутины.</returns>
        private IEnumerator WalkingEnemiesCourutine()
        {
            for (var i = 0; i < enemiesController.routes.Length; i++)
            {
                var currentRoute = enemiesController.routes[i];
                if (currentRoute.wayPoints.Length < 2) continue;

                var enemiesOnRoute = _enemies.Where(en => en.RouteNumber == i).ToList();
                if (enemiesOnRoute.Count == 0) continue;

                for (var j = 0; j < enemiesOnRoute.Count(); j++)
                {
                    var currentEnemy = enemiesOnRoute[j];
                    if (currentEnemy.State != EnemyState.Patrol) continue;

                    var currentEnemyTransform = enemiesOnRoute[j].transform;

                    if (Vector3.Distance(currentEnemyTransform.position, currentEnemy.TargetPoint) > 0.5f)
                    {
                        currentEnemy.transform.position = Vector3.MoveTowards(currentEnemy.transform.position,
                            currentEnemy.TargetPoint, 0.05f);

                        var rotateDir = currentEnemy.TargetPoint - currentEnemyTransform.position;
                        var rotation = Vector3.RotateTowards(currentEnemyTransform.forward,
                            new Vector3(rotateDir.x, 0, rotateDir.z),
                            10f * Time.deltaTime, 0f);
                        currentEnemyTransform.rotation = Quaternion.LookRotation(rotation);
                    }
                    else
                    {
                        currentEnemyTransform.position = currentEnemy.TargetPoint;
                        var indexOfTargetPoint = Array.IndexOf(currentRoute.wayPoints, currentEnemy.TargetPoint);

                        switch (currentEnemy.IsMovingForward)
                        {
                            case true:
                                if (currentEnemyTransform.position != currentRoute.wayPoints.Last())
                                    currentEnemy.TargetPoint = currentRoute.wayPoints[indexOfTargetPoint + 1];
                                else
                                {
                                    currentEnemy.IsMovingForward = false;
                                    currentEnemy.TargetPoint =
                                        currentRoute.wayPoints[currentRoute.wayPoints.Length - 1];
                                }

                                break;
                            case false:
                                if (currentEnemyTransform.position != currentRoute.wayPoints.First())
                                    currentEnemy.TargetPoint = currentRoute.wayPoints[indexOfTargetPoint - 1];
                                else
                                {
                                    currentEnemy.IsMovingForward = true;
                                    currentEnemy.TargetPoint = currentRoute.wayPoints[1];
                                }

                                break;
                        }
                    }
                }
            }
            yield return null;
        }
        
        #endregion

        #region Event handlers

        /// <summary>
        /// Gets a notification if player is aiming.
        /// </summary>
        /// <param name="aimingPosition">Aiming position.</param>
        private void PlayerIsAiming(Vector3 aimingPosition)
        {
            _heroAimingPosition = aimingPosition;

            if (_heroAimingPosition == Vector3.zero)
                foreach (var aiming in aimingColorizes)
                    aiming.SetColor(new Color32(237, 229,45,255)); 
            else
                foreach (var aiming in aimingColorizes)
                    aiming.SetColor(new Color32(121, 237,45,255)); 
        }

        #endregion

        #region Other methods

         /// <summary>
         /// Checks number of enemies on routes.
         /// </summary>
         private void CheckEnemies()
         {
             for (var i = 0; i < enemiesController.routes.Length; i++)
             {
                 if (_enemies != null && _enemies.Count(en => en.RouteNumber == i) == numberEnemiesOnRoute) continue;

                 var spawnPoint = enemiesController.routes[i].wayPoints[0];
                 var targetPoint = enemiesController.routes[i].wayPoints[1];
                 var newEnemy = Instantiate(enemiesController.enemyPrefab, spawnPoint, Quaternion.identity);
                 newEnemy.RouteNumber = i;
                 newEnemy.TargetPoint = targetPoint;
                 newEnemy.IsMovingForward = true;
                 _enemies.Add(newEnemy);
             }
         }
        #endregion
        #endregion
    }
}
