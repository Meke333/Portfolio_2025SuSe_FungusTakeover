using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {

        Container.Bind<TestBase>().To<TestDeriveOne>().AsSingle();
    }
}

public abstract class TestBase
{
    public virtual void A()
    {
        Debug.Log("A");
    }
}

public class TestDeriveOne : TestBase
{
    public override void A()
    {
        Debug.Log("AAAAAAAA");
    }
}

public class TestDeriveTwo : TestBase
{

}
