using System;
using UnityEngine;

namespace Game.Entity
{
    /// <summary>
    /// These events fire to the AllyManager
    /// this doesnt work lol
    /// </summary>
    public class AllyEvents
    {
        public Action<EntityBase> AddNewAllyToManager;
        public Action<EntityBase> RemoveNewAllyInManager;
        public Action<EntityBase> PlayerDied;

        public Action<float> GivePlayerHealth;

        public Action<Transform> TeleportToPoint;

        public Action<int> UseSpores;
        public Action<int> GetSpores;

        public Action CallFormation;
    }
}

