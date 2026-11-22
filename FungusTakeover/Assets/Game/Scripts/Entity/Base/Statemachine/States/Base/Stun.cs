using UnityEngine;
using Zenject;

namespace Game.Entity.States.Base
{
    /// <summary>
    /// EntityState: Stun
    /// Determines the behaviour of Stun Entities. What the Entities can do and what not
    /// is determined in the EntityConfiguration via the StunAction
    /// </summary>
    public class Stun : EntityStateBase
    {
        public override void Enter()
        {
            base.Enter();
            
            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
            {
                _resourceManager.StunTimer.ResourceValue = 0;
                SwitchState(EntityStateType.Dead);
            }
            ToggleEvents(true);
        }

        public override void Update()
        {
            base.Update();

            _resourceManager.StunTimer.ResourceValue -= MyTime.deltaTime;

            if (_resourceManager.StunTimer.ResourceValue <= 0) //check if stun over
                SwitchState(EntityStateType.Alert);

            //check if close enough to target to attack ONLY IF ATTACK IS POSSIBLE DURING STUN
            if (_resourceManager.StunAction.ResourceValue.HasFlag(CharActions.Attack))
            {
                if (!CheckIfInRange() || nextState != null)
                    return;
                SwitchState(EntityStateType.Attack);
            }

        }

        public override void Exit()
        {
            ToggleEvents(false);
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Stun;
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

