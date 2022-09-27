// ReSharper disable once CheckNamespace
namespace DmitryAdventure
{
    /// <summary>
    /// Represents game values in collection.
    /// </summary>
    public class GameValueItem
    {
        public string Key { get; }
        public GameValue Value { get; }
        /// <summary>
        /// Number of game values in collection.
        /// </summary>
        public int Count { get; set; }

        public GameValueItem(string key, GameValue value, int count)
        {
            Key = key;
            Value = value;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Key} {Value.ValueKey}, {Value.Name}, {Value.Rarity}, {Value.HpBoostRate}:  {Count}";
        }
    }
}