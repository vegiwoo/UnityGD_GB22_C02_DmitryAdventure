using System;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Enemy movement route.
    /// </summary>
    [Serializable]
    public class Route
    {
        /// <summary>
        /// Number of enemies on the route
        /// </summary>
        public int numberEnemies = 1;
        
        /// <summary>
        /// Enemy movement points on route.
        /// </summary>
        public Vector3[] wayPoints;
    }
}