using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DmitryAdventure
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties

        /// <summary>
        /// Current hit points.
        /// </summary>
        public float CurrentHp { get; protected set; }

        /// <summary>
        /// Character current movement speed.
        /// </summary>
        protected float CurrentSpeed { get;  set; }

        protected float MovementSpeedDelta => CurrentSpeed / 3;
        
        protected float RotationSpeedDelta => CurrentSpeed / 10;
        
        protected const float GravityValue = -9.81f;

        #endregion

        #region Monobehavior methods

        protected virtual void Update()
        {
            OnMovement();
            if(!IsAlive()) 
                Destroy(gameObject);
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

        /// <summary>
        /// Character movement method.
        /// </summary>
        protected abstract void OnMovement();

        /// <summary>
        /// Checks character's health limit.
        /// </summary>
        private bool IsAlive()
        {
            return CurrentHp > 0;
        }

        /// <summary>
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public virtual void OnHit(float damage)
        {
            CurrentHp -= damage;
        }

        #endregion
        #endregion
    }
}