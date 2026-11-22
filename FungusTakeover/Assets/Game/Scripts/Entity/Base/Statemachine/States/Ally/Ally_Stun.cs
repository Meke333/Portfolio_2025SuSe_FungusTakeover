using Game.Entity.States.Base;
using UnityEngine;

namespace Game.Entity.States
{
    public class Ally_Stun : Stun
    {
        //Additionally subscribe to attack input

        public override void Update()
        {
            base.Update();

            _resourceManager.StunTimer.ResourceValue -= MyTime.deltaTime;

            if (_resourceManager.StunTimer.ResourceValue <= 0) //check if stun over
                SwitchState(EntityStateType.Idle);

        }

        private void ChangeToAttack(bool isAttacking)
        {
            if (!isAttacking || !_resourceManager.StunAction.ResourceValue.HasFlag(CharActions.Attack))
                return;

            SwitchState(EntityStateType.Attack);
        }

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.InputActions.Attacking += ChangeToAttack;
                    break;
                case false:
                    _events.InputActions.Attacking -= ChangeToAttack;
                    break;
            }
        }
    }
}

