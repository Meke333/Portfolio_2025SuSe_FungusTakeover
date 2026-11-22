using UnityEngine;

namespace Game.Entity
{
    public class EntityResourceTarget : EntityResourceBase<Transform>
    {

        protected override Transform Initialization()
        {
            return null;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.Target;
            OutputAction = _events.ResourceOutput.Target;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.Target += HandleInput; 
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.Target -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(Transform output)
        {
            _events.ResourceOutput.Target?.Invoke(output);
        }
        protected override void SetResourceInManager()
        {
            _resources.Target = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = null;
        }
    }
}

