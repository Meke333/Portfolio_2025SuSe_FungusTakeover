using Game.Environment;

namespace Game.Interface
{
    public interface IEnvironmentDependent
    {
        public bool IsAfflictable(EnvironmentEffectType effectType);
        public void AfflictEffect(EnvironmentEffect effect);
        public void StopEffect(EnvironmentEffect effect);
    }

}

