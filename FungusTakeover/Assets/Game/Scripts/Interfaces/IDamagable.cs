using Game.Entity;
using UnityEngine;

namespace Game.Interface
{
    public interface IDamagable
    {
        public bool IsDamagable();
        public void Damage(float damage);
        public void SetKnockback(Vector3 direction, float knockback);
        public void SetStunActions(CharActions actions, float duration);

        public void EntityBasedDamage();
        public void EnvironmentBasedDamage();
    }

}

