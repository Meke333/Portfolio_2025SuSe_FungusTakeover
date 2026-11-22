
using Game.Entity.States.Base;

namespace Game.Entity.States
{
    
    public class EnemyAnt_Idle : Idle
    {
        public override void Enter()
        {
            base.Enter();

            //check for if death on start then immediately switch
            if (_resourceManager.Health.ResourceValue <= 0)
                SwitchState(EntityStateType.Dead);

            SwitchState(EntityStateType.Alert);

            ToggleEvents(true);
        }
    }
}

