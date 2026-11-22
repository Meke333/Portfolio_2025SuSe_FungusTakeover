using Events;
using System;
using Game.InputHandler;
using Zenject;

namespace Game.GameFlow.States
{
    public class GameFlowState_GameOver : GameFlowStateBase
    {

        public override void SetStateType()
        {
            stateType = GameFlowStateType.GameOver;
            base.SetStateType();
        }
        
        public override void Enter()
        {
            base.Enter();
            _events.Input.SwitchActionMapAction(InputMapType.Menu);
        }

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            switch(shouldSubscribe)
            {
                case true:
                    _events.GameFlow.ReturnToMainMenu += CloseMenu;
                    break;
                case false:
                    _events.GameFlow.ReturnToMainMenu -= CloseMenu;
                    break;
            }
        }

        private void CloseMenu()
        {
            nextState = GetState(GameFlowStateType.MainMenu);
            stage = EVENT.EXIT;
        }
    }

}


