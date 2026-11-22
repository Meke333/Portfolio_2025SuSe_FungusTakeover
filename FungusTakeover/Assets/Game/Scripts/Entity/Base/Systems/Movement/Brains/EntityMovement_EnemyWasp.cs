using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntityMovement_EnemyWasp : EntityMovement_Base
    {
        //stays at spawn location while idle
        //attacks when entity in notice radius but does not move!
        //stays at spawn location while player hp > specific percentage
        //targets only player and moves to it once the player fell beneath specific hp percentage

        [Inject] EntityManagers _entityManagers;

        public override void ProcessMovement()
        {
            if (CheckForDeath())
                return;

            SetupKnockback();

            bool _targetPlayer = _entityManagers.allyManager.IsPlayerBelowHPPercent(_entityStatContainer.playerHPLeftPercent);

            /*Save found target in notice range if player isn't below certain hp threshhold*/
            if (_targetPlayer)
                _resources.Target.ResourceValue = _entityManagers.allyManager.Player.transform;
            else
                _resources.Target.ResourceValue = _entityManagers.allyManager.GetClosestAllyNoPlayer(_base);

            ProcessKnockback();

            if (_resources.Target.ResourceValue == null)
            {
                if (!_targetPlayer)
                {
                    NavMeshAgentHelper.NavMeshAgentWalk(_base.navMeshAgent,
                     _base.StartPosition,
                     _resources.StateType.ResourceValue == EntityStateType.Stun,
                     _resources.StunAction.ResourceValue);
                }
                
                return;
            }

            /*Rotation*/
            NavMeshAgentHelper.FaceTarget(_base.navMeshAgent, _resources.Target.ResourceValue.position);

            /*If target isn't player, then just set destination target but keep velocity zero*/
            if (!_targetPlayer)
            {
                NavMeshAgentHelper.NavMeshAgentWalk(_base.navMeshAgent,
                _resources.Target.ResourceValue.position);
                _base.navMeshAgent.velocity = Vector3.zero;
                return;
            }

            /*Process main movement based on target seeking*/
            NavMeshAgentHelper.NavMeshAgentWalk(_base.navMeshAgent,
                _resources.Target.ResourceValue.position,
                _resources.StateType.ResourceValue == EntityStateType.Stun,
                _resources.StunAction.ResourceValue);
            
            
        }
        
        
    }

}

