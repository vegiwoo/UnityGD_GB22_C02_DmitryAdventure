using UnityEngine;
using GameDevLib.Args;
using GameDevLib.Interfaces;
using GameDevLib.Managers;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Organizes game.
    /// </summary>
    public class AdventureGameManager : GameManager
    {
        #region Fileds
        
        private int _currentKillEnemiesCount;
        
        #endregion
        
        #region Properties

        [Header("Game")] 
        [SerializeField, Range(1,20)] private int goalToKillEnemiesCount = 5;
        
        #endregion
     
        #region Functionality
        public override void OnEventRaised(ISubject<UnitArgs> subject, UnitArgs args)
        {
            // TODO: Определить условавия победы, поражения и конца игры
        }
        
        #endregion
    }
}


