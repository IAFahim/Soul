using _Root.Scripts.Model.Runtime.Attacks;

namespace _Root.Scripts.Controller.Runtime.Attacks
{
    public interface IAttack
    {
        public void Execute(AttackIntention attackIntention);
    }
}