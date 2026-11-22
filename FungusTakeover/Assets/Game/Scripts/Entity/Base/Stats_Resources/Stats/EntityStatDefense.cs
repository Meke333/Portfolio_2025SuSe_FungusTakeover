using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatDefense : EntityResourceBase<float>
    {

        protected override float Initialization()
        {
            return _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .defenseStat;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.Defense;
            OutputAction = _events.ResourceOutput.Defense;
        }

        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.Defense += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.Defense -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.Defense?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.Defense = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = _initialStats.GetStats(type).EntityStatContainer.defenseStat;
        }
    }
}

