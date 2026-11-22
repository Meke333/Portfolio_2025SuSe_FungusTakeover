using Game.Entity.States.Base;
using UnityEngine;
using Zenject;

namespace Game.Entity.States
{
    public class EnemyWasp_Attack : Attack
    {
        public override void Update()
        {
            base.Update();

            //Windup time, returns if it's running so attack cooldown doesn't get processed quite yet
            if (_windUpTimer > 0)
            {
                _windUpTimer -= MyTime.deltaTime;
                if (_windUpTimer <= 0)
                    _events.EntityUnit.TriggerAttack?.Invoke();
                return;
            }
            if (_postAttackTimer > 0)
            {
                _postAttackTimer -= MyTime.deltaTime;
                if (_postAttackTimer <= 0)
                    _events.EntityUnit.AttackEnded?.Invoke();
            }
            //Attack cooldown to go back to regular state machine once another attack is possible
            _attackTimer -= MyTime.deltaTime;

            if (_attackTimer <= 0)
            {
                SwitchState(EntityStateType.Alert);
                _events.EntityUnit.AttackEnded?.Invoke();
            }
        }
    }
}

