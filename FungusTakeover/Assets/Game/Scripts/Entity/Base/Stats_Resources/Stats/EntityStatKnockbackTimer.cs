using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatKnockbackTimer : EntityResourceBase<float>
    {

        protected override float Initialization()
        {
            return 0f;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.KnockbackTimer;
            OutputAction = _events.ResourceOutput.KnockbackTimer;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.KnockbackTimer += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.KnockbackTimer -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.KnockbackTimer?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.KnockbackTimer = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = 0f;
        }
    }
}

