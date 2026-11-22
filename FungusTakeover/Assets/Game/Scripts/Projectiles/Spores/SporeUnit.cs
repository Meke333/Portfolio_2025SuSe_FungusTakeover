using Game.Interface;
using Interface;
using ObjectPool;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;


namespace Game.Projectiles
{
    /// <summary>
    /// Behavior Script for the Spores
    /// It will:
    /// - Check for ISporables (IDamagable if it needs to due to upgrades maybe)
    /// - Check for it's lifetime (Deactivates if Time is up)
    /// - 
    /// </summary>
    public class SporeUnit : PoolObjectBase

    {
        [Inject] private SporeConfiguration _config;

        //If multiple are wanted then use Array or List
        //this can be replaced in a different way since it uses Transform
        //due to the Collider2DHelper Class implementation
        [SerializeField, Title("Hitbox")] private Transform _hitBox; 
        [SerializeField] private LayerMask _hitLayerMask;

        private Vector3 _moveDirection;

        #region UnityMethods

        private void Update()
        {
            LifeTimeCheck();
            Movement();
            EvaluateHitbox();
        }



        #endregion

        //Sets Movement Direction
        public void SetMovementDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }

        //Lifetime Check, how long the Spore would live
        private void LifeTimeCheck()
        {
            throw new NotImplementedException();
        }

        //Move the spore into an direction;
        private void Movement()
        {
            throw new NotImplementedException();
        }

        private void EvaluateHitbox()
        {
            //I provided a possible implementation of a HitboxEvaluation
            //Replace it if you don't want to use this

            //Get List of all hit Hurtboxes and search for ISporables
            //Look at Collider2DHelper Class for 2D Colliders!

            //e.g.
            /*ISporeable[] sporeList = Collider2DHelper.GetHitArray<ISporeable>(_hitBox, _hitLayerMask); //Get all ISporeables from Casting
            for (int i = 0; i < sporeList.Length; i++)
            {
              if (!sporeList[i].IsSporeable())
                  continue;

                sporeList[i].Spore();
            }

            // When Spores should do Damage:
            IDamagable[] damageList = Collider2DHelper.GetHitArray<IDamagable>(_hitBox, _hitLayerMask);//Get all ISporeables from Casting
            for (int i = 0; i < damageList.Length; i++)
            {
              if (!damageList[i].IsDamagable())
                  continue;

                //put value
                damageList[i].Damage(0);
            }
*/
            //To-Do: Check for Wall before all of these stuff and disable the gameObject

        }

        //Reset Object to their Original State
        public override void ResetObject()
        {
            throw new NotImplementedException();
        }
    }

}

