using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Helper
{
	public class FactoryHelper
	{
        [Inject] private IInstantiator _instantiator;

        /*public FactoryHelper(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }*/

		public T InstantiateObject<T>() where T : MonoBehaviour
		{
			return _instantiator.Instantiate<T>();
        }
        public T InstantiateObject<T>(Transform parent) where T : MonoBehaviour
        {
            T result = _instantiator.Instantiate<T>((IEnumerable<object>)parent);

            return result;
        }

        public T InstantiateObject<T>(Vector3 position, Quaternion rotiation, Transform parent = null) where T : MonoBehaviour
		{
            T result = _instantiator.Instantiate<T>((IEnumerable<object>)parent);
			Transform resultTransform = result.GetComponent<Transform>();

			resultTransform.SetPositionAndRotation(position, rotiation);

			return result;
        }

        public T InstantiateObject<T>(Vector2 position, float rotation, Transform parent = null) where T : MonoBehaviour
        {
            T result = _instantiator.Instantiate<T>((IEnumerable<object>)parent);
            Transform resultTransform = result.GetComponent<Transform>();

            resultTransform.SetPositionAndRotation(position, Quaternion.AngleAxis(rotation, Vector3.forward));

            return result;
        }
        
        
        public GameObject InstantiatePrefab(GameObject objectToInstantiate)
		{
            return _instantiator.InstantiatePrefab(objectToInstantiate);
        }
        public GameObject InstantiatePrefab(GameObject objectToInstantiate, Transform parent)
        {
            GameObject result = _instantiator.InstantiatePrefab(objectToInstantiate, parent);

            return result;
        }

        public GameObject InstantiatePrefab(GameObject objectToInstantiate, Vector3 position, Quaternion rotiation, Transform parent = null)
		{
            GameObject result = _instantiator.InstantiatePrefab(objectToInstantiate, parent);
			Transform resultTransform = result.transform;

			resultTransform.SetPositionAndRotation(position, rotiation);

			return result;
        }

        public GameObject InstantiatePrefab<T>(GameObject objectToInstantiate, Vector2 position, float rotation, Transform parent = null)
        {
            GameObject result = _instantiator.InstantiatePrefab(objectToInstantiate, parent);
            Transform resultTransform = result.transform;

            resultTransform.SetPositionAndRotation(position, Quaternion.AngleAxis(rotation, Vector3.forward));

            return result;
        }

    }
}