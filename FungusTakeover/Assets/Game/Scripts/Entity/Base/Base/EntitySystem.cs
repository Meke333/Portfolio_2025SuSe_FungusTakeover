using Game.Entity.Events;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntitySystem : MonoBehaviour
    {
        [Inject] protected EntityEvents _events;

    }
}
