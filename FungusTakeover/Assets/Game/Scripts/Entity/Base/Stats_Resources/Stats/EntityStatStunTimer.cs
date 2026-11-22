using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatStunTimer : EntityResourceBase<float>
    {

        protected override float Initialization()
        {
            return 0f;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.StunTimer;
            OutputAction = _events.ResourceOutput.StunTimer;
        }

        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.StunTimer += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.StunTimer -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.StunTimer?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.StunTimer = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = 0f;
        }
    }
}

