using UnityEngine;

namespace Game.Entity
{
    /// <summary>
    /// Base for EntityAttack Brains
    /// IMPORTANT: Class must be the same name as the enum that holds this movement
    /// e.g.: Player => Game.Entity.EntityAttack_Player
    /// </summary>
    public abstract class EntityAttack_Base
    {
        protected EntityStatsConfiguration _entityStats;
        protected EntityContextType _contextType;
        protected Collider _collider;

        protected bool _animatedCollider; //if this is enabled, animate it over update
        public virtual bool AnimatedCollider //overwrite this so you can set the animation status
        {
            get => _animatedCollider;
            set => _animatedCollider = false;
        }
        public float _rangePerSecond = 0; //total range / total time -> * time.deltatime
        public float _timer = 0; //animation timer
        public Vector3 _animatedPosition;
        public Vector3 _animatedDirection;

        public void Initialize(EntityStatsConfiguration data, Transform colliderTransform, EntityContextType type)
        {
            _entityStats = data;
            _contextType = type;
            _collider = colliderTransform.gameObject.GetComponent<Collider>();
            AnimatedCollider = _animatedCollider;
        }

        public abstract void ProcessAttack();
        public abstract void ProcessAnimatedAttack();
        public abstract void DrawGizmos();
    }
}

