using Game.Entity.Events;
using Game.Interface;
using System;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    /// <summary>
    /// This class triggers Absorption of Allies with Hitboxes
    /// </summary>
    public class PlayerAbsorbingAllies : PlayerSystem
    {

        [Inject] private EntityAllStatsConfiguration _allStatsConfigurations;

        [SerializeField] private Transform _hitboxParent;
        [SerializeField] private Transform _attackHitbox;

        #region Unity Methods


        private void OnDrawGizmos()
        {
            if (!_isActive)
                return;

            if (Application.isPlaying)
            {
                Gizmos.color = Color.blue;
                ColliderHelper.DrawGizmos(this.transform.position, _allStatsConfigurations.Player.EntityStatContainer.attackRange);
            }
        }
        #endregion

        protected override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.InputActions.ConsumeAllies += ConsumeAllies;
                    break;
                case false:
                    _events.InputActions.ConsumeAllies -= ConsumeAllies;
                    break;
            }
        }

        private void ConsumeAllies(bool isPressed)
        {
            if (!isPressed)
                return;

            EvaluateCollider();
        }

        private void EvaluateCollider()
        {
            Collider[] colliderList = ColliderHelper.Get3DColliderArrayForSpecificType(_attackHitbox.position, _hitboxParent.localScale.z, EntityContextType.Ally);

            if (colliderList == null || colliderList.Length == 0)
                return;

            bool doesAbsorb = false;
            
            for (int i = 0; i < colliderList.Length; i++)
            {
                IAbsorbable temp = ColliderHelper.GetComponent<IAbsorbable>(colliderList[i]);

                if (temp == null)
                    continue;

                if (!temp.IsAbsorbable())
                    continue;

                
                temp.Absorb();
                doesAbsorb = true;
            }

            if (doesAbsorb)
            {  
                _events.EntityUnit.EatAlly?.Invoke();
            }

        }
    }

}
