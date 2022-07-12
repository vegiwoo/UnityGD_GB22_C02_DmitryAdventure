using System;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Маршрут перемещения врага.
    /// </summary>
    [Serializable]
    public class Route
    {
        /// <summary>
        /// Точки перемещения врага на маршруте.
        /// </summary>
        public Vector3[] wayPoints;
    }
}