using Base;
using Zenject;
using Game.Entity.States;
using Game.Entity.Events;
using UnityEngine;
using Sirenix.OdinInspector;
using Helper;
using System;

namespace Game.Entity
{
    public class EntityStateMachine : StatemachineBase<EntityStateBase, EntityStateData>
    {
        [Inject] protected SubClassHelper _helper;

        [Inject] protected EntityEvents _entityEvents;
        [Inject] protected EntityResourceManager _resourceManager;
        [Inject] protected EntityAllStatsConfiguration _allStatsConfiguration;
        

        [Inject]
        public void Inject(EntityStateData data)
        {
            stateData = data;
        }

        //To-Do: Probably change this, so the GD can change it
        [SerializeField, DisplayAsString(false), ReadOnly] private EntityStateType _stateType;
        #if UNITY_EDITOR
        [SerializeField, DisplayAsString(false), ReadOnly] private string _stateName;
#endif


        #region UnityMethods

        protected virtual void Awake()
        {
            _entityEvents.EntityUnit.ResetEntity += ResetEntity;
            
        }

        void Start()
        {
            GetInitialState();
            ToggleActivity(true);
        }

        protected override void Update()
        {
            base.Update();

            if (_stateType == stateData.currentType)
                return;

            _stateType = stateData.currentType;
            
#if UNITY_EDITOR
            _stateName = currentState.ToString();
#endif
            
            //_entityEvents.ResourceInput.State?.Invoke(_stateType);
            _resourceManager.StateType.ResourceValue = _stateType;
        }

        protected virtual void OnDestroy()
        {
            ToggleActivity(false);

            _entityEvents.EntityUnit.ResetEntity -= ResetEntity;
        }

        #endregion

        protected void ResetEntity()
        {
            currentState.Exit();

            GetInitialState();
        }

        protected void GetInitialState()
        {
            if (_allStatsConfiguration.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer.entityType == 0)
                throw new InvalidOperationException("EntityStateType of " + _resourceManager.EntityContext.ResourceValue + " is not set!!");

            string newState = typeof(EntityStateBase).Namespace + "."
                + _allStatsConfiguration.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer.entityType.ToString() + "_";

            newState += EntityStateType.Awake.ToString();

            currentState = _helper.GetNewInputBrain<EntityStateBase>(newState);
            #if UNITY_EDITOR
            _stateName = currentState.ToString();
            #endif
            
        }

    }

}

