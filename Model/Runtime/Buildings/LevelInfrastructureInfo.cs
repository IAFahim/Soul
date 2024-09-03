using System;
using Soul.Model.Runtime.Infos;
using UnityEngine;

namespace Soul.Model.Runtime.Buildings
{
    [Serializable]
    public class LevelInfrastructureInfo : ScriptableStringInfo
    {
        public Vector3[] infoPanelPositionOffsets;

        public Vector3 GetInfoPanelPositionOffset(int index)
        {
            return infoPanelPositionOffsets[Math.Min(index, infoPanelPositionOffsets.Length - 1)];
        }
    }
}