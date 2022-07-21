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
        
        public readonly Dictionary<GUI, GameValue> GameValues = new Dictionary<GUI, GameValue>
        {
            {
                new GUI(), 
                new GameValue(MedicineLabelText, "Easily restore lost health", 
                    GameValueType.Resource, RarityLevel.Ordinary, 0, 0.20f)
            },
            {
                new GUI(), 
                new GameValue(KeyLabelText, "Every rusty key opens something", 
                    GameValueType.Item, RarityLevel.Rare, 0, 0.15f)
            },
            {
                new GUI(), 
                new GameValue(MineLabelText, "Enemies on this mine fly up like crazy frogs", 
                    GameValueType.Item, RarityLevel.Rare, 50, 1.50f)
            }
        };
        
        // Tags
        public const string PlayerTag = "Player";
        public const string EnemyTag = "Enemy";

        // Layer masks
        public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask($"Player");

        public const string EnemiesLabelText = "ENEMIES";
        public const string MedicineLabelText = "MEDICINES";
        public const string KeyLabelText = "KEYS";
        public const string MineLabelText = "MINES";

        public const string WinMessage = "You win :)";
        public const string LoseMessage = "You lose :(";

        #endregion

        #region Initializers and Deinitializer

        private GameData() {}
        #endregion

        #region Functionality

        #region Event handlers
        // ...
        #endregion

        #region Other methods
    //..

        #endregion
        #endregion
    }
}