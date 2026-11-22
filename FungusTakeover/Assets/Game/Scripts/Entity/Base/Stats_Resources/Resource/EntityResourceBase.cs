using Game.Entity.Events;
using Game.ResourceElement;
using Zenject;

namespace Game.Entity
{
    public abstract class EntityResourceBase<T> : ResourceElement<T>
    {
        [Inject] protected EntityEvents _events;
        [Inject] protected EntityResourceManager _resources;

        [Inject] protected EntityAllStatsConfiguration _initialStats;
        [Inject] protected EntityBase _base;

        protected override void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.ResourceOutput.EntityContext += ChangeValuesWithNewContext;
                    break;
                case false:
                    _events.ResourceOutput.EntityContext -= ChangeValuesWithNewContext;
                    break;
            }

            base.ToggleActionSubscription(shouldSubscribe);
        }

        protected abstract void ChangeValuesWithNewContext(EntityContextType type);
    }
}

