using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.ResourceElement
{
    public abstract class ResourceElement<T> : MonoBehaviour
    {
        #region Values
        [SerializeField, ReadOnly] protected T _resourceValue;

        protected Action<T> InputAction;
        protected Action<T> OutputAction;

        #endregion

        #region Properties
        public T ResourceValue
        {
            get => _resourceValue;
            set
            {
                _resourceValue = value;

                NotifyEvent(_resourceValue);
            }
        }

        #endregion

        #region UnityMethod
        protected virtual void Awake()
        {
            SetActions();
            SetResourceInManager();

            ResourceValue = Initialization();
        }
        private void OnEnable()
        {
            ToggleActionSubscription(true);
        }
        private void OnDisable()
        {
            ToggleActionSubscription(false);
        }
        #endregion

        #region Methods


        /// <summary>
        /// Initialization Method, this requires the Value to be filled in!
        /// Gets called in Awake!
        /// </summary>
        protected abstract T Initialization();

        /// <summary>
        /// Set the Input and Output Actions!!!
        /// </summary>
        protected abstract void SetActions();

        /// <summary>
        /// Set this Element to the ResourceManager
        /// </summary>
        protected abstract void SetResourceInManager();
        protected virtual void ToggleActionSubscription(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    InputAction += HandleInput;
                    break;
                case false:
                    InputAction -= HandleInput;
                    break;
            }

        }

        public virtual void HandleInput(T newValue)
        {
            ResourceValue = newValue;
        }



        protected virtual void NotifyEvent(T output)
        {
            OutputAction?.Invoke(output);
        }

        [Button(ButtonStyle.Box, Expanded = true)]
        protected virtual void ChangeValue(T input)
        {
            ResourceValue = input;
        }
        #endregion
    }


}

