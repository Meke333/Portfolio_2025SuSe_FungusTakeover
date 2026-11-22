using Sirenix.OdinInspector;

namespace Game.Entity
{
    [System.Serializable]
    public class EnemyValueContainer
    {
        public bool enableNoticeRange;

        [ShowIf(nameof(enableNoticeRange))] public float noticeRange;
    }
}

