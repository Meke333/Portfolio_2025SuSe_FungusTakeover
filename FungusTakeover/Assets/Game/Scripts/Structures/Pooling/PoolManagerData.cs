using Events;
using Interface;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    public class PoolManagerData
    {
        [Inject] private ProjectEvents _projectEvents;
        private int _poolAmount;
        private int _filledPoolAmount;

        //HashCode -> PoolManager
        public Dictionary<int, IPoolManager> PoolManagers = new Dictionary<int, IPoolManager>();

        public void AddInDictionary (int hash, IPoolManager poolManager)
        {
            PoolManagers.Add(hash, poolManager);

            _poolAmount = PoolManagers.Count;
            Debug.Log("Added: " + _poolAmount + "; " + _filledPoolAmount);
        }

        public void APoolIsFilled()
        {
            _filledPoolAmount++;


            Debug.Log("Filling: " + _filledPoolAmount + "; " + _poolAmount);

            if (_filledPoolAmount < _poolAmount)
                return;

            Debug.Log("Filled");
            //_projectEvents.ActivateSaveSystem?.Invoke();
        }

        public void ResetFilledPoolAmount()
        {
            _filledPoolAmount = 0;
        }

        public PoolObjectBase ActivateObject(int hash, Vector3 position)
        {
             return PoolManagers[hash]?.ActivateAndPlace(position);
        }
    }
}