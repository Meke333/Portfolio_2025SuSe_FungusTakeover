using UnityEngine;
using UnityEngine.AI;

namespace Game.Entity
{
    /// <summary>
    /// Base for EntityMovement Brains
    /// IMPORTANT: Class must be the same name as the enum that holds this movement
    /// e.g.: Player => Game.Entity.EntityMovement_Player
    /// </summary>    
    public abstract class EntityMovement_Base
    {
        protected EntityStatContainer _entityStatContainer;
        protected EntityResourceManager _resources;
        protected EntityBase _base;
        protected GlobalProjectSettings _projectSettings;

        Vector3 _controlKnockback = Vector3.zero;
        float _timer = 0.0f;
        AnimationCurve knockbackCurve;
        bool _knockbackActive = false;

        public void Initialize(EntityResourceManager resource, EntityBase entity, GlobalProjectSettings settings)
        {
            _resources = resource;
            _base = entity;
            _projectSettings = settings;

            GrabKnockbackStats();
        }
        public bool CheckForDeath()
        {
            /*Exit if already dead*/
            return _resources.StateType.ResourceValue == EntityStateType.Dead;
        }
        public void GrabKnockbackStats()
        {
            /*Grabs knockback stats*/
            _entityStatContainer = _base.StatsConfiguration.GetStats(_base.ContextType).EntityStatContainer;
            knockbackCurve = _entityStatContainer.useEntityKnockbackSpeed
                ? _entityStatContainer.knockbackSpeed
                : _projectSettings.knockbackSpeed;
        }
        public void SetupKnockback()
        {
            /*Start knockback when new value is set externally*/
            if (_controlKnockback == Vector3.zero && _resources.Knockback.ResourceValue != Vector3.zero)
            {
                _controlKnockback = _resources.Knockback.ResourceValue;
                _timer = 0f;
                _knockbackActive = true;
            }

            /*Reset knockback if forcibly cleared externally (e.g. interrupt or overwrite)*/
            if (_controlKnockback != Vector3.zero && _controlKnockback != _resources.Knockback.ResourceValue)
            {
                _controlKnockback = _resources.Knockback.ResourceValue;
                _resources.KnockbackTimer.ResourceValue = 0.0f;
                _timer = 0.0f;
                _knockbackActive = true;
            }
        }
        public void ProcessKnockback()
        {
            /*Process knockback only while active*/
            if (_knockbackActive)
            {
                // Apply knockback based on curve
                _controlKnockback = NavMeshAgentHelper.Knockback(
                    _base.transform,
                    _controlKnockback,
                    knockbackCurve,
                    _resources.KnockbackTimer.ResourceValue);

                _resources.Knockback.ResourceValue = _controlKnockback;

                // Advance timer
                _timer += MyTime.deltaTime;

                if (_timer >= _projectSettings.KnockbackGlobalTime)
                {
                    _timer = 0f;
                    _controlKnockback = Vector3.zero;
                    _resources.Knockback.ResourceValue = Vector3.zero;
                    _knockbackActive = false;
                }

                _resources.KnockbackTimer.ResourceValue = Mathf.Clamp01(_timer / _projectSettings.KnockbackGlobalTime);
            }
            else
            {
                _resources.KnockbackTimer.ResourceValue = 0f;
            }
        }
        public abstract void ProcessMovement();

    }

}

