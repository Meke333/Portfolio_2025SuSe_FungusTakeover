using UnityEngine;

namespace Game.Entity
{
    public class EntityResourceHealth : EntityResourceBase<float>
    {
        protected override float Initialization()
        {
            return _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .healthPoints;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.HealthPoints;
            OutputAction = _events.ResourceOutput.HealthPoints;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.HealthPoints += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.HealthPoints -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        public void AddValue(float add)
        {
            ResourceValue = Mathf.Min(
                ResourceValue + add, 
                _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .healthPoints);
        }

        public override void HandleInput(float newValue)
        {
            ResourceValue = Mathf.Min(
                ResourceValue,
                _initialStats.GetStats(_base.ContextType)
                .EntityStatContainer
                .healthPoints);
        }

        protected override void NotifyEvent(float output)
        {
            _events.ResourceOutput.HealthPoints?.Invoke(output);
        }
        protected override void SetResourceInManager()
        {
            _resources.Health = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = _initialStats.GetStats(type).EntityStatContainer.healthPoints;
        }
    }
}

