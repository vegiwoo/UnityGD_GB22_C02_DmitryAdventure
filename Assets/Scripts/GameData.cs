using System.Collections.Generic;
using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Static data for game.
    /// </summary>
    internal class GameData
    {
        #region Сonstants, variables & properties

        public static Dictionary<GUI, GameValue> GameValues = new Dictionary<GUI, GameValue>
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

        #endregion

        #region Initializers and Deinitializer

        //...

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