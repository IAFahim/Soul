﻿using System;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Progressions;
using Soul.Model.Runtime.Records;
using UnityEngine;

namespace Soul.Controller.Runtime.Records
{
    [Serializable]
    public class RecordProduction : ITimeBasedReference, IInProgression
    {
        public RecordWorker worker;
        [SerializeField] private RecordTime time;
        public bool isProducing;
        public Item productionItem;
        public ITimeBased Time => time;

        public bool InProgression
        {
            get => isProducing;
            set => isProducing = value;
        }
    }
}