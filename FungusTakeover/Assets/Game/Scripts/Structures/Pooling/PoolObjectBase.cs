using Interface;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    public abstract class PoolObjectBase : MonoBehaviour, IPoolObject
    {
        [Inject] protected IPoolManager poolManager;
         
        private void Awake()
        {
            //Debug.Log(poolManager == null);
        }
        public bool IsGameObjectActive()
        {
            if (gameObject == null)
                return false;

            return gameObject.activeSelf;
        }

        public abstract void ResetObject();
        public void SetActivity(bool value)
        {
            gameObject.SetActive(value);
        }

        public void TransmitPoolInterface(IPoolManager ipoolManager)
        {
            poolManager = ipoolManager;
        }
    }
}