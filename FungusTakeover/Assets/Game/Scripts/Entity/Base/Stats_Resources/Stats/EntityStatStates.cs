using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatStates : ResourceElement<EntityStateType>
    {
        [Inject] private EntityEvents _events;
        [Inject] private EntityResourceManager _resources;
        
        protected override EntityStateType Initialization()
        {
            return EntityStateType.Awake;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.State;
            OutputAction = _events.ResourceOutput.State;
        }

        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.State += HandleInput;
                    break;
                case false:
                    _events.ResourceInput.State -= HandleInput;
                    break;
            }
        }

        protected override void NotifyEvent(EntityStateType output)
        {
            _events.ResourceOutput.State?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.StateType = this;
        }
    }
}