using QuickEye.Utility;
using Soul.Controller.Runtime.DragAndDrop;
using Soul.Controller.Runtime.Inventories;
using Soul.Controller.Runtime.Records;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Rewards;
using Soul.Model.Runtime.SaveAndLoad;

namespace Soul.Controller.Runtime.Buildings.Managers
{
    public class EggProductionManager : ProgressionManager<RecordProduction>, ISingleDrop, IWeightCapacity,
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

        public float WeightLimit { get; }
        public bool CanClaim { get; }

        public void RewardClaim()
        {
            throw new System.NotImplementedException();
        }

        public Pair<Item, int> Reward { get; }
    }
}