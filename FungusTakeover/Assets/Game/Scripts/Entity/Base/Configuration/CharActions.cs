namespace Game.Entity
{
    [System.Flags]
    public enum CharActions
    {
        None = 0,
        Attack = 1 << 0,
        Rotate = 1 << 1,
        Spores = 1 << 2,
        Formation = 1 << 3,
        SpecialAttack = 1 << 4,
        All = ~0
    }
    [System.Flags]
    public enum PlayerActions
    {
        None = 0,
        SporeShooting = 1 << 2,
        Formation = 1 << 3,
        SpecialAttack = 1 << 4,
        All = ~0,
    }
}

