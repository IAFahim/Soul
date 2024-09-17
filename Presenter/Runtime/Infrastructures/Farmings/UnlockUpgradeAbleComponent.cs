using System.Collections.Generic;
using Alchemy.Inspector;
using Soul.Controller.Runtime.SelectableComponents;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using Soul.Presenter.Runtime.Slots;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public abstract class UnlockUpgradeAbleComponent : BaseSelectableComponent, ICurrentLevelReference<int>, ISaveAble,
        ISaveAbleReference, IUpgrade, ILocked, IUnlock
    {
        [Title("UnlockUpgradeAbleComponent")]
        [SerializeField] protected UpgradeSlot upgradeSlotPrefab;
        [SerializeField] protected List<UpgradeSlot> upgradeSlots;
        
        #region ICurrentLevelReference

        public abstract int CurrentLevel { get; set; }

        #endregion

        #region ISaveAble

        public abstract void Save(string key);

        #endregion

        #region ISaveAbleReference

        public abstract void Save();

        #endregion

        public abstract void Load(string key);


        #region IUpgrade

        #region ILevel

        [SerializeField] protected Level level;
        public Level Level => level;

        #endregion

        public abstract bool CanUpgrade { get; }
        public abstract bool IsUpgrading { get; }
        public abstract void Upgrade();
        public abstract void OnUnlockUpgradeStart();
        public abstract void OnUnlockUpgradeComplete(int toLevel);

        public abstract void ShowUpgradeUnlockPreview(RectTransform parent);
        
        public virtual void HideUpgradeUnlockPreview()
        {
            foreach (var upgradeSlot in upgradeSlots) upgradeSlot.ReturnToPool();
            upgradeSlots.Clear();
        }

        #endregion

        #region ILocked

        public bool IsLocked => Level.IsLocked;

        #endregion

        #region IUnlock

        public abstract bool CanUnlock { get; }
        public abstract bool IsUnlocking { get; }
        public abstract void Unlock();

        #endregion

        
    }
}