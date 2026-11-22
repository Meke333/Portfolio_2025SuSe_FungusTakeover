using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Base
{
    /// <summary>
    /// StateBase for a Statemachine to use
    /// EVENT stage: internal Event-Stage for the Statemachine
    /// StateBase nestState: Next State to load when the EXIT Stage is done
    /// 
    /// State Methods to override
    /// - Enter()
    /// - Update()
    /// - Exit()
    /// Please don't forget to include the base methods for the statemachine to work!!!
    /// </summary>
    [System.Serializable]
    public abstract class StateBase
    {

        public enum EVENT
        {
            ENTER   = 0,
            UPDATE  = 1,
            EXIT    = 2
        }

        [ShowNonSerializedField] protected EVENT stage;
        [ShowNonSerializedField] protected StateBase nextState;

        public StateBase()
        {

        }

        public abstract void SetStateType();

        public virtual void Enter()
        {
            stage = EVENT.UPDATE;
            //Debug.Log("Entering " + this.GetType().ToString());
        }

        public virtual void Update()
        {
            stage = EVENT.UPDATE;
        }

        public virtual void Exit()
        {
            stage = EVENT.EXIT;
            //Debug.Log("Exiting " + this.GetType().ToString());
        }

        /// <summary>
        /// Call this in an Update Loop to run the state machine.
        /// </summary>
        /// <returns>Current StateBase Instance.</returns>
        public StateBase Process()
        {
            switch (stage)
            {
                case EVENT.ENTER:
                    Enter();
                    break;
                case EVENT.UPDATE:
                    Update();
                    break;
                case EVENT.EXIT:
                    Exit();
                    return nextState;
            }

            return this;
        }

    }
}

