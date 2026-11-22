using Game.Entity.Events;
using Game.Interface;
using System;
using UnityEngine;
using Zenject;
using Events;

namespace Game.Entity
{
    public class EnemyConvertToAlly : EnemySystem, ISporeable
    {
        [Inject] EntityManagers _entityManagers;
        [Inject] ProjectEvents _projectEvents;

        public bool IsSporeable()
        {
            if (!_isActive)
                return false;

            if (_base.ContextType != EntityContextType.Enemy)
                return false;

            if (_base.ResourceManager.StateType.ResourceValue != EntityStateType.Dead)
                return false;

            //if the entity costs more spores than the player has
            if (_base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer.sporeCost > _entityManagers.allyManager.SporeAmount)
                return false;

            return true;
        }

        public void Spore()
        {
            _entityManagers.allyManager.UseSpores(_base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer.sporeCost);
            _events.ResourceInput.EntityContext?.Invoke(EntityContextType.Ally);
            _events.EntityUnit.ResetEntity?.Invoke();
            _events.EntityUnit.ConvertIntoAlly?.Invoke();
            _projectEvents.Entity.AddNewAllyToManager?.Invoke(_base);
        }
    }

}
