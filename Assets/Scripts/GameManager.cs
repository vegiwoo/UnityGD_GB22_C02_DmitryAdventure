using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace DmitryAdventure
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Контроллер, предоставлющий функционал работы с врагами.
        /// </summary>
        [FormerlySerializedAs("enemies")] [SerializeField] private EnemiesController enemiesController;
        
        [Tooltip("Количество активных врагов на маршруте")]
        [SerializeField, Range(1,3)] private int numberEnemiesOnRoute = 1;

        private List<Enemy> enemies = new List<Enemy>(10);

        private Coroutine walkingEnemiesCoroutine;
        
        
        private void Update()
        {
            CheckEnemies();
            if (enemiesController.routes.Length <= 0)
            {
                walkingEnemiesCoroutine = StartCoroutine(WalkingEnemiesCourutine());
            }
        }
        
        /// <summary>
        /// Проверяет наличтие врагов на маршрутах 
        /// </summary>
        private void CheckEnemies()
        {
            if (enemiesController.routes.Length <= 0) return;
            
            for (var i = 0; i < enemiesController.routes.Length; i++)
            {
                if (enemies.Count(en => en.RouteNumber == i) == numberEnemiesOnRoute) continue;

                var spawnPoint = enemiesController.routes[i].wayPoints[0];
                var targetPoint = enemiesController.routes[i].wayPoints[1];
                var newEnemy = Instantiate(enemiesController.enemyPrefab, spawnPoint, Quaternion.identity);
                newEnemy.Set(i);
                newEnemy.Set(targetPoint);
                newEnemy.Set(true);
                enemies.Add(newEnemy);
            }
        }

        /// <summary>
        /// Организует перемещние врагов по марштурам.
        /// </summary>
        /// <returns>Стандарный тип корутины.</returns>
        private IEnumerator WalkingEnemiesCourutine()
        {
            if (enemiesController.routes.Length <= 0)
            {
                walkingEnemiesCoroutine = null;
                yield break;
            }
            
            for (var i = 0; i < enemiesController.routes.Length; i++)
            {
                var currentRoute = enemiesController.routes[i];
                if (currentRoute.wayPoints.Length < 2) continue;
                
                var enemiesOnRoute = enemies.Where(en => en.RouteNumber == i).ToList();
                
                if (enemiesOnRoute.Count == 0) continue;

                for (var j = 0; j < enemiesOnRoute.Count(); j++)
                {
                    var currentEnemy = enemiesOnRoute[j];
                    var currentEnemyTransform = enemiesOnRoute[j].transform;
                    
                    if (Vector3.Distance(currentEnemyTransform.position, currentEnemy.TargetPoint) > 0.5f)
                    {
                        currentEnemy.transform.position = Vector3.MoveTowards(currentEnemy.transform.position,
                              currentEnemy.TargetPoint, currentEnemy.MovingSpeed / 12);

                        var rotateDir = currentEnemy.TargetPoint - currentEnemyTransform.position;
                        var rotation = Vector3.RotateTowards(currentEnemyTransform.forward, new Vector3(rotateDir.x, 0, rotateDir.z),
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
                                    currentEnemy.Set(currentRoute.wayPoints[indexOfTargetPoint + 1]);
                                else
                                {
                                    currentEnemy.Set(false);
                                    currentEnemy.Set(currentRoute.wayPoints[currentRoute.wayPoints.Length - 1]);
                                }
                                break;
                            case false:
                                if (currentEnemyTransform.position != currentRoute.wayPoints.First())
                                    currentEnemy.Set(currentRoute.wayPoints[indexOfTargetPoint - 1]);
                                else
                                {
                                    currentEnemy.Set(true);
                                    currentEnemy.Set(currentRoute.wayPoints[1]);
                                }
                                break;
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
