using System;
using System.Linq;
using UnityEngine;

namespace DmitryAdventure
{
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

        public Vector3 StartPoint => wayPoints.First().position;
        private Vector3 EndPoint => wayPoints.Last().position;
      
        /// <summary>
        /// Returns requested route positions.
        /// </summary>
        /// <param name="type">Requested item type.</param>
        /// <param name="i">Index of position relative to which result is requested.</param>
        /// <remarks>
        /// When receiving positions of type 'First' or 'Last', index is not specified.
        /// </remarks>>
        public Vector3 this[PositionType type, int i]
        {
            get
            {
                return type switch
                {
                    PositionType.Previous => wayPoints[i - 1].position,
                    PositionType.Current => wayPoints[i].position,
                    PositionType.Next => wayPoints[i + 1].position,
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
        /// <param name="currentPoint">Current waypoint.</param>
        /// <returns>Calculation result.</returns>
        public (bool isMovingForward, Vector3 currentWayPoint) ChangeWaypoint(bool isMovingForward, Vector3 currentPoint)
        {
            var itp = Array.IndexOf(wayPoints, currentPoint);

            return isMovingForward switch
            {
                true => currentPoint != EndPoint
                    ? (true, this[PositionType.Next, itp])
                    : (false, this[PositionType.Previous, itp]),
                false => currentPoint != StartPoint
                    ? (false, this[PositionType.Previous, itp])
                    : (true, this[PositionType.Next, itp])
            };
        }

        #endregion

        #endregion
    }
}

public enum PositionType
{
    Previous, Current, Next
}