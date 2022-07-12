using UnityEngine;

// hp = 100;
// поле зрения 

namespace DmitryAdventure 
{
    /// <summary>
    /// Противник.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Номер маршрута врага
        /// </summary>
        public int RouteNumber { get;  set; }
        /// <summary>
        /// Текущая точка маршрута, к которой движется враг
        /// </summary>
        public Vector3 TargetPoint { get; set; }
        /// <summary>
        /// Скорость дивжения.
        /// </summary>
        public float MovingSpeed { get; set; } = 1f;
        /// <summary>
        /// Флаг движения врага вперед по маршруту.
        /// </summary>
        public bool IsMovingForward { get;  set; }

        private Rigidbody rb;
        
        private void Awake()
        {
            rb = transform.GetComponent<Rigidbody>();
            rb.mass = 30;
        }
    }
}
