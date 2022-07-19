using UnityEngine;

namespace DmitryAdventure
{
    /// <summary>
    /// Represents play value in game.
    /// </summary>
    public class GameValue
    {
        #region Сonstants, variables & properties
        /// <summary>
        ///  Name of Value.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Game value description.
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// Game value type.
        /// </summary>
        public GameValueType Type { get; }
        /// <summary>
        /// Rarity level in game.
        /// </summary>
        public RarityLevel Rarity { get; }
        /// <summary>
        /// Price in in-game currency.
        /// </summary>
        public float Cost { get; private set; }
        /// <summary>
        /// Weight in kilograms.
        /// </summary>
        public float Weight { get; private set; }
        
        #endregion

        #region Initializers and Deinitializer

        public GameValue(string name, string description, GameValueType type, RarityLevel rarity, float cost,
            float weight)
        {
            Name = name;
            Description = description;
            Type = type;
            Rarity = rarity;
            Cost = cost;
            Weight = weight;
        }
        #endregion
    }
}