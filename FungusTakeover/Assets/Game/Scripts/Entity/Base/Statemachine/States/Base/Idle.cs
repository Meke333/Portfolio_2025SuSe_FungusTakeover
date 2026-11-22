using Zenject;

namespace Game.Entity.States.Base
{
    
    /// <summary>
    /// EntityState: Idle
    /// Determines the behaviour of Idling Entities. Normal State of the Entities.
    /// </summary>
    public class Idle : EntityStateBase
    {
        public override void Enter()
        {
            base.Enter();

            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
                SwitchState(EntityStateType.Dead);

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
            stateType = EntityStateType.Idle;
            base.SetStateType();
        }

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage += OnTakenEnvironmentDamage;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= OnTakenEnvironmentDamage;
                    break;
            }
        }

        protected virtual void OnTakenEntityDamage()
        {
            //exit early if next state is already filled
            if (nextState != null)
                return;
            //check priorities
            if (_resourceManager.Health.ResourceValue <= 0) //switch to dead
                SwitchState(EntityStateType.Dead);
            else if (_resourceManager.StunTimer.ResourceValue > 0) //switch to stun
                SwitchState(EntityStateType.Stun);
            else //switch to alert
                SwitchState(EntityStateType.Alert);
        }
        protected virtual void OnTakenEnvironmentDamage()
        {
            if (_resourceManager.Health.ResourceValue <= 0) //switch to dead
                SwitchState(EntityStateType.Dead);
        }

    }
}

