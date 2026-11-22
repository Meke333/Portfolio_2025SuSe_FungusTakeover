using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;
using Sirenix.OdinInspector;

namespace Game.Entity
{
    public class EntityStatStunAction : EntityResourceBase<CharActions>
    {
        
        protected override CharActions Initialization()
        {
            return CharActions.All;
        }

        protected override void SetActions()
        {
            InputAction = _events.ResourceInput.StunActions;
            OutputAction = _events.ResourceOutput.StunActions;
        }
        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true: 
                    _events.ResourceInput.StunActions += HandleInput;
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceInput.StunActions -= HandleInput;
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }
        }

        protected override void NotifyEvent(CharActions output)
        {
            _events.ResourceOutput.StunActions?.Invoke(output);
        }

        protected override void SetResourceInManager()
        {
            _resources.StunAction = this;
        }
        protected override void ChangeValuesWithNewContext(EntityContextType type)
        {
            ResourceValue = CharActions.All;
        }
    }
}