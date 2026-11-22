using UnityEngine;
using Zenject;

namespace Game.Entity.States.Base
{
    /// <summary>
    /// EntityState: Attack
    /// Determines the behaviour of Attacking Entities. This is the trigger for actual Attacking.
    /// Entity should go back to the Alert State when they are finished with the Attack (additional Cooldown?)
    /// This state is there to differentiate between being Alert und doing an Attack.
    /// </summary>
    public class Attack : EntityStateBase
    {
        //switch state if: stunned/attack finished (build in a timer linked to animation length???) wait we got a cooldown var lol
        //to-do bro really link this to the animation timer :teary_eyed_emoji:
        protected float _attackTimer;
        protected float _windUpTimer;
        protected float _postAttackTimer;
        protected EntityStatContainer _stats;

        public override void Enter()
        {
            base.Enter();
            
            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
            {
                SwitchState(EntityStateType.Dead);
                return;
            }

            //grab attack cooldown and windup time
            if (_attackTimer <= 0)
                _attackTimer = _allStatsConfig.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer.attackCooldown;
            if (_windUpTimer <= 0)
                _windUpTimer = _allStatsConfig.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer.attackWindUp;

            if (_windUpTimer > 0) //if windup exists: trigger windup event, otherwise trigger attack
                _events.EntityUnit.TriggerWindup?.Invoke();
            else
                _events.EntityUnit.TriggerAttack?.Invoke();

            _stats = _allStatsConfig.GetStats(_resourceManager.EntityContext.ResourceValue).EntityStatContainer;
            if (_stats.attackBehaviourType == EntityAttackType.Animated)
                _postAttackTimer = _stats.attackThrustRange;
            else
                _postAttackTimer = 0.5f;
            
            ToggleEvents(true);
        }

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
                SwitchState(EntityStateType.Idle);
        }

        public override void Exit()
        {
            _windUpTimer = 0;
            _postAttackTimer = 0;
            _events.EntityUnit.AttackEnded?.Invoke(); //just trigger it so movement definitely turns back on lol 
            ToggleEvents(false);
            base.Exit();
        }

        public override void SetStateType()
        {
            stateType = EntityStateType.Attack;
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

