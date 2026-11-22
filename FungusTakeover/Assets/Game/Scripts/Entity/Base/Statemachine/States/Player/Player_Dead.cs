using Game.Entity.States.Base;
using Zenject;
using Events;

namespace Game.Entity.States
{
    public class Player_Dead : Dead
    {
        [Inject] ProjectEvents _projectEvents;
        [Inject] EntityBase _base;

        public override void Enter()
        {
            base.Enter();
            _base.OnDeath?.Invoke();
            
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
            _projectEvents.Entity.PlayerDied?.Invoke(_base);
        }

    }
}

