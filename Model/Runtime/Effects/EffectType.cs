using System;

namespace Soul.Model.Runtime.Effects
{
    [Flags]
    public enum EffectType
    {
        None,
        Bleed,
        Poison,
        Fire,
        Slow,
        Knockback,
        Electrify
    }
}