using UnityEngine;

namespace Game.Entity.States.Base
{
    /// <summary>
    /// EntityState: Alert
    /// Determines the behaviour of Alert Entities. Alert should get triggered, when Entities notice an attacker or their enemy
    /// no matter if it is through an attack or just through normal notice.
    /// </summary>
    public class Alert : EntityStateBase
    {
        //by default switches to attack if close enough
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

            ChangeToAttack();
        }

        public override void Exit()
        {
            ToggleEvents(false);
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Alert;
            base.SetStateType();
        }
        
        protected virtual void ChangeToAttack()
        {
            if (!CheckIfInRange())
                return;

            SwitchState(EntityStateType.Attack);
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

        protected void OnTakenEntityDamage()
        {
            //exit early if next state is already filled
            if (nextState != null)
                return;
            //check priorities
            if (_resourceManager.Health.ResourceValue <= 0) //switch to dead
                SwitchState(EntityStateType.Dead);
            else if (_resourceManager.StunTimer.ResourceValue > 0) //switch to stun
                SwitchState(EntityStateType.Stun);
        }
        protected void OnTakenEnvironmentDamage()
        {
            if (_resourceManager.Health.ResourceValue <= 0) //switch to dead
                SwitchState(EntityStateType.Dead);
        }
    }
}