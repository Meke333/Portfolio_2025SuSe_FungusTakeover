using Game.Entity.States.Base;
using UnityEngine;
using Zenject;

namespace Game.Entity.States
{
    
    public class EnemyWasp_Idle : Idle
    {
        [Inject] EntityBase _base;
        public override void Enter()
        {
            base.Enter();

            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
                SwitchState(EntityStateType.Dead);
            else if (_base.StartPosition == _base.transform.position) //check if arrived at start position
                 SwitchState(EntityStateType.Alert);

            ToggleEvents(true);
        }

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage += OnTakenEnvironmentDamage;
                    _events.ResourceOutput.Target += OnTargetChanged;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= OnTakenEntityDamage;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= OnTakenEnvironmentDamage;
                    _events.ResourceOutput.Target -= OnTargetChanged;
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
        protected void OnTargetChanged(Transform target)
        {
            if (nextState != null)
                return;

        }
    }
}

