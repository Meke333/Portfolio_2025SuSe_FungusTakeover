namespace Game.Environment
{
    [System.Flags]
    public enum EnvironmentEffectType
    {
        None = 0,
        Pushing = 1 << 0,
        Speed = 1 << 1,
        Poison = 1 << 2,
        All = ~0
    }
}

