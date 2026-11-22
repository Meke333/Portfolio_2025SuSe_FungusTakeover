using Game.Entity.Events;
using UnityEngine;
using Zenject;

namespace Game.Entity.Test
{
    public class EntityInjectTest : MonoBehaviour
    {
        [Inject] private EntityEvents entityEvents;
        [Inject] private EntityResourceManager entityResource;
        [Inject] private EntityStateData entityStateData;

        void Awake()
        {
            Debug.Log((entityEvents == null));
            Debug.Log((entityResource == null));
            Debug.Log((entityStateData == null));
        }
    }
}


