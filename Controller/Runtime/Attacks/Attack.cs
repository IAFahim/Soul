using System;
using Pancake;

namespace Soul.Controller.Runtime.Attacks
{
    [Serializable]
    public class Attack : GameComponent, IAttack
    {
        public AttackIntention intention;

        public virtual void Execute(AttackIntention attackIntention)
        {
            intention = attackIntention;
        }
    }
}