using Helper;
using Interface;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ObjectPool
{
    [Serializable]
	public class GameObjectPoolBase<T> where T : PoolObjectBase
    {
        [Inject] private FactoryHelper _factoryHelper;
        [Inject] private IInstantiator _instantiator;

        public Action PoolIsFilled;

        [field: SerializeField, ReadOnly] protected T[] pool { get; private set; }


        /*[SerializeField, ReadOnly] private int _activeObjectAmount;
        public int ActiveObjectAmount
		{
			get => _activeObjectAmount;
			set
			{
				_activeObjectAmount = (int) Math.Clamp(value, 0, pool.Length);
			}
		}*/

        protected Transform objectParent;

		protected GameObject elementPrefab;

        private int _latestIndex;


        //Pseudo Constructors
        public void Construct(Transform parent, IPoolManager manager = null)
        {
            pool = parent.GetComponentsInChildren<T>();
            objectParent = parent;


            PoolIsFilled?.Invoke();

            if (manager == null)
                return;

            /*for (int i = 0; i < pool.Length; i++)
            {
                pool[i].TransmitPoolInterface(manager);
            }*/
        }

        public void Construct(Transform parent, GameObject prefab, int length, IPoolManager manager = null)
        {
            pool = new T[length];

            objectParent = parent;
            elementPrefab = prefab;

            FillInPool();

            if (manager == null)
                return;

            /*for (int i = 0; i < pool.Length; i++)
            {
                pool[i].TransmitPoolInterface(manager);
            }*/
        }

        public virtual void Initialize()
		{
			//Events
		}

        public virtual void Dispose()
		{
			//Events
		}

        private void FillInPool()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = _instantiator.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();

                //for some reason this can't fill in the Dependencies...?
                //pool[i] = _factoryHelper.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();
                SetElementActivity(i, false);
            }

            PoolIsFilled?.Invoke();
        }

        public void ResetPool()
		{
			pool = new T[pool.Length];
			FillInPool();

            //ActiveObjectAmount = 0;

            _latestIndex = 0;
        }

        public void ResetAndDeletePool()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				T temp = pool[i];
				pool[i] = null;

                UnityEngine.Object.Destroy(temp.gameObject);
			}

            pool = new T[pool.Length];
            FillInPool();

            //ActiveObjectAmount = 0;

            _latestIndex = 0;
        }

        public int GetActiveAmount()
		{
            int result = 0;

            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i].IsGameObjectActive())
                    result++;
            }

			return result;

        }

        public bool IsFull()
		{
			return GetActiveAmount() >= pool.Length;

        }

        public bool IsEmpty()
		{
			return GetActiveAmount() <= 0;
		}

        public bool IsIndexActive(int index)
		{
			if (index < 0 || index >= pool.Length)
				throw new IndexOutOfRangeException("Index out of bounds from Object Pool!");

			return pool[index].gameObject.activeSelf;
		}

        public T GetFirstActiveObject()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i].IsGameObjectActive())
                {
                    return pool[i];
                }
            }

            return null;
        }

        public T GetLastActiveObject()
		{
            for (int i = pool.Length - 1; i >= 0; i--)
            {
                if (pool[i].IsGameObjectActive())
                {
                    return pool[i];
                }
            }

            return null;
        }

		private void SetElementActivity(int index, bool value)
		{
            pool[index].gameObject.SetActive(value);
            pool[index].SetActivity(value);
        }


        /// <summary>
        /// Remove an Object with an index, which starts from the queue.
        /// </summary>
        /// <param name="index">Index starting from the currentIndex</param>
        /// <returns>
        /// true: Object at Index exists and is removed from pool
        /// false: Object at Index does not exist and therefore can not be removed
        /// </returns>
        public bool ResetAtIndex(int index)
        {
            if (IsEmpty() || pool[index] == null)
                return false;

			bool isActive = pool[index].gameObject.activeSelf;

            pool[index] = _factoryHelper.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();
            SetElementActivity(index, isActive);

            return true;
        }

        public bool ResetAndDeleteAtIndex(int index)
		{
            if (IsEmpty() || pool[index] == null)
                return false;

            bool isActive = pool[index].gameObject.activeSelf;

            UnityEngine.Object.Destroy(pool[index].gameObject);
            pool[index] = _factoryHelper.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();
            SetElementActivity(index, isActive);

            return true;
        }

        public bool ResetObject(T objectToRemove)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i] != objectToRemove)
                    continue;

                bool isActive = pool[i].gameObject.activeSelf;
                pool[i] = _factoryHelper.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();
                SetElementActivity(i, isActive);

				return true;
			}
			return false;
		}

        public bool ResetAndDeleteObject(T objectToRemove)
		{
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i] != objectToRemove)
                    continue;

                bool isActive = pool[i].gameObject.activeSelf;
                UnityEngine.Object.Destroy(pool[i].gameObject);
                pool[i] = _factoryHelper.InstantiatePrefab(elementPrefab, objectParent).GetComponent<T>();
                SetElementActivity(i, isActive);

                return true;
            }
            return false;
        }

        public T ActivateFirstDeactiveObject(bool deactivateIfFull = false)
		{
			if (IsFull())
            {
                if (deactivateIfFull)
                    DeactivateLastOfLatestActiveObject();
                else
                    return null;
            }

            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i].IsGameObjectActive())
                    continue;

                SetElementActivity(i, true);
                _latestIndex = i;

                return pool[i];
            }

            return null;
		}

        

        public T DeactivateFirstActiveObject()
		{
			if (IsEmpty())
				return null;

            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].IsGameObjectActive())
                    continue;

                SetElementActivity(i, false);

                return pool[i];
            }

            return null;
        }

        public T DeactivateLastOfLatestActiveObject()
        {
            if (IsEmpty())
                return null;

            int lastToCheck = _latestIndex;

            for (int i = 0; i < pool.Length; i++)
            {
                lastToCheck = (_latestIndex - (i+1) + pool.Length) % pool.Length;

                if (!pool[i].IsGameObjectActive())
                    continue;

                SetElementActivity(i, false);
                
                return pool[i];
            }

            return null;
        }

        public List<T> DeactivateAllActiveObjects()
        {
            if (IsEmpty())
                return null;

            List<T> result = new List<T>();

            for (int i = 0; i < pool.Length; i++)
            {
                if (!pool[i].IsGameObjectActive())
                    continue;

                SetElementActivity(i, false);

                result.Add(pool[i]);
            }

            return result;


        }

	}
}