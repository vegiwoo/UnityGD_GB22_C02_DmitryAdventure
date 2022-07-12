using System;
using UnityEngine;

namespace DmitryAdventure
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody rb;
        private float bulletSpeed = 20f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            rb.AddForce(transform.up * bulletSpeed, ForceMode.VelocityChange);
        }

        // Столкновение любым объектом
        // Столкновение с противником
    }
}

