using Events;
using Game.InputHandler;
using System;
using UnityEngine.Device;
using Zenject;

namespace Game.GameFlow.States
{
    public class GameFlowState_MainMenu : GameFlowStateBase
    {
        public override void SetStateType()
        {
            stateType = GameFlowStateType.MainMenu;
            base.SetStateType();
        }

        public override void Enter()
        {
            base.Enter();
            _events.Input.SwitchActionMapAction(InputMapType.Menu);
        }

        public override void Exit()
        {
            base.Exit();
            //_projectEvents.GameFlow.StartGame?.Invoke();
        }

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.GameFlow.StartGame += StartGame;
                    _events.GameFlow.CloseMenu += CloseMenu;
                    break;
                case false:
                    _events.GameFlow.StartGame -= StartGame;
                    _events.GameFlow.CloseMenu -= CloseMenu;
                    break;
            }
        }

        private void CloseMenu()
        {
            Application.Quit();
        }

        private void StartGame()
        {
            nextState = GetState(GameFlowStateType.Gameplay);
            stage = EVENT.EXIT;
        }
    }

}


