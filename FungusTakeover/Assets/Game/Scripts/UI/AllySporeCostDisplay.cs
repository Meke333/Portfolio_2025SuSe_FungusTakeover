using Game.Entity;
using System;
using UnityEngine;

namespace Game.UI
{
    public class AllySporeCostDisplay : MonoBehaviour
    {
        public EntityBase SetEntity;
        [SerializeField] private GameObject[] _sporeGameObjects = new GameObject[5];
        private int _setSporeCost;

        public void ChangeSporeDisplay(int amount)
        {
            if (amount == 0)
            {
                DeactivateDisplay();
                return;
            }

            ActivateDisplay(amount);
        }

        //Animations can be added here
        private void DeactivateDisplay()
        {
            this.gameObject.SetActive(false);
        }

        private void ActivateDisplay(int amount)
        {
            this.gameObject.SetActive(true);
            _setSporeCost = amount;

            if ((amount * 2) - 1 > _sporeGameObjects.Length)
            {
                throw new InvalidOperationException("Amount of Spores is not given in the AllySporeCost Prefab!!");
            }

            for (int i = 0; i < _sporeGameObjects.Length; i++)
            {
                _sporeGameObjects[i].SetActive(i < (amount * 2) - 1);
            }
        }
    }

}


