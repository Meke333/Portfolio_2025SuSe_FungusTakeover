using System;
using System.Collections;
using Events;
using Game.Entity;
using Interface;
using MoreMountains.Feedbacks;
using UnityEngine;
using Zenject;


namespace Game.Dungeon
{
    //This depends on how we will do it, but for MVP we only need one room
    public class DungeonTeleporter : MonoBehaviour
    {
        //Look at Prototype Teleporter
        [Inject] private ProjectEvents _projectEvents;
        [Inject] private EntityManagers _entityManagers;
        
        [SerializeField] private DungeonRoom _nextDungeonRoom;
        [SerializeField] private MMFeedbacks _feedback;

        private Collider[] colliderList;
        
        private bool _isOpen;

        //Dissolve Shader properties
        private float dissolveRate = 0.0125f;
        private float refreshRate = 0.025f;

        [SerializeField] private Renderer _renderer;

        #region UnityMethods

        private void FixedUpdate()
        {
            if (!_isOpen)
                return;

            EvaluateCollider();
        }

        #endregion
        
        public void OpenDoor()
        {
            //Enable Collision 
            _isOpen = true;

            //LIAM PUT HERE YOUR DOOR OPEN VFX
            _feedback?.PlayFeedbacks();
            StartCoroutine(DissolveCo());
        }

        private void EvaluateCollider()
        {
            colliderList = ColliderHelper.Get3DColliderArrayForSpecificType(this.transform.position,
                this.transform.localScale.x, EntityContextType.Player);

            if (colliderList == null || colliderList.Length == 0)
                return;

            SendPlayerToDungeonRoom();
            _isOpen = false;
        }

        public void SendPlayerToDungeonRoom()
        {
            _projectEvents.Dungeon.MovedIntoNextRoom?.Invoke(_nextDungeonRoom);
        }

        IEnumerator DissolveCo()
        {
            float counter = 0;
            while (_renderer.material.GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                _renderer.material.SetFloat("_DissolveAmount", counter);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}

