using System;
using UnityEngine;

namespace DmitryAdventure
{
    public enum PositionsRouteType
    {
        Previous, Next
    }
    
    /// <summary>
    /// Represents enemy's route.
    /// </summary>
    public class EnemyRoute : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] private int number;
        public int Number => number;

        [SerializeField] private Transform[] wayPoints;

        [SerializeField] private int maxNumberEnemies;
        public int MaxNumberEnemies => maxNumberEnemies;

        private int startIndex = 0;
        private int EndIndex => wayPoints.Length - 1;
        
        public Vector3 StartPoint => wayPoints[startIndex].position;
        
        private Vector3 EndPoint => wayPoints[EndIndex].position;
      
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
                    PositionsRouteType.Previous => wayPoints[i - 1].position,
                    PositionsRouteType.Next => wayPoints[i + 1].position,
                    _ => Vector3.zero
                };
            }
        }

        #endregion

        #region Monobehavior methods
        // ...
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
        /// Change destination waypoint and direction of moving.
        /// </summary>
        /// <param name="isMovingForward">Current direction of moving.</param>
        /// <param name="oldWayPoint">Old waypoint.</param>
        /// <returns>Calculation result.</returns>
        public (bool isMovingForward, Vector3 currentWayPoint) ChangeWaypoint(bool isMovingForward, Vector3 oldWayPoint)
        {
            var i = Array.FindIndex(wayPoints,el =>
            {
                Vector3 position;
                return (position = el.position).x.Equals(oldWayPoint.x) && position.z.Equals(oldWayPoint.z);
            });
            
            return isMovingForward switch
            {
                true => oldWayPoint != EndPoint
                    ? (true, this[PositionsRouteType.Next,i])
                    : (false, this[PositionsRouteType.Previous, i]),
                false => oldWayPoint != StartPoint
                    ? (false, this[PositionsRouteType.Previous, i])
                    : (true, this[PositionsRouteType.Next,i])
            };
        }

        #endregion
        #endregion
    }
}
