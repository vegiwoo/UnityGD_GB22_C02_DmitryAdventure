using System.Collections.Generic;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Static data for game.
    /// </summary>
    /// <remarks>Singleton (No Thread Safe)</remarks>>
    public sealed class GameData
    {
        #region Сonstants, variables & properties

        private static GameData _instance;  
        public static GameData Instance 
        {  
            get { return _instance ??= new GameData(); }  
        }

        public static readonly float Gravity = Physics.gravity.y;
        
        public readonly Dictionary<GUI, GameValue> GameValues = new()
        {
            {
                new GUI(), 
                new GameValue(MedicineKey, "Easily restore lost health", 
                    GameValueType.Resource, RarityLevel.Ordinary, 0, 0.20f, hpBoostRate:25.0f, valueKey: MedicineKey)
            },
            {
                new GUI(), 
                new GameValue(KeysKey, "Every rusty key opens something", 
                    GameValueType.Item, RarityLevel.Rare, 0, 0.15f, hpBoostRate:0, valueKey: KeysKey)
            },
            {
                new GUI(), 
                new GameValue(MineKey, "Enemies on this mine fly up like crazy frogs", 
                    GameValueType.Item, RarityLevel.Rare, 50, 1.50f, hpBoostRate:-100.0f, valueKey: MineKey)
            }
        };
        
        // Tags
        public const string PlayerTag = "Player";
        public const string EnemyTag = "Enemy";
        
        // Keys 
        public const string EnemiesKey = "enemies";
        public const string MedicineKey = "medicines";
        public const string KeysKey = "keys";
        public const string MineKey = "mines";

        // Messages 
        public const string WinMessage = "You win :)";
        public const string LoseMessage = "You lose :(";
        
        // Screen
        public readonly int ScreenCenterX = Screen.width / 2; 

        #endregion

        #region Initializers and Deinitializer

        private GameData() {}
        #endregion
    }
}