using Game.Entity.States.Base;
using Zenject;
using Events;

namespace Game.Entity.States
{
    public class Ally_Dead : Dead
    {
        [Inject] EntityBase _base;
        [Inject] ProjectEvents _projectEvents;
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void ToggleEvents(bool shouldSubscribe)
        {
            switch (shouldSubscribe)
            {
                case true:
                    _events.EntityUnit.FinishDissolveShader += DissolveFinished;
                    break;
                case false:
                    _events.EntityUnit.FinishDissolveShader -= DissolveFinished;
                    break;
            }
        }
        private void DissolveFinished()
        {
            _projectEvents.Entity.RemoveNewAllyInManager?.Invoke(_base);
        }
    }
}

