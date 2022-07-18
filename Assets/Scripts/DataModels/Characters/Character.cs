using System;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Essence of playable or non-playable character.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        #region Сonstants, variables & properties

        /// <summary>
        /// Character type.
        /// </summary>
        [SerializeField] public CharacterType characterType;
        
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

        // Events 
        public delegate void CharacterHandler(CharacterEventArgs e);
        public event CharacterHandler CharacterNotify;

        #endregion

        #region Monobehavior methods

        protected virtual void Update()
        {
            OnMovement();

            if (CurrentHp <= 0)
            {
                Destroy(gameObject);
            }
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
        /// Character Damage Method.
        /// </summary>
        /// <param name="damage">Damage value.</param>
        /// <returns></returns>
        public abstract void OnHit(float damage);
        
        #endregion
        #endregion

        protected void OnCharacterNotify(CharacterEventArgs e)
        {
            CharacterNotify?.Invoke(e);
        }
    }
}