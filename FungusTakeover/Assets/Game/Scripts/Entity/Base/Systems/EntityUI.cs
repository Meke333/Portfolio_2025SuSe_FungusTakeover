using Game.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using Zenject;
using TMPro;

namespace Game.Entity
{
    public class EntityUI : EntitySystem
    {
        [Inject] private GameUIHolder _gameUIHolder;
        [Inject] protected EntityResourceManager _resource;
        [Inject] protected EntityAllStatsConfiguration _allStatsConfig;

        [Header("HealthBarUI")]
        [SerializeField] private MMHealthBar healthBar;
        [SerializeField] private MMHealthBar playerHealthBar;
        [SerializeField] private MMHealthBar allyHealthBar;

        //[Header("Changing Enemy to Ally HealthBar")]
        //[SerializeField] private GameObject enemyProgressBar;
        //[SerializeField] private GameObject allyProgressBar;

        [Header("DamageUI")]
        [SerializeField] private MMFeedbacks playerDamageUI;

        [Header("CallAlliesUI")]
        [SerializeField] private MMFeedbacks world_CallAllyUI;
        [SerializeField] private MMFeedbacks screenSpace_CallAllyUI;

        [Header("SporeCostDeadEnemyUI")]
        [SerializeField] private MMFeedbacks sporeCostEnemyUI;

        [SerializeField] private TextMeshProUGUI sporeCostText;

        [Header("EnemyNearlyDeadUI")]
        [SerializeField] private MMFeedbacks enemyNearlyDeadUI;

        [Header("CameraFeedbacks")]
        [SerializeField] private MMFeedbacks cameraShakeFeedback;

        private MMHealthBar enemyHealthBar;
        
        private bool isNearlyDeadAlreadyPlayed = false;


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
                    _events.EntityUnit.OnTakenEnvironmentDamage += AttackImpact;
                    _events.ResourceOutput.HealthPoints += HealthChange;
                    _events.ResourceOutput.EntityContext += ContextChanged;
                    _events.ResourceOutput.State += StateChanged;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= AttackImpact;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= AttackImpact;
                    _events.ResourceOutput.HealthPoints -= HealthChange;
                    _events.ResourceOutput.EntityContext -= ContextChanged;
                    _events.ResourceOutput.State -= StateChanged;
                    break;
            }
        }
        

        private void HealthChange(float value)
        {
            if (value <= 0)
                return;

            switch (_resource.EntityContext.ResourceValue)
            {
                case EntityContextType.Player:
                    if (_gameUIHolder == null || _gameUIHolder.healthBarCallIcon == null)
                        break;
                    if (_resource == null || _resource.Health == null || _resource.EntityContext == null)
                        break; 
                    
                    _gameUIHolder.healthBarCallIcon?.UpdateHealthBars(_resource.Health.ResourceValue, .0f,
                            _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer
                                .healthPoints, true);

                    break;
                case EntityContextType.Ally:
                    allyHealthBar?.UpdateBar(_resource.Health.ResourceValue, .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);
                    break;
                case EntityContextType.Enemy:
                    return;
            }
            
        }

        private void AttackImpact()
        {
            if (_resource.EntityContext.ResourceValue == EntityContextType.Player)
            {
                _gameUIHolder.healthBarCallIcon.UpdateHealthBars(Mathf.Max(0, _resource.Health.ResourceValue), .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);

                //healthBar?.UpdateBar(_resource.Health.ResourceValue, .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);
                //playerHealthBar?.UpdateBar(_resource.Health.ResourceValue, .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);
                playerDamageUI?.PlayFeedbacks();
            }
            else if (_resource.EntityContext.ResourceValue == EntityContextType.Enemy)
            {
                healthBar?.UpdateBar(_resource.Health.ResourceValue, .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);


                if (!isNearlyDeadAlreadyPlayed && 1 - (_resource.Health.ResourceValue / _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints) >= 0.6f && 1 - (_resource.Health.ResourceValue / _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints) < 1f)
                {
                    isNearlyDeadAlreadyPlayed = true;
                    enemyNearlyDeadUI?.PlayFeedbacks();
                }
            }
            else
            {
                allyHealthBar?.UpdateBar(_resource.Health.ResourceValue, .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);
                cameraShakeFeedback?.PlayFeedbacks();
            }
        }

        private void StateChanged(EntityStateType type)
        {
            switch (type)
            {
                case EntityStateType.Formation:
                    
                    //world_CallAllyUI?.PlayFeedbacks();
                    //screenSpace_CallAllyUI?.PlayFeedbacks();
                    _gameUIHolder.healthBarCallIcon.TriggerAllyCalling();
                    break;

                case EntityStateType.Dead:
                    //Debug.Log("EntityDead: " + _allStatsConfig.Enemy.EntityStatContainer.sporeCost);
                    if (_allStatsConfig.Enemy.EntityStatContainer.sporeCost > 1)
                    {
                        sporeCostEnemyUI?.gameObject.SetActive(true);
                        sporeCostText.gameObject.SetActive(true);
                        sporeCostEnemyUI?.PlayFeedbacks();
                    }
                    
                    enemyNearlyDeadUI?.ResumeFeedbacks();
                    
                    //set spore cost text
                    //activate if it needs more than one spore
                    if (sporeCostText != null)
                        sporeCostText.text = _allStatsConfig.Enemy.EntityStatContainer.sporeCost.ToString();
                    
                    break;

            }
        }

        private void ContextChanged(EntityContextType type)
        {

            switch (type)
            {
                case EntityContextType.Player:
                    healthBar?.gameObject.SetActive(false);
                    allyHealthBar?.gameObject.SetActive(false);
                    if (_gameUIHolder == null || _gameUIHolder.healthBarCallIcon == null)
                        break;
                    if (_resource == null || _resource.Health == null || _resource.EntityContext == null)
                        break; 
                    
                    _gameUIHolder.healthBarCallIcon?.SwitchOutPlayer(_allStatsConfig.Player.PlayerValueContainer.playerActions.HasFlag(PlayerActions.All));
                    _gameUIHolder.healthBarCallIcon?.UpdateHealthBars(Mathf.Max(0, _resource.Health.ResourceValue), .0f, _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints, true);
                    break;
                case EntityContextType.Ally:
                    //enemyProgressBar.SetActive(false);
                    //allyProgressBar.SetActive(true);
                    if (sporeCostEnemyUI.IsPlaying)
                        sporeCostEnemyUI?.ResumeFeedbacks();
                    if (enemyNearlyDeadUI.IsPlaying)
                        enemyNearlyDeadUI?.ResumeFeedbacks();
                
                    //healthBar = allyHealthBar;
                    break;
            }
                
        }

    }
}

