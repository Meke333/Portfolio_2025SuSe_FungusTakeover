using UnityEngine;

namespace Game.Entity
{
    public class EntityResourceKnockback : EntityResourceBase<Vector3>
    {

        protected override Vector3 Initialization()
        {
            return Vector3.zero;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.Knockback;
            OutputAction = _events.ResourceOutput.Knockback;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.Knockback += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.Knockback -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(Vector3 output)
        {
            _events.ResourceOutput.Knockback?.Invoke(output);
        }
        protected override void SetResourceInManager()
        {
            _resources.Knockback = this;
        }

        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = Vector3.zero;
        }
    }
}

