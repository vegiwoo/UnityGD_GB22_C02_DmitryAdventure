using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Args
{
    /// <summary>
    /// Represents the arguments required to create a loot box.
    /// </summary>
    public class LootBoxArgs
    {
        public readonly GameValue[] GameValues;

        public LootBoxArgs(int boxMaxCapacity, IDictionary<int, RarityLevel> permissibleRarity)
        {
            var capacity = Random.Range(0, boxMaxCapacity);
            GameValues = new GameValue[capacity];

            if (capacity <= 0) return;
            
            for (var i = 0; i < GameValues.Length; i++)
            {
                var rarity = CollectionExtensions
                    .RandomValues(permissibleRarity)
                    .Take(1)
                    .First();
                
                var item = CollectionExtensions
                    .RandomValues(GameData.Instance.GameValues)
                    .Where(gv => gv.Rarity == rarity)
                    .Take(1)
                    .First();
                
                GameValues[i] = item;
            }
        }
    }
}