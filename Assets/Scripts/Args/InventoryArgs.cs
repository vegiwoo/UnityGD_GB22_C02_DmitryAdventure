using System;
using System.Collections.Generic;
using DmitryAdventure;

// ReSharper disable once CheckNamespace
namespace GameDevLib.Args
{
    public class InventoryArgs : EventArgs
    {
        public List<GameValueItem> GameValues { get; }

        public InventoryArgs(List<GameValueItem> items)
        {
            GameValues = items;
        }
    }
}