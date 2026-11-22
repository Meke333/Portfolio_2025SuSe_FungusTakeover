using Zenject;

namespace Game.Entity
{
    public abstract class EnemySystem : EntitySystem
    {
        [Inject] protected EntityBase _base;
        protected bool _isActive;

        #region UnityMethods

        protected virtual void Awake()
        {
            ToggleActivity(_base.ContextType);
        }

        protected virtual void OnEnable()
        {
            _events.ResourceOutput.EntityContext += ToggleActivity;

            //ALLYEVENT HERE
        }

        protected virtual void OnDisable()
        {
            _events.ResourceOutput.EntityContext -= ToggleActivity;

            //ALLYEVENT HERE
        }

        #endregion

        private void ToggleActivity(EntityContextType type)
        {
            _isActive = type == EntityContextType.Enemy;
        }
    }

}
