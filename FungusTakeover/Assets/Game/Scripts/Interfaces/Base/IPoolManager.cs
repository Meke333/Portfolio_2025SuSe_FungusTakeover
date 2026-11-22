using ObjectPool;
using UnityEngine;

namespace Interface
{
    public interface IPoolManager
    {
        public PoolObjectBase ActivateAndPlace(Vector3 position);
        public PoolObjectBase ActivateInOtherManager(int hash, Vector3 position);
    }

}

