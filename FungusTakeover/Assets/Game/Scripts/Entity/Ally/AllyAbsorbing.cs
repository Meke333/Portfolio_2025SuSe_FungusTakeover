using Game.Interface;
using Zenject;
using Events;

namespace Game.Entity
{
    /// <summary>
    /// Processes what happens when Ally is getting absorbed
    /// </summary>
    public class AllyAbsorbing : AllySystem, IAbsorbable
    {
        [Inject] private AllyEvents _allyEvents;
        [Inject] private EntityResourceManager _resourceManager;
        [Inject] private ProjectEvents _projectEvents;

        public bool IsAbsorbable()
        {
            if (!_isActive)
                return false;

            bool result = _isActive;

            return result;
        }

        public void Absorb()
        {
            //Contact AllyManager to remove the Ally
            //_allyEvents.RemoveNewAllyInManager?.Invoke(_base);
            _projectEvents.Entity.RemoveNewAllyInManager?.Invoke(_base);
            //Give Player Health
            //_allyEvents.GivePlayerHealth?.Invoke(_resourceManager.Health.ResourceValue);
            _projectEvents.Entity.PlayerHealthChanged?.Invoke(_resourceManager.Health.ResourceValue);

            //Disable this Entity //Destroy?
            gameObject.SetActive(false);
        }

    }

}
