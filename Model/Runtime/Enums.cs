namespace Soul.Model.Runtime
{
    public enum DamageTypeModes
    {
        BaseDamage,
        TypedDamage
    }


    public enum PlaceEnum
    {
        Gate,
        OuterWall,
        City,
        Map
    }

    public enum EQuality
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    public enum EDailyRewardDayStatus
    {
        Claimable = 0,
        Claimed = 1,
        Locked = 2
    }

    public enum EProgress
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2
    }

    public enum EBuildingBusyAt
    {
        None = 0,
        UnLocking = 1,
        Production = 2,
        Upgrading = 4
    }
}