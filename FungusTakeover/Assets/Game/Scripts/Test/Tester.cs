
using System;
using Game.Entity;
using Game.Entity.Events;
using UnityEngine;
using Zenject;

public class Tester : MonoBehaviour
{
    [Inject] private EntityEvents _events;
    [Inject] private EntityResourceManager _manager;


    private Action actionA;
    private Action actionB;
    private Action actionC;

    public float A;

    public int aAmount;
    public int bAmount;
    public int cAmount;

    

    void Awake()
    {
        _events.ResourceOutput.StunTimer += Test;
        
        
        actionA += TestA;
        actionB += TestB;
        actionC += TestC;
        
        Debug.Log("hash A: " + (actionA == null) + actionA?.GetHashCode());
        Debug.Log("hash B: " + (actionB == null) + actionB?.GetHashCode());
        Debug.Log("hash C: " + (actionC == null) + actionC?.GetHashCode());
        

        actionB = actionA;
        actionC = actionB;
        
        Debug.Log("hash A: " + (actionA == null) + actionA?.GetHashCode());
        Debug.Log("hash B: " + (actionB == null) + actionB?.GetHashCode());
        Debug.Log("hash C: " + (actionC == null) + actionC?.GetHashCode());

        actionA -= TestA;
        actionB -= TestB;
        actionC -= TestC;
    }

    void OnEnable()
    {
        actionA += TestA;
        actionB += TestB;
        actionC += TestC;

    }

    void Start()
    {
        actionA?.Invoke();
        actionB?.Invoke();
        actionC?.Invoke();
    }

    private void Update()
    {
        //_manager.StunTimer.ResourceValue += 0.1f;
        _events.ResourceInput.StunTimer?.Invoke(5f);
    }

    private void Test(float a)
    {
        A = a;
    }

    void TestA()
    {
        aAmount++;
    }

    void TestB()
    {
        bAmount++;
    }
    void TestC()
    {
        cAmount++;
    }

}
