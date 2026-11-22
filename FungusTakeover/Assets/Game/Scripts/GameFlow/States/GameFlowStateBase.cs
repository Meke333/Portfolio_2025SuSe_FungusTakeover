using Base;
using Events;
using Helper;
using Zenject;
using System;
using UnityEngine;
namespace Game.GameFlow.States
{
    public abstract class GameFlowStateBase : StateBase
    {
        [Inject] protected SubClassHelper _helper;
        [Inject] protected ProjectEvents _events;
        [Inject] protected GameFlowStateData _stateData;


        public GameFlowStateType stateType;

        public override void SetStateType()
        {
            _stateData.currentType = stateType;
        }

        public override void Enter()
        {
            SetStateType();

            base.Enter();
            ToggleEvents(true);

            _events.GameFlow.GameFlowChange?.Invoke(stateType);
        }

        public override void Exit()
        {
            base.Exit();
            ToggleEvents(false);
        }

        protected GameFlowStateBase GetState(GameFlowStateType type)
        {
            GameFlowStateBase result = null;

            if (type == 0)
                throw new InvalidOperationException("" + type.GetType() + " of " + type + " is not set!!");

            result = type switch
            {
                GameFlowStateType.MainMenu => _helper.GetNewInputBrain<GameFlowState_MainMenu>(),
                GameFlowStateType.Gameplay => _helper.GetNewInputBrain<GameFlowState_Gameplay>(),
                GameFlowStateType.PauseMenu => _helper.GetNewInputBrain<GameFlowState_PauseMenu>(),
                GameFlowStateType.GameOver => _helper.GetNewInputBrain<GameFlowState_GameOver>(),
                GameFlowStateType.GameWon => _helper.GetNewInputBrain<GameFlowState_GameWon>()
            };

            return result;
        }

        protected virtual void ToggleEvents(bool shouldSubscribe)
        {
            
        }
    }

}


