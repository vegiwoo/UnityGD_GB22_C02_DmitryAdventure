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
        public static GameData Instance {  
            get { return _instance ??= new GameData(); }  
        }

        public readonly Dictionary<GUI, GameValue> GameValues = new Dictionary<GUI, GameValue>
        {
            {
                new GUI(), 
                new GameValue("Medicine", "Easily restore lost health", 
                    GameValueType.Resource, RarityLevel.Ordinary, 0, 0.20f)
            },
            {
                new GUI(), 
                new GameValue("Key", "Every rusty key opens something", 
                    GameValueType.Item, RarityLevel.Rare, 0, 0.15f)
            },
            {
                new GUI(), 
                new GameValue("Mine", "Enemies on this mine fly up like crazy frogs", 
                    GameValueType.Item, RarityLevel.Rare, 50, 1.50f)
            }
        };

        /// <summary>
        /// Hero's maximum inventory capacity
        /// </summary>
        public float MaximumInventoryCapacity = 3.0f;

        #endregion

        #region Initializers and Deinitializer
        GameData() {}
        #endregion

        #region Functionality

        #region Event handlers

        // ...

        #endregion

        #region Other methods

        // ...

        #endregion

        #endregion
    }
}