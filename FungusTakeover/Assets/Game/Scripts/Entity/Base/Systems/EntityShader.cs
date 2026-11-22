using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.CullingGroup;

namespace Game.Entity
{
    public class EntityShader : EntitySystem
    {
        [Inject] protected EntityResourceManager _resource;
        [Inject] protected EntityAllStatsConfiguration _allStatsConfig;
        [Inject] private EntityMaterialsConfiguration _materials;

        [InfoBox("Insert SkinnedMeshRenderer from the Graphics", InfoMessageType.Info)]
        [SerializeField] private SkinnedMeshRenderer _renderer; //Expand this if more parts are required

        [Header("Material Change with Feel Package")]
        [SerializeField] private MMFeedbacks materialChangeFeedback;

        [Header("Dissolve Shader on Ally Death")]
        [SerializeField] private float dissolveRate = 0.0125f;
        [SerializeField] private float refreshRate = 0.025f;

        [Header("MushroomGrowShader")]
        [SerializeField] private float mushroomAmount = 5;
        [SerializeField] private MeshRenderer [] mushrooms;
        [SerializeField] private float mushroomGrowRate = 0.0125f;
        [SerializeField] private float mushroomRefreshRate = 0.025f;
        

        #region UnityMethods

        //To-Do: Put in the EntityEvents Actions what you need
        private void OnEnable()
        {
            ToggleEvents(true);
        }

        private void OnDisable()
        {
            ToggleEvents(false);
        }

        #endregion

        private void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.OnTakenEntityDamage += AttackImpact;
                    _events.EntityUnit.OnTakenEnvironmentDamage += AttackImpact;
                    _events.ResourceOutput.EntityContext += ContextChanged;
                    _events.ResourceInput.State += StateChanged;
                    break;
                case false:
                    _events.EntityUnit.OnTakenEntityDamage -= AttackImpact;
                    _events.EntityUnit.OnTakenEnvironmentDamage -= AttackImpact;
                    _events.ResourceOutput.EntityContext -= ContextChanged;
                    _events.ResourceInput.State -= StateChanged;

                    break;
            }
        }

        private void StateChanged(EntityStateType type)
        {
            switch (type)
            {
                case EntityStateType.Dead:
                    if (_resource.EntityContext.ResourceValue == EntityContextType.Ally)
                        StartCoroutine(DissolveCo());
                    break;
            }
        }

        private void ContextChanged(EntityContextType type)
        {
            if (type == EntityContextType.Ally)
            {
                materialChangeFeedback?.PlayFeedbacks();
                if(mushrooms.Length >= 1 )
                    SelectMushrooms();
            }
        }

        private void AttackImpact() {
            if (_renderer != null)
            {

                if (_resource.Health.ResourceValue > 0)
                    _renderer.material.SetFloat("_DamageAmount", (1 - (_resource.Health.ResourceValue / _allStatsConfig.GetStats(_resource.EntityContext.ResourceValue).EntityStatContainer.healthPoints)));
                else if (_resource.Health.ResourceValue <= 0)
                {
                    _renderer.material.SetFloat("_DamageAmount", 0f);
                    _renderer.material.SetFloat("_Saturation", 0.8f);

                    if(_resource.EntityContext.ResourceValue == EntityContextType.Ally)
                    {
                        StartCoroutine(DissolveCo());
                    }
                    
                }

            }
        }

        IEnumerator DissolveCo()
        {
             float counter = 0;
            while (_renderer.material.GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate;
                _renderer.material.SetFloat("_DissolveAmount", counter);
                yield return new WaitForSeconds(refreshRate);
            }
            //fire event if ending coroutine
            _events.EntityUnit.FinishDissolveShader?.Invoke();
        }


        private void SelectMushrooms()
        {
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < mushrooms.Length; i++)
                availableIndices.Add(i);

            for (int i = 0; i < mushroomAmount; i++)
            {
                int randomIndex = Random.Range(0, availableIndices.Count);
                int selectedIndex = availableIndices[randomIndex];

                StartCoroutine(MushroomGrow(selectedIndex));

            }
        }

        IEnumerator MushroomGrow(int selectedIndex)
        {  
            float counter = 0;
            while (mushrooms[selectedIndex].material.GetFloat("_GrowFloat") < 1)
            {
                    counter += mushroomGrowRate;
                    mushrooms[selectedIndex].material.SetFloat("_GrowFloat", counter);
                    yield return new WaitForSeconds(mushroomRefreshRate);
            }
        }
    }
}


