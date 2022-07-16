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
        [SerializeField] private Transform[] wayPoints;
        public Transform[] WayPoints => wayPoints;

        [SerializeField] private int maxNumberEnemies;
        public int MaxNumberEnemies => maxNumberEnemies;
        
        public int Count => wayPoints.Length;

        public Vector3 FirstPosition => wayPoints.First().position;
        public Vector3 LastPosition => wayPoints.Last().position;

        public Func<int, Vector3?> NextPositionFrom;
        public Func<int, Vector3?> PreviousPositionFrom;

        #endregion

        #region Monobehavior methods

        private void Start()
        {
            NextPositionFrom = i =>
            {
                var nextIndex = i + 1;
                if (nextIndex != Count - 1)
                    return WayPoints[nextIndex].position;
                return null;
            };

            PreviousPositionFrom = i =>
            {
                var previousIndex = i - 1;
                if (previousIndex >= 0)
                    return WayPoints[previousIndex].position;
                
                return null;
            };
        }

        #endregion

        #region Functionality

        #region Coroutines

        // ...

        #endregion

        #region Event handlers

        // ...

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}