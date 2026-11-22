using System;
using Game.Interface;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Game.Entity.Events;

namespace Game.Entity
{
    public class EntityAttack_Animated : EntityAttack_Base
    {
        [Inject] private EntityResourceManager _resources;
        [Inject] private EntityBase _base;
        [Inject] private EntityEvents _entityEvents;
        public override bool AnimatedCollider //overwrite this so you can set the animation status
        {
            get => _animatedCollider;
            set => _animatedCollider = true;
        }
        private List<Collider> _evaluatedColliders = new List<Collider>();

        
        //<summary> In animated attacks, this starts the values</summary>
        public override void ProcessAttack()
        {
            _evaluatedColliders.Clear();
            _timer = _entityStats.EntityStatContainer.attackThrustSeconds;
            _rangePerSecond = _entityStats.EntityStatContainer.attackThrustRange / _timer;

            if (_collider != null)
            {
                _animatedPosition = _collider.transform.position;
                _animatedDirection = _collider.transform.forward;
            }
            else
            {
                _animatedPosition = _base.transform.position;
                _animatedDirection = _base.transform.forward;
            }
        }
        //<summary> In animated attacks, this collects all hit colliders once and attacks once on its path</summary>
        public override void ProcessAnimatedAttack()
        {
            _animatedPosition += _animatedDirection * _rangePerSecond * MyTime.deltaTime;
            TransmitAttackPosition();

            Collider[] colliderList = ColliderHelper.Get3DColliderArray(_collider,_animatedPosition,
                    _entityStats.EntityStatContainer.attackRange,
                    _contextType);


            if (colliderList == null || colliderList.Length == 0)
                return;


            for (int i = 0; i < colliderList.Length; i++)
            {
                //Check if it's already been hit if yes just skip it
                if (_evaluatedColliders.Contains(colliderList[i]))
                    continue;

                IDamagable temp = ColliderHelper.GetComponent<IDamagable>(colliderList[i]);

                if (temp == null)
                    continue;
                if (!temp.IsDamagable()) //don't hit it if it can't be hit
                    continue;

                //add to hit colliders
                _evaluatedColliders.Add(colliderList[i]);

                Vector3 knockbackDirection = (colliderList[i].transform.position - _animatedPosition).normalized;

                float knockback = _base.StatsConfiguration.Player.EntityStatContainer.knockbackForce;
                float damage = _resources.Attack.ResourceValue;

                temp.SetKnockback(knockbackDirection, knockback);
                temp.Damage(damage);
                temp.SetStunActions(_entityStats.EntityStatContainer.stunPossibleActions, _entityStats.EntityStatContainer.stunDuration);
                temp.EntityBasedDamage();
            }
        }
        private void TransmitAttackPosition()
        {
            _entityEvents.EntityUnit.TransmitAttackData?.Invoke(_animatedPosition);
        }
        public override void DrawGizmos()
        {
            if (_collider == null)
                ColliderHelper.DrawGizmos(_animatedPosition, _entityStats.EntityStatContainer.attackRange);
            else
                ColliderHelper.DrawGizmos(_collider, _animatedPosition);
        }
    }
}

