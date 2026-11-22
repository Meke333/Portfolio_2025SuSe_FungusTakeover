using Game.Interface;
using UnityEngine;
using Zenject;
using Helper;

namespace Game.Entity
{
    public class EntityDamage : EntitySystem, IDamagable
    {
        [Inject] private EntityBase _base;

        public bool IsDamagable()
        {
            bool result = true;

            if (_base.ResourceManager.Health.ResourceValue <= 0)
                result = false;
            if (_base._allyFormation.isInFormation)
                result = false;

            return result;
        }

        public void Damage(float damage)
        {
            _base.ResourceManager.Health.ResourceValue -= damage;
        }

        public void SetKnockback(Vector3 direction, float knockback)
        {
            Vector3 finalKnockback = VectorHelper.AddForceToVector(_base.ResourceManager.Knockback.ResourceValue, direction, knockback * 100);
            _base.ResourceManager.Knockback.ResourceValue = finalKnockback;
        }

        public void SetStunActions(CharActions actions, float stunDuration)
        {
            CharActions combinedActions = LayerAndMaskHelper.EnumFlagAND(_base.ResourceManager.StunAction.ResourceValue, actions);
            _base.ResourceManager.StunAction.ResourceValue = combinedActions;
            if (stunDuration > _base.ResourceManager.StunTimer.ResourceValue)
                _base.ResourceManager.StunTimer.ResourceValue = stunDuration;
        }

        public void EntityBasedDamage() { _events.EntityUnit.OnTakenEntityDamage?.Invoke(); }
        public void EnvironmentBasedDamage() { _events.EntityUnit.OnTakenEnvironmentDamage?.Invoke(); }
    }
}

