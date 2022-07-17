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
        
        //PlayerStats _characterStats;
        
        /// <summary>
        /// Current hit points.
        /// </summary>
        public float CurrentHp { get; protected set; }

        /// <summary>
        /// Character current movement speed.
        /// </summary>
        protected float CurrentSpeed { get;  set; }

        protected float MovementSpeedDelta => CurrentSpeed / 25;
        
        protected const float GravityValue = -9.81f;

        #endregion

        #region Monobehavior methods

        protected virtual void Update()
        {
            OnMovement();
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
        protected bool IsAlive()
        {
            return CurrentHp > 0;
        }

        /// <summary>
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public virtual bool OnHit(float damage)
        {
            CurrentHp -= damage;
            return CurrentHp <= 0;   
        }

        #endregion
        #endregion
    }
}