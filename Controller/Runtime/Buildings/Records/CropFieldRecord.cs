using System;
using System.Collections.Generic;
using _Root.Scripts.Model.Runtime;
using _Root.Scripts.Model.Runtime.Levels;
using _Root.Scripts.Model.Runtime.Workers;
using Pancake.Common;
using QuickEye.Utility;

namespace _Root.Scripts.Controller.Runtime.Buildings.Records
{
    [Serializable]
    public record CropFieldRecord
    {
        public Level level;
        public List<WorkerGroup> workers;
        public UnityDateTime startDateTime;
        public UnityTimeSpan timeReduction;
        public EBuildingBusyAt buildingBusyAt;

        public UnityTimeSpan TimeRequiredForTask(UnityTimeSpan fullTime)
        {
            return new UnityTimeSpan();
        }


        public static implicit operator Level(CropFieldRecord cropFieldRecord) => cropFieldRecord.level;
        public void Save(string key) => Data.Save(key, this);
        public CropFieldRecord Load(string key) => Data.Load(key, this);
    }
}