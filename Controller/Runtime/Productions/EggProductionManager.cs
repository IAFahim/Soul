using QuickEye.Utility;
using Soul.Controller.Runtime.Inventories;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.RequiredAndRewards.Rewards;
using Soul.Model.Runtime.SaveAndLoad;

namespace Soul.Controller.Runtime.Productions
{
    public class EggProductionManager : ProgressionManager<RecordProduction>, IWeightCapacityReference,
        IRewardClaim,
        IReward<Pair<Item, int>>
    {
        public override UnityTimeSpan FullTimeRequirement { get; }

        protected override bool Setup(RecordProduction record, Level level, ISaveAbleReference saveAbleReference)
        {
            return base.Setup(record, level, saveAbleReference);
        }

        protected override void TakeRequirement()
        {
        }

        public override bool HasEnough()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTimerStart()
        {
            throw new System.NotImplementedException();
        }

        public override void OnComplete()
        {
            throw new System.NotImplementedException();
        }

        public int WeightCapacity { get; set; }
        public bool CanClaim { get; }

        public void RewardClaim()
        {
            throw new System.NotImplementedException();
        }

        public Pair<Item, int> Reward { get; }
    }
}