using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Peoples.Workers;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Records;
using UnityEngine;

namespace Soul.Controller.Runtime.Upgrades
{
    [Serializable]
    public class RecordUpgrade : ITimeBasedReference, IInProgression
    {
        public  Pair<WorkerType, int>  worker;
        [SerializeField] private RecordTime time;
        [SerializeField] private bool isUpgrading;
        public int toLevel = 1;
        public ITimeBased Time => time;

        public bool InProgression
        {
            get => isUpgrading;
            set => isUpgrading = value;
        }
    }
}