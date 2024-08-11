using System;

namespace _Root.Scripts.Model.Runtime.Effects
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