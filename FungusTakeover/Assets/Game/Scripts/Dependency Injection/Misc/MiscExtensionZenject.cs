using ModestTree;
using System.Linq;
using UnityEngine;
using Zenject;


#if UNITY_EDITOR
#endif


namespace DependencyInjection
{
    public static class MiscExtensionZenject
    {
        /// <summary>
        /// Creates an empty game objects (not destroyed on load) adds the monobheaviour <see cref="T"/> and binds it as a singleton to the interface <see cref="Y"/>
        /// </summary>
        /// <typeparam name="Y">The interface you wind to bind the behaviour to</typeparam>
        /// <typeparam name="T">The monobheaviour you want to bind as a singleton</typeparam>
        public static ConcreteIdArgConditionCopyNonLazyBinder BindSingletonMonoBehaviour<Y, T>(this DiContainer container) where T : MonoBehaviour, Y
        {

            return container.BindInstance<Y>(container.InstantiateSingletonMonoBehaviour<T>()).AsSingle();
        }

        public static IdScopeConcreteIdArgConditionCopyNonLazyBinder BindInterfacesToInstance<T>(this DiContainer container, T instance, bool toSelf = false)
        {
            var interfaces = instance.GetType().Interfaces();
            var statement = container.StartBinding();
            var bindInfo = statement.SpawnBindInfo();
            if (interfaces.Any())
            {
                bindInfo.ContractTypes.AddRange(interfaces);
            }
            if (toSelf)
            {
                bindInfo.ContractTypes.Add(instance.GetType());
            }


            statement.SetFinalizer(
                new ScopableBindingFinalizer(
                    bindInfo,
                    (container, type) => new InstanceProvider(type, instance, container, bindInfo.InstantiatedCallback)));

            return new IdScopeConcreteIdArgConditionCopyNonLazyBinder(bindInfo);
        }


        public static T InstantiateSingletonMonoBehaviour<T>(this DiContainer container) where T : MonoBehaviour
        {
            GameObject gameObject = new GameObject("Singleton " + typeof(T).Name);
            GameObject.DontDestroyOnLoad(gameObject);
            return container.InstantiateComponent<T>(gameObject);
        }
    }



}

