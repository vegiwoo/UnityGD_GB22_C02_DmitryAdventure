using System;

// ReSharper disable once CheckNamespace
namespace DmitryAdventure.Args
{
    public class EnemyArgs : EventArgs
    {
        public int RouteNumber { get; }
        public int NumberUnitsCreated { get; }
        public int NumberUnitsRemoved { get; }
        
        public int UnitsRemovedSum { get; }

        public EnemyArgs(int routeNumber, int numberUnitsCreated, int numberUnitsRemoved, int unitsRemovedSum)
        {
            RouteNumber = routeNumber;
            NumberUnitsCreated = numberUnitsCreated;
            NumberUnitsRemoved = numberUnitsRemoved;
            UnitsRemovedSum = unitsRemovedSum;
        }
    }
}