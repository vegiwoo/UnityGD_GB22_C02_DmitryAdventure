using System;
using UnityEngine;
using DmitryAdventure.Characters;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.WeaponsAndAmmunition
{
    /// <summary>
    /// Represents ammunition
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Ammunition : MonoBehaviour
    {
        #region Сonstants, variables & properties

        [SerializeField] 
        protected GameObject effectPrefab;
        protected Rigidbody AmmunitionRigidbody;


        [field: SerializeField, Tooltip("Damage dealt by ammunition"), Range(10f,100f)] 
        protected int Damage { get; set; }
        
        #endregion

        #region Monobehavior methods
        
        protected virtual void Start()
        {
            AmmunitionRigidbody = GetComponent<Rigidbody>();
        }

        #endregion
    }
}