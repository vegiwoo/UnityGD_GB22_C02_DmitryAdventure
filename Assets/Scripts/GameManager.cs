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
            walkingEnemiesCoroutine = StartCoroutine(WalkingEnemiesCourutine());
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
                newEnemy.transform.LookAt(targetPoint);
                enemies.Add(newEnemy);
            }
        }

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
                var enemiesOnRoute = enemies.Where(en => en.RouteNumber == i).ToList();
                
                if(enemiesOnRoute.Count == 0) continue;

                for (var j = 0; j < enemiesOnRoute.Count(); j++)
                {
                    var currentEnemy = enemiesOnRoute[j];
                    var currentEnemyTransform = enemiesOnRoute[j].transform;
                    
                    if (!currentEnemy.IsReachedTargetPoint)
                    {
                        currentEnemy.transform.position = Vector3.MoveTowards(currentEnemy.transform.position,
                              currentEnemy.TargetPoint, currentEnemy.MovingSpeed / 12);
       
                        currentEnemy.transform.LookAt(currentEnemy.TargetPoint);

                        // var targetDirection = currentEnemyTransform.position - currentEnemy.TargetPoint;
                        // var newDirection = Vector3.RotateTowards(currentEnemyTransform.forward, targetDirection, 1 * Time.deltaTime, 0.0f);
                        // currentEnemyTransform.rotation = Quaternion.LookRotation(newDirection);
                    }
                    else
                    {
                        Debug.Log("Yep!");
                        currentEnemyTransform.position = currentEnemy.TargetPoint;

                        // Индекс текущей позиции в массиве точек маршрута
                        var indexOfTargetPoint = Array.IndexOf(currentRoute.wayPoints, currentEnemy.TargetPoint);

                        switch (currentEnemy.IsMovingForward)
                        {
                            // Движение по маршруту вперед  
                            case true:
                                // Если враг не на последней точке маршрута
                                if (currentEnemyTransform.position != currentRoute.wayPoints.Last())
                                {
                                    currentEnemy.Set(currentRoute.wayPoints[indexOfTargetPoint + 1]);
                                }
                                else
                                {
                                    currentEnemy.Set(false);
                                    currentEnemy.Set(currentRoute.wayPoints[currentRoute.wayPoints.Length - 1]);
                                    
                                }
                                break;
                            // Движение по маршруту назад  
                            case false:
                                // Если враг не на первой точке маршрута
                                if (currentEnemyTransform.position != currentRoute.wayPoints.First())
                                {
                                    currentEnemy.Set(currentRoute.wayPoints[indexOfTargetPoint - 1]);
                                }
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



// Перебираем все маршруты + 
// Находим всех врагов на маршруте + 
// Проходим по всем врагам на маршруте + 
//   Если враг не на своей целевой точке +
//      Двигаем его к целевой точке +
//   Или если враг на целевой точке +
//      Присваиваем целевую точку его позиции +
//      Если двигается вперед + 
//         Если он не на последней точке маршршрута, присваиваем целевой точкой следующую точку маршрута + 
//         Или если он на последней точке маршрута, меняем его направление и  присваиваем целевой точкой предудущую точку маршрута
//      Или если он двигается назад
//         Если он не на первой точке маршрута, присваиваем целевой точкой предыдушую точку маршрута
//         Или если он на первой точке маршрута, меняем его направление и присваиваем целевой точкой следующую маршрута
