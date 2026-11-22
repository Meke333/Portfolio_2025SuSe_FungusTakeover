using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntityMovement_EnemyAnt : EntityMovement_Base
    {
        [Inject] EntityManagers _entityManagers;

        public override void ProcessMovement()
        {
            if (CheckForDeath())
                return;

            SetupKnockback();

            /*Save found target*/
            _resources.Target.ResourceValue = _entityManagers.allyManager.GetClosestAlly(_base);

            ProcessKnockback();

            if (_resources.Target.ResourceValue == null)
                return;
            /*Rotation*/
            NavMeshAgentHelper.FaceTarget(_base.navMeshAgent, _resources.Target.ResourceValue.position);

            /*Process main movement based on target seeking*/
            NavMeshAgentHelper.NavMeshAgentWalk(_base.navMeshAgent,
                _resources.Target.ResourceValue.position,
                _resources.StateType.ResourceValue == EntityStateType.Stun,
                _resources.StunAction.ResourceValue);
            
        }
    }
}
