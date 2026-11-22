using Game.Entity.States.Base;
using UnityEngine;
using Zenject;

namespace Game.Entity.States
{
    public class EnemyWasp_Stun : Stun
    {
        [Inject] EntityManagers _entityManagers;
        [Inject] EntityBase _base;
         bool _targetPlayer;

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

            _targetPlayer = _entityManagers.allyManager.IsPlayerBelowHPPercent(_base.StatsConfiguration.Enemy.EntityStatContainer.playerHPLeftPercent);
        }
        public override void Update()
        {
            base.Update();

            _resourceManager.StunTimer.ResourceValue -= MyTime.deltaTime;

            if (_resourceManager.StunTimer.ResourceValue <= 0) //check if stun over
                SwitchState(EntityStateType.Alert);

            //check if walkin back or if 
            bool _savePlayerTarget = _entityManagers.allyManager.IsPlayerBelowHPPercent(_base.StatsConfiguration.Enemy.EntityStatContainer.playerHPLeftPercent);

            if (_targetPlayer != _savePlayerTarget && !_targetPlayer) //stopped tracking player
            {
                _targetPlayer = _savePlayerTarget;
                return; //dont do attack check
            }
            _targetPlayer = _savePlayerTarget;

            //check if close enough to target to attack ONLY IF ATTACK IS POSSIBLE DURING STUN
            if (_resourceManager.StunAction.ResourceValue.HasFlag(CharActions.Attack))
            {
                if (!CheckIfInRange() || nextState != null)
                    return;
                
                SwitchState(EntityStateType.Attack);
            }

        }
    }
}

