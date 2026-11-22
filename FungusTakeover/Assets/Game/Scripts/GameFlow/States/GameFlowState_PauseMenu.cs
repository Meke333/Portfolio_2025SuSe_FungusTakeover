using Game.InputHandler;

namespace Game.GameFlow.States
{
    public class GameFlowState_PauseMenu : GameFlowStateBase
    {
        public override void SetStateType()
        {
            stateType = GameFlowStateType.PauseMenu;
            base.SetStateType();
        }

        public override void Enter()
        {
            base.Enter();
            _events.Input.SwitchActionMapAction(InputMapType.Menu);
        }

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.Input.Menu.MenuExitAction += CloseMenu;
                    break;
                case false:
                    _events.Input.Menu.MenuExitAction -= CloseMenu;
                    break;
            }
        }

        private void CloseMenu()
        {
            nextState = GetState(GameFlowStateType.Gameplay);
            stage = EVENT.EXIT;
        }
        private void CloseMenu(bool state)
        {
            if (!state)
                return;

            CloseMenu();
        }

    }


}


