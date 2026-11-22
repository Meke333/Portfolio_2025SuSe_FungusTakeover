using System;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{


    public override void InstallBindings()
    {
        Container.Bind<SceneEvents>().AsSingle();
    }
}
