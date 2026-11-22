using Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.VFX;
using System;
using System.Collections;
using Zenject;

namespace Game.Entity
{

    public class EntityVFX : EntitySystem
    {
        [Inject] EntityResourceManager _resource;
        [Inject] private ProjectEvents _projectEvents;
        [Inject] EntityAllStatsConfiguration _stats;

        [Header("Common VFX")]
        [SerializeField] private MMFeedbacks attackImpactFeedback;
        [SerializeField] private MMFeedbacks stunFeedback;
        [SerializeField] private MMFeedbacks overtakingFeedback;

        [Header("Player VFX")]
        [SerializeField] private MMFeedbacks playerAttackFeedback;
        [SerializeField] private MMFeedbacks sporeFeedback; //not in lol
        [SerializeField] private MMFeedbacks eatAlliesFeedback;
        [SerializeField] private MMFeedbacks callingAlliesFeedback;

        [Header("Enemy VFX")]
        [SerializeField] private MMFeedbacks enemyAttackFeedback;
        [SerializeField] private GameObject enemyAttackVFXParent;

        [Header("Ally VFX")]
        [SerializeField] private MMFeedbacks allyAttackFeedback;
        [SerializeField] private GameObject allyAttackVFXParent;

        private bool _RunDuringWindup = false;

        private void Start()
        {
            ContextChanged(_resource.EntityContext.ResourceValue);
        }
        private void OnEnable()
        {
            ToggleEvents(true);
        }
        private void OnDisable()
        {
            ToggleEvents(false);
        }

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += AttackImpact;
                    _events.ResourceOutput.State += StateChanged;
                    _events.ResourceOutput.EntityContext += ContextChanged;
                    _projectEvents.Entity.PlayerHealthChanged += GivePlayerHealth;
                    _events.EntityUnit.TransmitAttackData += AnimateAttack;
                    _events.EntityUnit.TriggerWindup += StartAttackActionWINDUP;
                    _events.EntityUnit.TriggerAttack += StartAttackAction;
                    _events.EntityUnit.FinishTransmittingAttackData += StopAttackAction;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= AttackImpact;
                    _events.ResourceOutput.State -= StateChanged;
                    _events.ResourceOutput.EntityContext -= ContextChanged;
                    _projectEvents.Entity.PlayerHealthChanged -= GivePlayerHealth;
                    _events.EntityUnit.TransmitAttackData -= AnimateAttack;
                    _events.EntityUnit.TriggerWindup -= StartAttackActionWINDUP;
                    _events.EntityUnit.TriggerAttack -= StartAttackAction;
                    _events.EntityUnit.FinishTransmittingAttackData -= StopAttackAction;
                    break;
            }
        }

        private void AttackImpact()
        {
            attackImpactFeedback?.PlayFeedbacks();
        }
        private void StateChanged(EntityStateType type)
        {
            switch (type)
            {
                case EntityStateType.Dead:
                    HandleAttack(false);
                    break;
                case EntityStateType.Stun:
                    stunFeedback?.PlayFeedbacks();
                    break;
                case EntityStateType.Formation:
                    callingAlliesFeedback?.PlayFeedbacks();
                    break;
            }
        }
        private void ContextChanged(EntityContextType type)
        {
            if (type == 0)
                throw new Exception("EntityContextType is null in EntityVFX");

            if (type == EntityContextType.Ally)
                overtakingFeedback?.PlayFeedbacks();

            //Check specifically for everything but wasp and windup hacky bugfix pls end me
            float _windUpTimer = _stats.GetStats(type).EntityStatContainer.attackWindUp;
            if (_windUpTimer > 0 && _stats.Enemy.EntityStatContainer.attackBehaviourType == EntityAttackType.Default)
                _RunDuringWindup = true;
            else
                _RunDuringWindup = false;
        }

        private void GivePlayerHealth(float obj)
        {
            eatAlliesFeedback?.PlayFeedbacks();
        }

        private void AnimateAttack(Vector3 _pos)
        {
            if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
            {
                if (enemyAttackVFXParent == null)
                    return;
                enemyAttackVFXParent.transform.position = new Vector3(_pos.x,enemyAttackVFXParent.transform.position.y, _pos.z);
            }
            else //player or ally
            {
                if (allyAttackVFXParent == null)
                    return;
                allyAttackVFXParent.transform.position = new Vector3(_pos.x, allyAttackVFXParent.transform.position.y, _pos.z);
            }
        }
        
        private void StartAttackAction() {
            if (!_RunDuringWindup)
                HandleAttack(true); 
        }
        private void StartAttackActionWINDUP() { 
            if (_RunDuringWindup)
                HandleAttack(true); 
        }
        private void StopAttackAction() { HandleAttack(false); }
        private void HandleAttack(bool _shouldStart)
        {
            if (_shouldStart)
                StartAttack();
            else 
                StopAttack();
        }
        private void StartAttack()
        {
            
            if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
            {
                //check if it's not an animated attack
                if (_stats.Enemy.EntityStatContainer.attackBehaviourType == EntityAttackType.Default)
                {
                    enemyAttackFeedback?.PlayFeedbacks();
                    return;
                }
                if (enemyAttackVFXParent == null)
                    throw new System.NullReferenceException("Attack isn't referenced despite being an animated attack type");

                enemyAttackVFXParent.gameObject.SetActive(true);
                
                enemyAttackFeedback?.PlayFeedbacks();

                enemyAttackVFXParent.transform.parent = null;
            }
            else //player or ally
            {
                //check if it's not an animated attack
                if (_stats.Enemy.EntityStatContainer.attackBehaviourType == EntityAttackType.Default)
                {
                    if (_resource.EntityContext.ResourceValue == EntityContextType.Player)
                        playerAttackFeedback?.PlayFeedbacks();
                    else 
                        allyAttackFeedback?.PlayFeedbacks();
                    return;
                }
                if (allyAttackVFXParent == null)
                    return;

                allyAttackVFXParent.gameObject.SetActive(true);

                if (_resource.EntityContext.ResourceValue == EntityContextType.Player)
                    playerAttackFeedback?.PlayFeedbacks();
                if (_resource.EntityContext.ResourceValue == EntityContextType.Ally)
                    allyAttackFeedback?.PlayFeedbacks();

                allyAttackVFXParent.transform.parent = null;
            }
        }
        private void StopAttack()
        {

            if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
            {
                if (enemyAttackVFXParent == null)
                    return;

                enemyAttackVFXParent.transform.parent = enemyAttackFeedback.transform;
                enemyAttackVFXParent.gameObject.SetActive(false);
            }
            else //player or ally
            {
                if (allyAttackVFXParent == null)
                    return;

                allyAttackVFXParent.transform.parent = allyAttackFeedback.transform;
                allyAttackVFXParent.gameObject.SetActive(false);
            }
        }

    }
}

