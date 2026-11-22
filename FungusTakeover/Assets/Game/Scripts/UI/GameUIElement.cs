using UnityEngine;


namespace Game.UI
{
    public abstract class GameUIElement : MonoBehaviour
    {
        [field: SerializeField] public bool CanBeHidden { get; private set; }

        public virtual void SetActive(bool shouldActivate)
        {
            //Change this if another way is used
            this.gameObject.SetActive(shouldActivate);
        }



            //Check if it's the MaxAmount (need data from scriptable object?)
            //If it is then make display disappear after a duration
        //public abstract void TransmitData(T data = default);


    }


}


