using Base;
using Game.Entity.Events;
using Helper;
using System;
using UnityEngine;
using Zenject;

namespace Game.Entity.States
{
    public abstract class EntityStateBase : StateBase
    {
        [Inject] private SubClassHelper _helper;

        [Inject] protected EntityStateData _stateData;
        [Inject] protected EntityEvents _events;
        [Inject] protected EntityAllStatsConfiguration _allStatsConfig;
        [Inject] protected EntityResourceManager _resourceManager;

        public EntityStateType stateType;

        //To-Do: Make this a global settings so the animator person can change it
        protected static string _animatorAttackBool = "isAttacking";


        public override void SetStateType()
        {
            _stateData.currentType = stateType;
        }

        public override void Enter()
        {
            SetStateType();
            _events.State.StateChanged?.Invoke(stateType);
            base.Enter();
        }
        
        //Use this to call the other States within the same Entity
        protected EntityStateBase GetState(EntityStateType type)
        {
            EntityStateBase result = null;
            EntityContextType context = _resourceManager.EntityContext.ResourceValue;

            if (context == EntityContextType.Enemy && type == EntityStateType.Formation)
            throw new InvalidOperationException("Enemies don't have Formation as a state!!!");

            /*if (type == 0)
                throw new InvalidOperationException("EntityStateType of " + context + " is not set!!");
            */

            string newStateName = typeof(EntityStateBase).Namespace + "."
                + _allStatsConfig.GetStats(context).EntityStatContainer.entityType.ToString()
                + "_" + type.ToString();
            //Debug.Log(typeof(EntityStateBase).Namespace + ";" + _allStatsConfig.GetStats(context).EntityStatContainer.entityType.ToString() + "; " + type.ToString());

            result = _helper.GetNewInputBrain<EntityStateBase>(newStateName);

            if (result == null)
            {
                throw new InvalidOperationException("EntityStateBase is null!!!");
            }

            //Debug.Log("GetState: " + result.ToString());

            return result;
        }

        protected EntityStateBase GetState(EntityStateType type, EntityContextType context)
        {
            if (context == EntityContextType.Enemy && type == EntityStateType.Formation)
            throw new InvalidOperationException("Enemies don't have Formation as a state!!!");

            EntityStateBase result = null;

            if (type == 0)
                throw new InvalidOperationException("EntityStateType of " + context + " is not set!!");

            string newStateName = typeof(EntityStateBase).Namespace + "."
                + _allStatsConfig.GetStats(context).EntityStatContainer.entityType.ToString() 
                + type.ToString();

            result = _helper.GetNewInputBrain<EntityStateBase>(newStateName);

            /*
            switch (context)
            {
                case EntityContextType.Player:
                    result = type switch
                    {
                        EntityStateType.Idle => _diContainer.Resolve<Game.Entity.States.Player.Idle>(),
                        EntityStateType.Attack => _diContainer.Resolve<Game.Entity.States.Player.Attack>(),
                        EntityStateType.Stun => _diContainer.Resolve<Game.Entity.States.Player.Stun>(),
                        EntityStateType.Death => _diContainer.Resolve<Game.Entity.States.Player.Death>(),
                    };
                    break;
                case EntityContextType.Ally:
                    result = type switch
                    {
                        EntityStateType.Idle => _diContainer.Resolve<Game.Entity.States.Ally.Idle>(),
                        EntityStateType.Attack => _diContainer.Resolve<Game.Entity.States.Ally.Attack>(),
                        EntityStateType.Stun => _diContainer.Resolve<Game.Entity.States.Ally.Stun>(),
                        EntityStateType.Death => _diContainer.Resolve<Game.Entity.States.Ally.Death>(),
                    };
                    break;
                case EntityContextType.Enemy:
                    result = type switch
                    {
                        EntityStateType.Idle => _diContainer.Resolve<Game.Entity.States.Enemy.Idle>(),
                        EntityStateType.Attack => _diContainer.Resolve<Game.Entity.States.Enemy.Attack>(),
                        EntityStateType.Stun => _diContainer.Resolve<Game.Entity.States.Enemy.Stun>(),
                        EntityStateType.Death => _diContainer.Resolve<Game.Entity.States.Enemy.Death>(),
                    };
                    break;
            }
            */

            if (result == null)
            {
                throw new InvalidOperationException("EntityStateBase is null!!!");
            }

            return result;
        }

        public abstract void ToggleEvents(bool shouldSubscribe);

        public void SwitchState(EntityStateType entityStateType)
        {
            nextState = GetState(entityStateType);
            stage = EVENT.EXIT;
        }
        protected bool CheckIfInRange()
        {
            if (_resourceManager.Target.ResourceValue == null)
                return false;

            float distance = Vector3.Distance(_resourceManager.EntityContext.transform.position, _resourceManager.Target.ResourceValue.position);
            if (distance <= _allStatsConfig.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer.attackRange)
                return true;
            return false;
        }
    }
}

