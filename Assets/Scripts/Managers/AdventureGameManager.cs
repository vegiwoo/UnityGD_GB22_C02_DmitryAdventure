using System;
using DmitryAdventure.Args;
using Events;
using UnityEngine;
using GameDevLib.Args;
using GameDevLib.Interfaces;
using GameDevLib.Managers;
using UnityEditor;
using static UnityEngine.Debug;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Managers
{
    /// <summary>
    /// Organizes game.
    /// </summary>
    public class AdventureGameManager : GameManager, GameDevLib.Interfaces.IObserver<EnemyArgs>
    {

        #region Properties
        [field: SerializeField] private EnemyEvent EnemyEvent { get; set; }

        #endregion

        #region MonoBehaviour methods

        protected override void OnEnable()
        {
            base.OnEnable();
            EnemyEvent.Attach(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EnemyEvent.Detach(this);
        }

        #endregion
        
        #region Functionality
        
        // Event handler for PlayerEvent
        public override void OnEventRaised(ISubject<UnitArgs> subject, UnitArgs args)
        {
            var lose = args.Hp.current == 0;
            if (lose)
            {
                Log($"You lost because you died :(");
                EditorApplication.isPaused = true;
            }
        }
        
        // Event handler for EnemyEvent
        public void OnEventRaised(ISubject<EnemyArgs> subject, EnemyArgs args)
        {
            var win = args.UnitsRemovedSum >= GameStats.GameHighScore;
            if (win)
            {
                Log($"You win because you destroyed required number of enemies :)");
                EditorApplication.isPaused = true;
            }
        }
        
        #endregion
    }
}


