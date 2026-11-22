using Game.Entity.States.Base;
using Zenject;

namespace Game.Entity.States
{
    public class EnemyWasp_Alert : Alert
    {
        [Inject] EntityManagers _entityManagers;
        [Inject] EntityBase _base;
        bool _targetPlayer;

        public override void Enter()
        {
            base.Enter();

            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
                SwitchState(EntityStateType.Dead);

            ToggleEvents(true);

            _targetPlayer = _entityManagers.allyManager.IsPlayerBelowHPPercent(_base.StatsConfiguration.Enemy.EntityStatContainer.playerHPLeftPercent);
        }
        public override void Update()
        {
            base.Update();

            bool _savePlayerTarget = _entityManagers.allyManager.IsPlayerBelowHPPercent(_base.StatsConfiguration.Enemy.EntityStatContainer.playerHPLeftPercent);
            
            if (_targetPlayer != _savePlayerTarget && !_targetPlayer) //stopped tracking player
            {
                SwitchState(EntityStateType.Idle);
            }
            _targetPlayer = _savePlayerTarget;
            ChangeToAttack();
        }

        protected override void ChangeToAttack()
        {
            if (!CheckIfInRange() || nextState != null)
                return;

            SwitchState(EntityStateType.Attack);
        }
    }
}