using UnityEngine;
using Random = UnityEngine.Random;

namespace DmitryAdventure
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] 
        private Bullet bulletPrefab;
        
        [SerializeField] 
        private AudioSource shotSound;

        [SerializeField, Range(5,10)] 
        private float lowFiringRange;
        /// <summary>
        /// Lower firing range.
        /// </summary>
        public float LowFiringRange => lowFiringRange;
        
        [SerializeField, Range(20, 100)] 
        private float upFiringRange;
        /// <summary>
        /// Upper firing range.
        /// </summary>
        public float UpFiringRange => upFiringRange;

        [SerializeField, Tooltip("Shooting angle in degrees"), Range(8,85)]
        private float angeleInDeg;
        
        [SerializeField, Tooltip("Bullet spawn point")] 
        private Transform barrel;
        public Transform Barrel => barrel;
        
        [SerializeField, Range(0.5f,2f), Tooltip("Delay between shots")]
        private float shotDelay;
        private float _shotDelayTimer;

        private static readonly float Gravity = Physics.gravity.y;
        
        private void Start()
        {
            lowFiringRange = 5;
            upFiringRange = 50;
            angeleInDeg = 10;
            _shotDelayTimer = 0f;
            shotDelay = 0.5f;
        }
        
        private void Update()
        {
            transform.localEulerAngles = new Vector3(-angeleInDeg, 0, 0);
            
            if (_shotDelayTimer > 0)
                _shotDelayTimer -= Time.deltaTime;
        }
        
        /// <summary>
        /// Осуществляет выстрел.
        /// </summary>
        /// <remarks>
        /// - генерирует пулю
        /// - воспроизводит звук выстрела
        /// - запускает таймер задержки
        /// </remarks>>
        public void Fire(Vector3 targetPosition)
        {
            if(_shotDelayTimer > 0) return;

            // TODO: Реализовать как пул объетов !
            var newBullet = Instantiate(bulletPrefab, barrel.position,barrel.rotation);
            newBullet.PointOfShoot = barrel;
            newBullet.TargetPosition = targetPosition;
            newBullet.BulletVelocity = Shoot(targetPosition);
            
            shotSound.pitch = Random.Range(0.8f, 1.2f); 
            shotSound.Play();
            
            _shotDelayTimer = shotDelay;
        }
        
        
        /// <summary>
        /// Рассчитывает скорость полета пули по баллистической траектории;
        /// </summary>
        /// <param name="targetPosition">Позиция цели для попадания</param>
        /// <returns></returns>
        private float Shoot(Vector3 targetPosition)
        {
            var fromShooterToTarget = targetPosition - transform.position;
            var fromShooterToTargetXZ = new Vector3(fromShooterToTarget.x, 0, fromShooterToTarget.z);
            transform.rotation = Quaternion.LookRotation(fromShooterToTargetXZ, Vector3.up);
            
            var x = fromShooterToTargetXZ.magnitude;
            var y = fromShooterToTarget.y;

            var angleInRadians = angeleInDeg * Mathf.PI / 180;
            var v2 = Gravity * Mathf.Pow(x, 2) /
                     (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
            return Mathf.Sqrt(Mathf.Abs(v2));
        }
    }
}

