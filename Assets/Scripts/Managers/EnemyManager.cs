using System;
using System.Collections;
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
            _walkingEnemiesCoroutine = StartCoroutine(WalkingEnemiesCoroutine());
        }

        #endregion

        #region Functionality

        #region Coroutines

        private IEnumerator WalkingEnemiesCoroutine()
        {
            for (var cri = 0; cri < routes.Length; cri++)
            {
                var cr = routes[cri];
                if (cr.WayPoints.Length < 2) continue;
                
                var enemiesOnRoute = _enemies.Where(en => en.RouteNumber == cri).ToList();
                if (enemiesOnRoute.Count == 0) continue;

                for (var cei = 0; cei < enemiesOnRoute.Count(); cei++)
                {
                    var ce = enemiesOnRoute[cei];
                    if (ce.State != EnemyState.Patrol) continue;
                    
                    var cet = enemiesOnRoute[cei].transform;

                    if (Vector3.Distance(cet.position, ce.CurrentWaypoint) >
                        ce.enemyStats.pointContactDistance)
                    {
                        ce.transform.position = Vector3.MoveTowards(ce.transform.position,  ce.CurrentWaypoint, 0.05f);
                        var rotateDir = ce.CurrentWaypoint - cet.position;
                        var rotation = Vector3.RotateTowards(cet.forward,
                            new Vector3(rotateDir.x, 0, rotateDir.z),
                            ce.enemyStats.RotationAngleDelta * Time.deltaTime, 0f);
                        cet.rotation = Quaternion.LookRotation(rotation);
                    }
                    else
                    {
                        cet.position = ce.CurrentWaypoint;
                        var indexOfTargetPoint = Array.IndexOf(cr.WayPoints, ce.CurrentWaypoint);
                        var nextPosition = cr.NextPositionFrom(indexOfTargetPoint);
                        var previousPosition = cr.PreviousPositionFrom(indexOfTargetPoint);
                        
                        switch (ce.IsMovingForward)
                        {
                            case true:
                                if (cet.position != cr.LastPosition && nextPosition != null)
                                    ce.CurrentWaypoint = (Vector3)nextPosition;
                                else if(previousPosition != null)
                                {
                                    ce.IsMovingForward = false;
                                    ce.CurrentWaypoint = (Vector3)previousPosition;
                                }
                                break;
                            case false:
                                if (cet.position != cr.FirstPosition && previousPosition != null)
                                    ce.CurrentWaypoint = (Vector3)previousPosition;
                                else if (nextPosition != null)
                                {
                                    ce.IsMovingForward = true;
                                    ce.CurrentWaypoint = (Vector3)nextPosition;
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
                if (_enemies.Count(en => en.RouteNumber == i) == routes[i].MaxNumberEnemies) continue;

                var spawnPoint = routes[i].FirstPosition;
                var targetPoint = routes[i].NextPositionFrom(0);
                if (targetPoint == null) continue;
                
                var newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                newEnemy.RouteNumber = i;
                newEnemy.CurrentWaypoint = (Vector3)targetPoint;
                newEnemy.IsMovingForward = true;
                _enemies.Add(newEnemy);
            }
        }
    }
        #endregion
        #endregion
}