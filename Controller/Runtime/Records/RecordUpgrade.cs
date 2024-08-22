using System;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Records;
using UnityEngine;

namespace Soul.Controller.Runtime.Records
{
    [Serializable]
    public class RecordUpgrade : ITimeBasedReference, IInProgression
    {
        public RecordWorker worker;
        
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