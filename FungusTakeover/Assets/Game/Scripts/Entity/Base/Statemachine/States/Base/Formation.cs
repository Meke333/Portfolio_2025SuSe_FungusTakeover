using Zenject;

namespace Game.Entity.States.Base
{
    public class Formation : EntityStateBase
    {
        [Inject] EntityBase _base;

        public override void Enter()
        {
            base.Enter();
            ToggleEvents(true);
        }

        public override void Update()
        {
            base.Update();

            if (IsFormationOver())
                SwitchState(EntityStateType.Idle);
        }
        
        public override void Exit()
        {
            ToggleEvents(false);
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Formation;
            base.SetStateType();
        }
        
        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    break;
                case false:
                    break;
            }
        }

        private bool IsFormationOver()
        {
            if (_base._allyFormation.isInFormation)
                return false;
            return true;
        }
    }
}