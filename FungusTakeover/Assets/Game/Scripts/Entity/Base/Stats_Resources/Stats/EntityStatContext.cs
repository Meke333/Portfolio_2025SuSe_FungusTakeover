using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatContext : ResourceElement<EntityContextType>
    {
        [Inject] private EntityEvents _events;
        [Inject] private EntityResourceManager _resources;

        [Inject] private EntityBase _base;

        protected override EntityContextType Initialization()
        {
            return _base.ContextType;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.EntityContext;
            OutputAction = _events.ResourceOutput.EntityContext;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.EntityContext += HandleInput;
                    break;
                case false:
                    _events.ResourceInput.EntityContext -= HandleInput;
                    break;
            }
        }

        protected override void NotifyEvent(EntityContextType output)
        {
            _events.ResourceOutput.EntityContext?.Invoke(output);
        }
        protected override void SetResourceInManager()
        {
            _resources.EntityContext = this;
        }
    }
}

