using System;
using UnityEngine;

namespace Game.Entity
{
    public class EntityInput_Enemy : EntityInput_Base
    {
        //Override Methods if you need to
        public override void ProcessMoveInput(Vector2 input)
        {
            return;
        }

        public override void ProcessAttackInput(bool input)
        {
            return;
        }

        public override void ProcessAttackDirectionInput(Vector2 input)
        {
            return;
        }

        public override void ProcessShootSporeInput(bool input)
        {
            return;
        }

        public override void ProcessShootSporeDirectionInput(Vector2 input)
        {
            return;
        }

        public override void ProcessCallAlliesInput(bool input)
        {
            return;
        }

        public override void ProcessConsumeAlliesInput(bool input)
        {
            return;
        }
    }
}

