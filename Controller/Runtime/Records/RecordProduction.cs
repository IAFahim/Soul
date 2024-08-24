using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Records;
using UnityEngine;
using UnityEngine.Serialization;

namespace Soul.Controller.Runtime.Records
{
    [Serializable]
    public class RecordProduction : ITimeBasedReference, IInProgression
    {
        public RecordWorker worker;
        [SerializeField] private RecordTime time;
        public bool isProducing;
        [FormerlySerializedAs("productionItem")] public Pair<Item, int> productionItemValuePair;
        public ITimeBased Time => time;

        public bool InProgression
        {
            get => isProducing;
            set => isProducing = value;
        }
    }
}