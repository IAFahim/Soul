using Soul.Controller.Runtime.SpritePopups;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Rewards;
using UnityEngine;

namespace Soul.Controller.Runtime.Rewards
{
    public class RewardPopup : PopupSpriteCount
    {
        [SerializeField] private bool onCollectReturnToPool = true;
        
        private IRewardClaim _rewardClaimReference;
        private IReward<Pair<Item, int>> _rewardReference;

        public Pair<Item, int> Reward => _rewardReference.Reward;

        public void Setup(IRewardClaim rewardClaim, IReward<Pair<Item, int>> reward, bool returnToPool)
        {
            _rewardClaimReference = rewardClaim;
            _rewardReference = reward;
            onCollectReturnToPool = returnToPool;
            base.Setup(Reward.Key, Reward.Value);
        }

        public override void OnSelected(RaycastHit selfRaycastHit)
        {
            if (_rewardClaimReference.CanClaim) _rewardClaimReference.RewardClaim();
            if(onCollectReturnToPool) ReturnToPool();
        }
    }
}