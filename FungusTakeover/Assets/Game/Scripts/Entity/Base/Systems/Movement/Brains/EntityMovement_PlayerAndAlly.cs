using Zenject;

namespace Game.Entity
{
    public class EntityMovement_PlayerAndAlly : EntityMovement_Base
    {
        public override void ProcessMovement()
        {
            if (_base == null)
                throw new System.NullReferenceException("Player/Ally Movement: Entity base is null");
            if (_base._allyFormation == null)
                throw new System.NullReferenceException("Player/Ally Movement: Entity Formation is null");

            if (CheckForDeath())
                return;

            if (_base._allyFormation.isInFormation)
            {
                if (_base.ContextType == EntityContextType.Ally)
                {
                    NavMeshAgentHelper.NavMeshAgentWalk(_base.navMeshAgent, _base.ResourceManager.Target.ResourceValue.position);
                }
                return;
            }

            SetupKnockback();

            /*Process main movement based on the controller input*/
            NavMeshAgentHelper.ControllerMovement(_base.transform,_base.InputVector,
                _entityStatContainer.rotationSpeed,
                _resources.Speed.ResourceValue,_resources.StateType.ResourceValue == EntityStateType.Stun,_resources.StunAction.ResourceValue);

            ProcessKnockback();
        }
    }

}

