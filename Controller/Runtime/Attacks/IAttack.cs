namespace Soul.Controller.Runtime.Attacks
{
    public interface IAttack
    {
        public void Execute(AttackIntention attackIntention);
    }
}