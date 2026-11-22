using Game.Entity;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using Events;

namespace Game.Camera
{
    public class TargetGroupSetter : MonoBehaviour
    {
        [Inject] private EntityManagers _entityManagers;
        [Inject] private ProjectEvents _projectEvents;

        [SerializeField] private CinemachineTargetGroup _cinemachineTargetGroup;
        [SerializeField] private float weight = 1;
        [SerializeField] private float radius = 1;

        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }


        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _projectEvents.Entity.OnAllyListChanged += SetupTargetsForCamera;
                    _projectEvents.GameFlow.GameOver += GameOverSetUp;
                    break;
                case false:
                    _projectEvents.Entity.OnAllyListChanged -= SetupTargetsForCamera;
                    _projectEvents.GameFlow.GameOver -= GameOverSetUp;
                    break;
            }
        }

        void Awake()
        {
            SetupTargetsForCamera();
        }

        private void AddTarget(EntityBase entity)
        {
            _cinemachineTargetGroup.AddMember(entity.transform, weight, radius);
            Debug.Log("yippe added");
        }
        private void RemoveTarget(EntityBase entity)
        {
            _cinemachineTargetGroup.RemoveMember(entity.transform);
        }

        private void SetupTargetsForCamera()
        {
            List<CinemachineTargetGroup.Target> newTargetGroup = new List<CinemachineTargetGroup.Target>();

            foreach (EntityBase charBase in _entityManagers.allyManager.AlliesList)
            {
                CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
                target.Object = charBase.transform;
                target.Radius = radius;
                target.Weight = weight;
                newTargetGroup.Add(target);
            }

            _cinemachineTargetGroup.Targets = newTargetGroup;
        }
        private void SetupTargetsForCamera(List<EntityBase> _entityList)
        {
            List<CinemachineTargetGroup.Target> newTargetGroup = new List<CinemachineTargetGroup.Target>();

            foreach (EntityBase charBase in _entityList)
            {
                CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
                target.Object = charBase.transform;
                target.Radius = radius;
                target.Weight = weight;
                newTargetGroup.Add(target);
            }

            _cinemachineTargetGroup.Targets = newTargetGroup;
        }
        
        private void GameOverSetUp()
        {
            _cinemachineTargetGroup.AddMember(_entityManagers.allyManager.followPlayerTransform.transform, weight, radius);
        }
    }
}

