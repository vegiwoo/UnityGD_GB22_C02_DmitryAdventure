using GameDevLib.Args;
using GameDevLib.Events;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "InventoryEvent", menuName = "Adventure02/InventoryEvent", order = 0)]
    public class InventoryEvent : GameEvent<InventoryArgs> { }
}