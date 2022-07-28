using System.Collections.Generic;
using JetBrains.Annotations;

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
        public List<(GameValue value, int count)> GameValues { get; }

        public UIManagerArgs(float maxHp, float currentHp, int goalToKillEnemies, int currentKillEnemiesCount, [CanBeNull] List<(GameValue value, int count)> gameValues)
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