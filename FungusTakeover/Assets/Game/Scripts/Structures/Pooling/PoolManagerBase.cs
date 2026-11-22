using Events;
using Interface;
using System;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    public abstract class PoolManagerBase<T> : MonoBehaviour, IPoolManager where T : PoolObjectBase
    {
        [Inject] protected ProjectEvents projectEvents;
        [Inject] protected IInstantiator instantiator;
        [Inject] protected PoolManagerData _dataBase;


        protected GameObjectPoolBase<T> pool;

        protected virtual void Awake()
        {
            AddToPoolManager();
            pool = instantiator.Instantiate<GameObjectPoolBase<T>>();

            projectEvents.Pooling.DeactivateAllInteracttions += DeactivatingAllInteractions;
        }

        private void DeactivatingAllInteractions()
        {
            pool.DeactivateAllActiveObjects();
        }

        protected virtual void Start()
        {
            pool.Construct(transform);
            _dataBase.APoolIsFilled();

        }

        private void OnDestroy()
        {
            projectEvents.Pooling.DeactivateAllInteracttions -= DeactivatingAllInteractions;
        }

        public PoolObjectBase ActivateAndPlace(Vector3 position)
        {
            T result = pool.ActivateFirstDeactiveObject(true);

            if (result == null)
                return null;

            result.transform.position = position;
            return result;
        }

        private void AddToPoolManager()
        {
            Debug.Log(this.GetType().Name);
            Debug.Log(this.GetType().GetHashCode());
            _dataBase.AddInDictionary(this.GetType().GetHashCode(), this);
        }

        public PoolObjectBase ActivateInOtherManager(int hash, Vector3 position)
        {
            return _dataBase.ActivateObject(hash, position);
        }
    }
}