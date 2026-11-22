using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
        /// <summary>
        /// A <see cref="ScriptableObjectInstaller"/> that installs to the <see cref="StaticContext"/> in
        /// the Unity Editor, and can be used as a normal installer at runtime. Install bindings in <see
        /// cref="InstallBindings(DiContainer)"/>.
        /// </summary>
        [DefaultExecutionOrder(-9900)]
        public abstract class EditorScriptableObjectInstaller : ScriptableObjectInstaller
        {

            protected virtual bool InjectOnlyOnce => true;

#if UNITY_EDITOR
            protected virtual void OnEnable()
            {
                if (!Application.isPlaying)
                {

                    InstallBindingsIfNeeded(StaticContext.Container);

                }
            }
#endif

            private void InstallBindingsIfNeeded(DiContainer container)
            {
                if (InjectOnlyOnce && container.HasBinding(this.GetType()))
                {
                    return;
                }
                InstallBindings(container);
                if (InjectOnlyOnce)
                {
                    var statement = container.StartBinding();
                    var bindInfo = statement.SpawnBindInfo();

                    bindInfo.ContractTypes.Add(this.GetType());



                    statement.SetFinalizer(
                        new ScopableBindingFinalizer(
                            bindInfo,
                            (container, type) => new InstanceProvider(type, this, container, bindInfo.InstantiatedCallback)));

                    new IdScopeConcreteIdArgConditionCopyNonLazyBinder(bindInfo).AsCached();
                }

            }

            public override void InstallBindings()
            {
                InstallBindingsIfNeeded(Container);
            }

            /// <summary>
            /// Override this and install any bindings to the given container.
            /// </summary>
            /// <param name="container">The container to install bindings to.</param>
            protected abstract void InstallBindings(DiContainer container);

        }
}
