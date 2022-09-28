using DmitryAdventure.Args;
using GameDevLib.Events;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "InventoryEvent", menuName = "Adventure02/Events/InventoryEvent", order = 0)]
    public class InventoryEvent : GameEvent<InventoryArgs> { }
}