using Zenject;
using UnityEngine;
namespace Game.Entity
{
    public abstract class AllySystem : EntitySystem
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
        protected virtual void OnDestroy()
        {
            _events.ResourceOutput.EntityContext -= EntityContextChanged;
        }

        #endregion


        private void EntityContextChanged(EntityContextType type)
        {
            ToggleActivity(type);
            CheckActivity();
        }


        protected void ToggleActivity(EntityContextType type)
        {
            _isActive = (type == EntityContextType.Ally || type == EntityContextType.Player);
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
