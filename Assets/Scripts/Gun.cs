using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform bulletPoint;
        [SerializeField] private AudioSource shotSound;

        [Tooltip("Задержка выстрела")]
        [SerializeField, Range(0.5f,2f)] private float shotDelay;
        private float shotDelayTimer;

        private void Start()
        {
            shotDelayTimer = 0f;
            shotDelay = 0.5f;
        }


        private void Update()
        {
            // TODO: Вынести в InputController
            if (Input.GetMouseButton(0) && shotDelayTimer <= 0f)
                Fire();
            
            if (shotDelayTimer > 0)
                shotDelayTimer -= Time.deltaTime;
        }
        
        /// <summary>
        /// Осуществляет выстрел.
        /// </summary>
        /// <remarks>
        /// - генерирует пулю
        /// - воспроизводит звук выстрела
        /// </remarks>>
        private void Fire()
        {
            // TODO: Реализовать как пул объетов
            var newBullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
            shotSound.pitch = Random.Range(0.8f, 1.2f); 
            shotSound.Play();
            shotDelayTimer = shotDelay;
        }
    }
}

