using Sirenix.OdinInspector;
using UnityEngine;

using Zenject;

namespace Game.Entity
{
    public class EntityAnimator : EntitySystem
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimatorOverrideController _allyAnimator;

        [Inject] EntityResourceManager _resource;
        [Inject] EntityBase _base;

        #region UnityMethods

        private void OnEnable()
        {
            _events.Animator.ChangeBoolParameter += SetParameter;
            _events.Animator.ChangeIntParameter += SetParameter;
            _events.Animator.ChangeFloatParameter += SetParameter;
            _events.Animator.TriggerParameter += SetTrigger;
            _events.Animator.PlayAnimationImmediately += PlayAnimation;
            ToggleEvents(true);
        }
        private void Update()
        {
            if (_base.ResourceManager.EntityContext.ResourceValue == EntityContextType.Enemy)
            {
                if (_base.ResourceManager.StateType.ResourceValue == EntityStateType.Idle || _base.ResourceManager.StateType.ResourceValue == EntityStateType.Alert)
                    MovementEnemy();
            }
        }
        private void OnDisable()
        {
            _events.Animator.ChangeBoolParameter -= SetParameter;
            _events.Animator.ChangeIntParameter -= SetParameter;
            _events.Animator.ChangeFloatParameter -= SetParameter;
            _events.Animator.TriggerParameter -= SetTrigger;
            _events.Animator.PlayAnimationImmediately -= PlayAnimation;
            ToggleEvents(false);
        }

        #endregion

        #region Methods
        private void SetParameter(string name, float value)
        {
            _animator.SetFloat(name, value);
        }

        private void SetParameter(string name, int value)
        {
            _animator.SetInteger(name, value);
        }

        private void SetParameter(string name, bool value)
        {
            _animator.SetBool(name, value);
        }

        private void SetTrigger(string name)
        {
            _animator.SetTrigger(name);
        }

        private void PlayAnimation(string name, int layer = 0)
        {
            _animator.Play(name, layer, 0);
        }
        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += AttackImpact;
                    //_events.EntityUnit.TriggerAttack += StartAttack;
                    _events.ResourceOutput.State += StateChanged;
                    _events.ResourceOutput.EntityContext += ContextChanged;
                    _events.InputActions.ConsumeAllies += EatAllies;
                    _events.InputActions.Moving += Movement;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= AttackImpact;
                    //_events.EntityUnit.TriggerAttack -= StartAttack;
                    _events.ResourceOutput.State -= StateChanged;
                    _events.ResourceOutput.EntityContext -= ContextChanged;
                    _events.InputActions.ConsumeAllies -= EatAllies;
                    _events.InputActions.Moving -= Movement;
                    break;
            }
        }

        private void AttackImpact()
        {
            SetTrigger("isTakingDamage");
        }
        private void StartAttack() { SetTrigger("isAttack"); }
        private void StateChanged(EntityStateType type)
        {
            switch (type)
            {
                case EntityStateType.Formation:
                    //if (_resource.EntityContext.ResourceValue == EntityContextType.Player)
                        SetTrigger("isCallingAllies");
                    /*if (_resource.EntityContext.ResourceValue == EntityContextType.Ally)
                        SetParameter("Movement", 1.0f);*/
                    break;
                case EntityStateType.Dead:
                    SetTrigger("isTakingDamage");
                    SetTrigger("isDead");
                    _animator.runtimeAnimatorController = _allyAnimator;
                    break;
                case EntityStateType.Attack:
                    StartAttack();
                    break;
                    //moved to update
                    /*case EntityStateType.Idle:
                        if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
                            MovementEnemy();
                        break;
                    case EntityStateType.Alert:
                        if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
                            MovementEnemy();
                        break;*/
            }
        }
        private void ContextChanged(EntityContextType type)
        {
            if (type == EntityContextType.Ally) { 
                SetTrigger("isTakenOver");
                _animator.ResetTrigger("isTakingDamage");
            }
        }
        private void EatAllies(bool input)
        {
            if (input)
                SetTrigger("isEatingAllies");
        }
        private void Movement(Vector2 input)
        {
            if (input.x < 0)
                input.x *= -1;
            if (input.y < 0)
                input.y *= -1;
            SetParameter("Movement", Mathf.Max(input.x, input.y));
        }
        private void MovementEnemy()
        {
            SetParameter("Movement", _base.navMeshAgent.velocity.magnitude);
        }
            
    }
}

