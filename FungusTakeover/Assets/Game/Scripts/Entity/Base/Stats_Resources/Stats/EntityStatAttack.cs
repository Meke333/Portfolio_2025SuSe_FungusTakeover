using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatAttack : EntityResourceBase<float>
    {

        protected override float Initialization()
        {
            return _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .attackStat;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.Attack;
            OutputAction = _events.ResourceOutput.Attack;
        }        
        
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.Attack += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.Attack -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.Attack?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.Attack = this;
        }


        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = _initialStats.GetStats(type).EntityStatContainer.attackStat;
        }
    }
}

