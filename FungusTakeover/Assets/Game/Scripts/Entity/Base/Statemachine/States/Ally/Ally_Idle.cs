
using Game.Entity.States.Base;

namespace Game.Entity.States
{
    
    public class Ally_Idle : Idle
    {
        //additionally subscribes to the attack event
        //as well as not going to alert upon getting damaged

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage += OnTakenEnvironmentDamage;
                    _events.InputActions.Attacking += SwitchToAttack;
                    _events.InputActions.CallingAllies += SwitchToFormation;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= OnTakenEnvironmentDamage;
                    _events.InputActions.Attacking -= SwitchToAttack;
                    _events.InputActions.CallingAllies -= SwitchToFormation;
                    break;
            }
        }

        protected override void OnTakenEntityDamage()
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
        protected override void OnTakenEnvironmentDamage()
        {
            if (_resourceManager.Health.ResourceValue <= 0) //switch to dead
                SwitchState(EntityStateType.Dead);
        }
        private void SwitchToAttack(bool input)
        {
            if (nextState != null || !input)
                return;

            SwitchState(EntityStateType.Attack);
        }
        private void SwitchToFormation(bool input)
        {
            if (nextState != null || !input)
                return;

            SwitchState(EntityStateType.Formation);
        }
    }
}

