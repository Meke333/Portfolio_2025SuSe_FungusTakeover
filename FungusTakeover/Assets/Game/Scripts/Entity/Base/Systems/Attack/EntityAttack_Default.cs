using System;
using Game.Interface;
using UnityEngine;
using Zenject;

namespace Game.Entity
{
    public class EntityAttack_Default : EntityAttack_Base
    {
        [Inject] private EntityResourceManager _resources;
        [Inject] private EntityBase _base;

        public override void ProcessAttack()
        {
            
            Collider[] colliderList;
            if (_collider != null)
            {
                _animatedPosition = _collider.transform.position;
                colliderList = ColliderHelper.Get3DColliderArray(_collider, _contextType);
            }
            else
            {
                _animatedPosition = _base.transform.position;
                colliderList = ColliderHelper.Get3DColliderArray(_base.transform.position,
                    _entityStats.EntityStatContainer.attackRange,
                    _contextType);
            }

            if (colliderList == null || colliderList.Length == 0)
                return;

            EntityStatContainer entityStatContainer = _base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer;
            float knockback = entityStatContainer.knockbackForce; 
            float damage = _resources.Attack.ResourceValue;

            for (int i = 0; i < colliderList.Length; i++)
            {

                IDamagable temp = ColliderHelper.GetComponent<IDamagable>(colliderList[i]);
                if (temp == null)
                    continue;
                if (!temp.IsDamagable())
                    continue;

                Vector3 knockbackDirection = (colliderList[i].transform.position - _animatedPosition).normalized;
                
                temp.SetKnockback(knockbackDirection, knockback);
                temp.Damage(damage);
                temp.SetStunActions(_entityStats.EntityStatContainer.stunPossibleActions, _entityStats.EntityStatContainer.stunDuration);
                temp.EntityBasedDamage();
            }
        }

        public override void ProcessAnimatedAttack()
        {
            Debug.Log("Default attack doesn't have collider animation, please use Animated if you meant to have it modify its position");
        }
        public override void DrawGizmos()
        {
            if (_collider == null)
                ColliderHelper.DrawGizmos(_base.transform.position, _entityStats.EntityStatContainer.attackRange);
            else
            {
                Gizmos.matrix = _collider.transform.localToWorldMatrix;
                ColliderHelper.DrawGizmos(_collider);
            }
        }
    }
}

