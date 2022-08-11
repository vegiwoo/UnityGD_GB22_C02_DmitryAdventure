using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    public enum PositionsRouteType
    {
        Previous, Current, Next
    }
    
    /// <summary>
    /// Represents enemy's route.
    /// </summary>
    public class EnemyRoute : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [field: SerializeField, Tooltip("Enemy route number")] 
        public int RouteNumber { get; set; }

        [SerializeField, Tooltip("Looped route")]
        private bool isCircularRoute;
        
        [field:SerializeField, Tooltip("Character waiting time at checkpoints and time to spawn a new enemy, sec")]
        public float WaitTime { get; set; }
        
        [field: SerializeField, ReadonlyField, Tooltip("Enemies spawn timer at starting point of route")]
        public float SpawnTimer { get; set; }

        [SerializeField] private WayPoint[] wayPoints;
        
        [field:SerializeField, Tooltip("Maximum number of enemies on the route")] 
        public int MaxNumberEnemies { get; set; }

        private const int RouteStartIndex = 0;
        private int RouteEndIndex => wayPoints.Length - 1;
        
        public Vector3 FirstWaypoint => wayPoints[RouteStartIndex].point.position;

        [SerializeField, Tooltip("Attention trigger size increase factor on checkpoints")] 
        public float attentionIncreaseFactor;

        private Coroutine _countdownToSpawnNewEnemyCoroutine;
        
        
        /// <summary>
        /// Returns requested route positions.
        /// </summary>
        /// <param name="type">Requested item type.</param>
        /// <param name="i">Index of position relative to which result is requested.</param>
        /// <remarks>
        /// When receiving positions of type 'First' or 'Last', index is not specified.
        /// </remarks>>
        public Vector3 this[PositionsRouteType type, int i]
        {
            get
            {
                return type switch
                {
                    PositionsRouteType.Previous => wayPoints[i - 1].point.position,
                    PositionsRouteType.Current => wayPoints[i].point.position,
                    PositionsRouteType.Next => wayPoints[i + 1].point.position,
                    _ => Vector3.zero
                };
            }
        }

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            attentionIncreaseFactor = 1.5f;
            SpawnTimer = 0;
        }

        #endregion

        #region Functionality
        
        /// <summary>
        /// Change destination waypoint and direction of moving.
        /// </summary>
        /// <param name="isMovingForward">Current direction of moving.</param>
        /// <param name="oldIndex">Old index of waypoint .</param>
        /// <returns>Calculation result.</returns>
        public (bool isMoveForward, int index, bool isControlPoint, bool isAttentionIsIncreased) ChangeWaypoint(bool isMovingForward, in int oldIndex)
        {
            const int step = 1;
            var isCurrentControlPoint = wayPoints[oldIndex].isCheckPoint;
            var isAttentionIsIncreased = wayPoints[oldIndex].isIncreaseAttention;

            return isMovingForward switch
            {
                true => oldIndex != RouteEndIndex
                    ? (true, oldIndex + step, isCurrentControlPoint, isAttentionIsIncreased)
                    : (false, isCircularRoute ? RouteStartIndex : oldIndex - step, isCurrentControlPoint, isAttentionIsIncreased),
                false => oldIndex != RouteStartIndex
                    ? (false, oldIndex - step, isCurrentControlPoint, isAttentionIsIncreased)
                    : (true, oldIndex + step, isCurrentControlPoint, isAttentionIsIncreased)
            };
        }

        public void OnCreateEnemiesEvent((int routeNumber, int createCount) item)
        {
            if (item.routeNumber != RouteNumber) return;
            _countdownToSpawnNewEnemyCoroutine = StartCoroutine(CountdownToSpawnNewEnemyCoroutine());
        }

        /// <summary>
        /// Event handler for killed enemies on route.
        /// </summary>
        /// <param name="killed">Dictionary of killed enemies (key - route number, value - number of killed).</param>
        public void OnRemoveKilledEnemies(Dictionary<int, int> killed)
        {
            var killedOnCurrentRoute = killed.Select(el => el.Key == RouteNumber).First();
            if (!killedOnCurrentRoute) return;
            
            _countdownToSpawnNewEnemyCoroutine = StartCoroutine(CountdownToSpawnNewEnemyCoroutine());
        }
        
        private IEnumerator CountdownToSpawnNewEnemyCoroutine()
        {
            SpawnTimer = WaitTime;
            
            while (SpawnTimer > 0)
            {
                SpawnTimer -= Time.deltaTime;
                yield return null;
            }

            SpawnTimer = 0;
            _countdownToSpawnNewEnemyCoroutine = null;
        }

        #endregion
    }
}
