using Zenject;

namespace Game.Entity
{
    /// <summary>
    /// PlayerSystems are only active when EntityContext is set to Player
    /// </summary>
    public abstract class PlayerSystem : EntitySystem
    {
        [Inject] protected EntityBase _base;
        protected bool _isActive;
        protected bool _subscribedToEvents;
        private bool _isGameObjectEnabled;

        #region UnityMethods

        protected virtual void Awake()
        {
            ToggleActivity(_base.ContextType);
            _events.ResourceOutput.EntityContext += EntityContextChanged;
        }

        protected virtual void OnEnable()
        {
            _isGameObjectEnabled = true;
            CheckActivity();
        }

        protected virtual void OnDisable()
        {
            _isGameObjectEnabled = false;
            CheckActivity();
        }

        public void OnDestroy()
        {
            _events.ResourceOutput.EntityContext -= EntityContextChanged;
        }

        #endregion

        private void EntityContextChanged(EntityContextType type)
        {
            ToggleActivity(type);
            CheckActivity();
        }

        protected virtual void ToggleActivity(EntityContextType type)
        {
            _isActive = type == EntityContextType.Player;
        }

        private void CheckActivity()
        {
            //if (_isActive is false) then it should unsubscribe if _subscribedToEvents is true

            switch (_subscribedToEvents)
            {
                case true:
                    if (!_isActive)
                    {
                        ToggleEvents(false);
                        break;
                    }

                    ToggleEvents(_isGameObjectEnabled);
                    break;
                case false:

                    if (_subscribedToEvents == _isGameObjectEnabled)
                        break;

                    ToggleEvents(_isGameObjectEnabled);

                    break;
            }

            _subscribedToEvents = _isGameObjectEnabled;
        }
        protected virtual void ToggleEvents(bool shouldSubscribe)
        {

        }
    }

}
