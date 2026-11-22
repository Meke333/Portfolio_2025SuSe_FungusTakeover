using Game.Environment;
using Game.Interface;
using Zenject;
using Helper;
using UnityEngine;

namespace Game.Entity
{
    public class EntityEnvironment : EntitySystem, IEnvironmentDependent
    {
        [Inject] EntityBase _base;
        [Inject] GlobalProjectSettings _setting;

        EnvironmentEffectType _finalDamage;
        float _timer;
        float _speedModifier = 0;

        private void Update()
        {
            if (_timer > 0)
            {
                _timer -= MyTime.deltaTime;
            }
        }
        public bool IsAfflictable(EnvironmentEffectType effectType)
        {
            bool _result = false;

            EnvironmentEffectType entityEffect = _base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer.affectedByEnvironment;

            _finalDamage = LayerAndMaskHelper.EnumFlagAND(entityEffect, effectType);

            if (_finalDamage != 0) //if at least one bit is set, return true
                _result = true;

            return _result;
        }

        public void AfflictEffect(EnvironmentEffect effect)
        {
            IDamagable _dmg = _base.GetComponentInChildren<IDamagable>();

            //effect has both values aND the flags itself yiPPEEEE
            if (effect.environmentType.HasFlag(EnvironmentEffectType.Poison) && _timer <= 0 && _dmg.IsDamagable()) //poison
            {
                _timer = _setting.poisonCooldown;
                _dmg.Damage(effect.PoisonDamage);
                _dmg.EnvironmentBasedDamage();
            }
            if (effect.environmentType.HasFlag(EnvironmentEffectType.Pushing)) //pushing 
            {
                _dmg.SetKnockback(effect.PushDirection, effect.PushForce);
            }
            if (effect.environmentType.HasFlag(EnvironmentEffectType.Speed)) //speed
            {
                if (_speedModifier == 0) //only if there's no current speed modifier
                {
                    _speedModifier = effect.SpeedModifier;
                    _base.ResourceManager.Speed.ResourceValue += _speedModifier;
                    _base.ResourceManager.Speed.ResourceValue = Mathf.Clamp(_base.ResourceManager.Speed.ResourceValue, 0f, Mathf.Infinity);
                }
            }
        }
        public void StopEffect(EnvironmentEffect effect)
        {
            //maybe reset knockback? then i need an environment knockback variable.... will test in game if needed
            if (effect.environmentType.HasFlag(EnvironmentEffectType.Speed)) //speed
            {
                _speedModifier = 0;
                _base.ResourceManager.Speed.ResourceValue = _base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer.speedStat;
            }
        }
    }
}

