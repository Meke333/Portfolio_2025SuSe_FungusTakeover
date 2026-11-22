using Game.Interface;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Environment
{
    public class EnvironmentTrigger : MonoBehaviour
    {
        private EnvironmentEffectConfiguration _configuration;
        [field: SerializeField] public bool OverrideScriptableObject { get; private set; }
        [SerializeField] private EnvironmentEffect _effect;

        [SerializeField] private Collider _collider;
        private List<IEnvironmentDependent> _affectedEntities = new List<IEnvironmentDependent>();

        private void Start()
        {
            //make the pushdirection the direction of the gameobjects transform forward if it's not set to anything else
            if (_effect.PushDirection == Vector3.zero)
                _effect.PushDirection = this.transform.forward;
        }
        void FixedUpdate()
        {
            EvaluateCollision();
        }

        private void EvaluateCollision()
        {
            Collider[] colliders = ColliderHelper.Get3DColliderArray(_collider);
            List<IEnvironmentDependent> _tempAffected = new List<IEnvironmentDependent>();

            for (int i = 0; i < colliders.Length; i++)
            {
                IEnvironmentDependent temp = ColliderHelper.GetComponent<IEnvironmentDependent>(colliders[i]);

                if (temp == null)
                    continue;

                if (!temp.IsAfflictable(_effect.environmentType))
                    continue;

                temp.AfflictEffect(_effect);

                //check if still in list, or if not then add it, later remove any that aren't in it???
                if (!_tempAffected.Contains(temp))
                    _tempAffected.Add(temp);
            }

            //set up list if it was empty before 
            if (_affectedEntities.Count == 0)
            {
                _affectedEntities = _tempAffected;
                return;
            }
            //if it isnt empty then it had entities from prior loop
            //remove any that arent hit anymore and make them stop the effects
            foreach (IEnvironmentDependent temp in _affectedEntities) 
            {
                if (_tempAffected.Contains(temp))
                    continue;
                temp.StopEffect(_effect);
            }
            _affectedEntities = _tempAffected;
        }

        private void SetUpDataContainer()
        {
            if (OverrideScriptableObject)
                return;

            _effect = _configuration.Effect;
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = _collider.transform.localToWorldMatrix;
            ColliderHelper.DrawGizmos(_collider);
        }

    }
}

