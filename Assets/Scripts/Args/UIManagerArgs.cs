using System.Collections.Generic;
using DmitryAdventure.Props;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Args
{
    /// <summary>
    /// Represents arguments passed to UIManager.
    /// </summary>
    public class UIManagerArgs
    {
        #region Сonstants, variables & properties

        public float MaxHp { get; }
        public float CurrentHp { get; }
        public int GoalToKillEnemies { get; }
        public int CurrentKillEnemiesCount { get; }
        
        public bool IsHeroDie { get; }

        #endregion

        #region Initializers and Deinitializer

        [CanBeNull] 
        public IEnumerable<(string,int)> GameValues { get; }

        public UIManagerArgs(float maxHp, float currentHp, int goalToKillEnemies, int currentKillEnemiesCount, [CanBeNull] IEnumerable<(string,int)> gameValues)
        {
            MaxHp = maxHp;
            CurrentHp = currentHp;
            GoalToKillEnemies = goalToKillEnemies;
            CurrentKillEnemiesCount = currentKillEnemiesCount;
            IsHeroDie = CurrentHp <= 0;
            GameValues = gameValues;
        }
        #endregion
    }
}