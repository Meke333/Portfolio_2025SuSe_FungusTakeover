namespace Game.Entity.States.Base
{
    /// <summary>
    /// EntityState: Awake
    /// Determines the behaviour of Awakening Entities, Entity usually start with this behaviour!
    /// </summary>
    public class Awake : EntityStateBase
    {
        public override void Enter()
        {
            base.Enter();
            
            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
                SwitchState(EntityStateType.Dead);
            else
                SwitchState(EntityStateType.Idle);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Awake;
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
    }
}