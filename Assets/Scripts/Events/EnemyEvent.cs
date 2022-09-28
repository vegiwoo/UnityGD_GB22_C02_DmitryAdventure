using DmitryAdventure.Args;
using GameDevLib.Events;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "EnemyEvent", menuName = "Adventure02/Events/EnemyEvent", order = 1)]
    public class EnemyEvent : GameEvent<EnemyArgs> { }
}