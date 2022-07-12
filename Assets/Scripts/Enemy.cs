using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace DmitryAdventure 
{
    public class Enemy : MonoBehaviour
    {
        public int RouteNumber { get; private set; }
        /// <summary>
        /// Целевая точка маршрута, к которой движется враг
        /// </summary>
        public Vector3 TargetPoint { get; private set; }

        public float MovingSpeed { get; private set; } = 1f;

        public bool IsMovingForward { get; private set; }

        /// <summary>
        /// Находится ли враг на целеой точке. 
        /// </summary>
        public bool IsReachedTargetPoint => transform.position == TargetPoint;
        
        private Rigidbody rb;
        
        private void Awake()
        {
            rb = transform.GetComponent<Rigidbody>();
            rb.mass = 30;
        }

        /// <summary>
        /// Присваивает врагу номер маршрута
        /// </summary>
        /// <param name="routeNumber">Присваевымый номер.</param>
        public void Set(int routeNumber)
        {
            RouteNumber = routeNumber;
        }

        public void Set(Vector3 targetPoint)
        {
            TargetPoint = targetPoint;
        }

        public void Set(bool isMovingForward)
        {
            IsMovingForward = isMovingForward;
        }
        
        
        public void FollowTargetWithRotation()
        {
            if (!(Vector3.Distance(transform.position, TargetPoint) > 0.5f)) return;
            
            transform.LookAt(TargetPoint);
            rb.AddRelativeForce(Vector3.forward * MovingSpeed, ForceMode.VelocityChange);
        }
        
        

        // hp = 100;
        // поле зрения 
    }
}
