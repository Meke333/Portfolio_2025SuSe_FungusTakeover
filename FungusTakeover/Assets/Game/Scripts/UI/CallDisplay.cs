using Events;
using Game.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class CallDisplay : GameUIElement
    {
        //Plays Animation when Pressed
        //Should Black out if the action is not possible

        bool _isPressable = true;


        public void TransmitCoolDownData(float percentage)
        {

            //Fill Up Cooldown Progress Bar

            if (percentage >= 1)
            {
                //Make Display Shine or habbe Animation

            }
        }

        public void ButtonPressed()
        {
            switch (_isPressable)
            {
                case true:
                    //PressAnimation
                    break;
                case false:
                    //Cannot be pressed dumbass animation
                    break;

            }
        }

        public void ChangeState(bool isPressable)
        {
            _isPressable = isPressable;
        }
    }

}


