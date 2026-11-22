using Game.InputHandler;
using System;

namespace Game.GameFlow.States
{
    public class GameFlowState_Gameplay : GameFlowStateBase
    {
        public override void SetStateType()
        {
            stateType = GameFlowStateType.Gameplay;
            base.SetStateType();
        }

        public override void Enter()
        {
            base.Enter();
            _events.Input.SwitchActionMapAction(InputMapType.Gameplay);
        }

        protected override void ToggleEvents(bool shouldSubscribe)
        {
          switch (shouldSubscribe)
          {
            case true:
                _events.Input.Gameplay.MenuOpenAction += OpenMenu;
                _events.GameFlow.OpenMenu += OpenMenu;
                _events.GameFlow.GameOver += GameOver;
                _events.GameFlow.GameWin += GameWin;
                break;
            case false:
                _events.Input.Gameplay.MenuOpenAction -= OpenMenu;
                _events.GameFlow.OpenMenu -= OpenMenu;
                _events.GameFlow.GameOver -= GameOver;
                _events.GameFlow.GameWin -= GameWin;
                break;
          }
        }

        private void GameWin()
        {
            nextState = GetState(GameFlowStateType.GameWon);
            stage = EVENT.EXIT;
        }
        private void GameOver()
        {
            nextState = GetState(GameFlowStateType.GameOver);
            stage = EVENT.EXIT;
        }

        private void OpenMenu()
        {
            _events.GameFlow.GameOver?.Invoke();
        }
        private void OpenMenu(bool state)
        {
            if (!state)
                return;

            OpenMenu();
        }  
    }

}


