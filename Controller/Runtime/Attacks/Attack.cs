using System;
using Pancake;
using UnityEngine;

namespace _Root.Scripts.Controller.Runtime.Attacks
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