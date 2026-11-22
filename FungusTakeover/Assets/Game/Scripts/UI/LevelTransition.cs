using System;
using Events;
using UnityEngine;
using Zenject;

public class LevelTransition : MonoBehaviour
{
    [Inject] private ProjectEvents _projectEvents;
    
    [SerializeField] private Animation _animation;

    #region  UnityMethods

    private void OnEnable()
    {
        ToggleEvents(true);
    }

    private void OnDisable()
    {
        ToggleEvents(false);
    }

    #endregion

    private void ToggleEvents(bool shouldSubscribe)
    {
        switch (shouldSubscribe)
        {
            case true:
                _projectEvents.Dungeon.StartRoomTransition += StartAnimation;
                break;
            case false:
                _projectEvents.Dungeon.StartRoomTransition -= StartAnimation;
                break;
        }
    }

    private void StartAnimation()
    {
        _animation.Play(PlayMode.StopAll);
    }
}
