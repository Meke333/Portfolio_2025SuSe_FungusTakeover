using System;
using UnityEngine;

namespace Game.Entity
{
    public class EntityInput_Ally : EntityInput_Base
    {
        //Override Methods if you need to

        public override void ProcessShootSporeInput(bool input)
        {
            return;
        }

        public override void ProcessShootSporeDirectionInput(Vector2 input)
        {
            return;
        }
        public override void ProcessConsumeAlliesInput(bool input)
        {
            return;
        }
    }
}

