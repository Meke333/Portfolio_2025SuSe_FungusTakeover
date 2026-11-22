
namespace Game.Entity.States.Base
{
    /// <summary>
    /// EntityState: Dead
    /// Determines the behaviour of Dead Entities
    /// </summary>
    public class Dead : EntityStateBase
    {
        public override void Enter()
        {
            base.Enter();
            ToggleEvents(true);

            
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            ToggleEvents(false);
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Dead;
            base.SetStateType();
        }

        private void ConvertToAlly(EntityContextType type)
        {
            SwitchState(EntityStateType.Awake);
        }

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.ResourceOutput.EntityContext += ConvertToAlly;
                    break;
                case false:
                    _events.ResourceOutput.EntityContext -= ConvertToAlly;
                    break;
            }
        }

    }
}

