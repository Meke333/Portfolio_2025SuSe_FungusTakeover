using System;
using UnityEngine;

namespace Game.Entity
{
    [Serializable]
    public class EntityInputData
    {
        public Vector2 moveInput;
        public bool attackInput;
        public Vector2 attackDirection;
        public bool shootSporeInput;
        public Vector2 shootSporeDirection;
        public bool callAlliesInput;
        public bool consumeAlliesInput;
    }
}

