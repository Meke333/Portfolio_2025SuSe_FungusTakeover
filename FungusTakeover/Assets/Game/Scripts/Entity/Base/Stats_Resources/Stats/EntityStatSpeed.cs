using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public class EntityStatSpeed : EntityResourceBase<float>
    {

        protected override float Initialization()
        {
            return _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .speedStat;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.Speed;
            OutputAction = _events.ResourceOutput.Speed;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.Speed += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.Speed -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.Speed?.Invoke(output);
        }
        protected override void SetResourceInManager()
        {
            _resources.Speed = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = _initialStats.GetStats(type).EntityStatContainer.speedStat;
        }
    }
}

