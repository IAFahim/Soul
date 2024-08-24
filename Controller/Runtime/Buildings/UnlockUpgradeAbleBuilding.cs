using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.SaveAndLoad;
using Soul.Model.Runtime.Unlocks;
using Soul.Model.Runtime.Upgrades;
using UnityEngine;

namespace Soul.Controller.Runtime.Buildings
{
    public abstract class UnlockUpgradeAbleBuilding : Building, IUpgrade, ILocked, IUnlock, ISaveAble, ISaveAbleReference
    {
        #region IUpgrade

        #region ILevel

        [SerializeField] protected Level level;
        public Level Level => level;

        #endregion

        public abstract bool CanUpgrade { get; }
        public abstract bool IsUpgrading { get; }
        public abstract void Upgrade();

        #endregion

        #region ILocked

        public bool IsLocked => Level.IsLocked;

        #endregion

        #region IUnlock

        public abstract bool CanUnlock { get; }
        public abstract bool IsUnlocking { get; }
        public abstract void Unlock();

        #endregion

        #region ISaveAble
        
        public abstract void Save(string key);

        #endregion

        #region ISaveAbleReference
        public abstract void Save();

        #endregion

        public abstract void Load(string key);
    }
}