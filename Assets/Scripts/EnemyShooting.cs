using UnityEngine;
using DmitryAdventure.Characters;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents enemies shooting functionality.
    /// </summary>
    public class EnemyShooting : CharacterShooting
    {
        #region Functionality
        protected override void OnTakeAim()
        {
            var enemy = Character as Enemy;

            if (enemy == null || enemy.CurrentEnemyState != EnemyState.Attack) return;

            var eObject = enemy.gameObject;
            var ePosition = eObject.transform.position;
            var eForward = eObject.transform.forward;

            var hit = AimingRaycast(ePosition, eForward, RaycastLayerType.Interaction,
                enemy.enemyStats.AttentionRadius);
            if(hit.collider == null) return;
            
            var bounds = hit.collider.bounds;
            AimPoint = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
            ShootWeapon();
        }
        #endregion
    }
}