using System;
using Game.Entity.Events;
using Game.Interface;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    /// <summary>
    /// If the Resource Management of the AllyManager allows to use up Spores
    /// Then this script should evaluate a general Hitbox if enemies are hit and can turn into allies
    /// is done in the AllyManager!!! (TO-DO!!!)
    /// </summary>
    public class PlayerTouchSpores : PlayerSystem
    {
        [Inject] private EntityAllStatsConfiguration _allStatsConfigurations;
        [SerializeField] private Transform _sporeHitBox;
        


        Collider[] colliderList;


        bool canUseSpores;

        #region UnityMethods

        private void Start()
        {
            ToggleActivity(_base.ContextType);
        }

        private void FixedUpdate()
        {
            if (!canUseSpores)
                return;

            EvaluateCollider();
        }


        #endregion

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            
        }

        protected override void ToggleActivity(EntityContextType type)
        {
            _isActive = (type == EntityContextType.Player || type == EntityContextType.Ally);
            canUseSpores = _isActive;

        }

        /*private void CheckOnEntityContext(EntityContextType type)
        /{
            
            if (type == 0)
                throw new System.NullReferenceException("Type is null in touch spores.");

            canUseSpores = (type == EntityContextType.Player || type == EntityContextType.Ally);
        
            _sporeHitBox.gameObject.SetActive(canUseSpores);
        }
        */
        
        private void EvaluateCollider()
        {
            colliderList = ColliderHelper.Get3DColliderArray(this.transform.position, _allStatsConfigurations.Player.PlayerValueContainer.sporeRange, EntityContextType.Player);

            if (colliderList == null || colliderList.Length == 0)
                return;

            for (int i = 0; i < colliderList.Length; i++)
            {
                ISporeable temp = ColliderHelper.GetComponent<ISporeable>(colliderList[i]);

                if (temp == null)
                    continue;

                if (!temp.IsSporeable())
                    continue;

                temp.Spore();
            }
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                ColliderHelper.DrawGizmos(this.transform.position, _allStatsConfigurations.Player.PlayerValueContainer.sporeRange);
            }
        }

    }

}
