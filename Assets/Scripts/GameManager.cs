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
        [SerializeField] private EnemiesData enemiesData;
        [SerializeField] private AiminngColorize[] aimingColorizes;

        private readonly List<Enemy> _enemies = new(10);
        private Coroutine _walkingEnemiesCoroutine;
        
        #endregion

        #region Monobehavior methods

        private void Start()
        {
            hero.HeroAimingNotify += PlayerIsAiming;
        }

        private void Update()
        {
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
        /// Organizes movement of enemies along routes.
        /// </summary>
        private IEnumerator WalkingEnemiesCourutine()
        {
            for (var i = 0; i < enemiesData.routes.Length; i++)
            {
                var currentRoute = enemiesData.routes[i];
                if (currentRoute.wayPoints.Length < 2) continue;

                var enemiesOnRoute = _enemies.Where(en => en.RouteNumber == i).ToList();
                if (enemiesOnRoute.Count == 0) continue;

                for (var j = 0; j < enemiesOnRoute.Count(); j++)
                {
                    var ce = enemiesOnRoute[j];
                    if (ce.State != EnemyState.Patrol) continue;

                    var currentEnemyTransform = enemiesOnRoute[j].transform;

                    if (Vector3.Distance(currentEnemyTransform.position, ce.CurrentWaypoint) > ce.enemyStats.pointContactDistance)
                    {
                        ce.transform.position = Vector3.MoveTowards(ce.transform.position,
                            ce.CurrentWaypoint, ce.MovementSpeedDelta);

                        var rotateDir = ce.CurrentWaypoint - currentEnemyTransform.position;
                        var rotation = Vector3.RotateTowards(currentEnemyTransform.forward,
                            new Vector3(rotateDir.x, 0, rotateDir.z),
                            ce.enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                        currentEnemyTransform.rotation = Quaternion.LookRotation(rotation);
                    }
                    else
                    {
                        currentEnemyTransform.position = ce.CurrentWaypoint;
                        var indexOfTargetPoint = Array.IndexOf(currentRoute.wayPoints, ce.CurrentWaypoint);

                        switch (ce.IsMovingForward)
                        {
                            case true:
                                if (currentEnemyTransform.position != currentRoute.wayPoints.Last())
                                    ce.CurrentWaypoint = currentRoute.wayPoints[indexOfTargetPoint + 1];
                                else
                                {
                                    ce.IsMovingForward = false;
                                    ce.CurrentWaypoint =
                                        currentRoute.wayPoints[currentRoute.wayPoints.Length - 1];
                                }

                                break;
                            case false:
                                if (currentEnemyTransform.position != currentRoute.wayPoints.First())
                                    ce.CurrentWaypoint = currentRoute.wayPoints[indexOfTargetPoint - 1];
                                else
                                {
                                    ce.IsMovingForward = true;
                                    ce.CurrentWaypoint = currentRoute.wayPoints[1];
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
        /// <param name="isAiming">Did player aim.</param>
        private void PlayerIsAiming(bool isAiming)
        {
            if (!isAiming)
                foreach (var aiming in aimingColorizes)
                    aiming.Set(new Color32(237, 229,45,255)); 
            else
                foreach (var aiming in aimingColorizes)
                    aiming.Set(new Color32(121, 237,45,255)); 
        }

        #endregion

        #region Other methods

         /// <summary>
         /// Checks number of enemies on routes.
         /// </summary>
         private void CheckEnemies()
         {
             
             
             
             
             
             
             
             
             // // Removing dead enemies from route.
             // for (var ei = 0; ei < _enemies.Count; ei++)
             //     if (_enemies[ei].CurrentHp <= 0)
             //         _enemies.RemoveAt(ei);
             //
             // // Checking required number of enemies on route.
             // for (var routeNumber = 0; routeNumber < enemiesData.routes.Length; routeNumber++)
             // {
             //     var currentRoute = enemiesData.routes[routeNumber];
             //
             //     if (_enemies.Count(en => en.RouteNumber.Equals(routeNumber)) == currentRoute.numberEnemies ||
             //         currentRoute.wayPoints.Length < 2) continue;
             //
             //     var spawnPoint = currentRoute.wayPoints[0];
             //     var targetPoint = currentRoute.wayPoints[1];
             //     
             //     var newEnemy = Instantiate(enemiesData.enemyPrefab, spawnPoint, Quaternion.identity);
             //     newEnemy.RouteNumber = routeNumber;
             //     newEnemy.CurrentWaypoint = targetPoint;
             //     newEnemy.IsMovingForward = true;
             //     newEnemy.SetDiscoveryType(new [] { DiscoveryType.Player });
             //     
             //     _enemies.Add(newEnemy);
             // }
         }

         #endregion

         #endregion
    }
}
