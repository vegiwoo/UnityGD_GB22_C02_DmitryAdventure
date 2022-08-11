// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents game values in collection.
    /// </summary>
    public class GameValueItem
    {
        /// <summary>
        /// Game value 
        /// </summary>
        public GameValue Value { get; }
        /// <summary>
        /// Number of game values in collection.
        /// </summary>
        public int Count { get; set; }

        public GameValueItem(GameValue value, int count)
        {
            Value = value;
            Count = count;
        }
    }
}