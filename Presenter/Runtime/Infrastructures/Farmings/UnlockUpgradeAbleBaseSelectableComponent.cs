using System;
using Alchemy.Inspector;
using Soul.Controller.Runtime.SelectableComponents;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.UpgradeAndUnlock.Unlocks;
using Soul.Model.Runtime.UpgradeAndUnlock.Upgrades;
using UnityEngine;

namespace Soul.Presenter.Runtime.Infrastructures.Farmings
{
    public abstract class UnlockUpgradeAbleBaseSelectableComponent : BaseSelectableComponent, ICurrentLevelReference<int>, ISaveAble,
        ISaveAbleReference, IUpgrade, ILocked, IUnlock
    {
        [Title("UnlockUpgradeAbleBuilding")]
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
        public event Action<int> OnLevelChanged;

        #endregion

        public abstract bool CanUpgrade { get; }
        public abstract bool IsUpgrading { get; }
        public abstract void Upgrade();
        public abstract void OnUnlockUpgradeStart();
        public abstract void OnUnlockUpgradeComplete(int toLevel);

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