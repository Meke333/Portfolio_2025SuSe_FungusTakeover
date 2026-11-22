using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Base
{
    public abstract class StatemachineBase<T, U> : MonoBehaviour where T : StateBase where U : StateData
    {
        [Inject] protected IInstantiator instantiator;

        protected T currentState;
        protected U stateData;

        [SerializeField, ReadOnly] private bool isActive;

        public void SetUp(T startState, U data)
        {
            currentState = startState;
            stateData = data;

            isActive = true;
        }

        #region Unity Methods
        protected virtual void Update()
        {
            if (!isActive)
                return;

            currentState = (T) currentState.Process();
        }
        #endregion

        protected void ToggleActivity(bool active)
        {
            isActive = active;
        }
    }
}

