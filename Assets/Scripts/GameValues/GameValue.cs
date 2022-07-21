// ReSharper disable once CheckNamespace
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
        private string Description { get; }
        /// <summary>
        /// Game value type.
        /// </summary>
        private GameValueType Type { get; }
        /// <summary>
        /// Rarity level in game.
        /// </summary>
        public RarityLevel Rarity { get; }
        /// <summary>
        /// Price in in-game currency.
        /// </summary>
        private float Cost { get;}
        /// <summary>
        /// Weight in kilograms. 
        /// </summary>
        private float Weight { get; }

        /// <summary>
        /// Indicator of increase and decrease in character's HP.
        /// </summary>
        public float HpBoostRate { get; }

        #endregion

        #region Initializers and Deinitializer
        public GameValue(string name, string description, GameValueType type, RarityLevel rarity, float cost,
            float weight, float hpBoostRate)
        {
            Name = name;
            Description = description;
            Type = type;
            Rarity = rarity;
            Cost = cost;
            Weight = weight;
            HpBoostRate = hpBoostRate;
        }
        #endregion
    }
}