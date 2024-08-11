using System;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Variables;
using UnityEngine;

namespace Soul.Presenter.Runtime.UI
{
    [Serializable]
    public class SelectData
    {
        public int foundCount;
        public InterfaceFinder<ITitle> title;
        public InterfaceFinder<ILevel> level;

        public int GetDataFrom(Transform transform)
        {
            foundCount = 0;
            if (title.TryGet(transform, ref title)) foundCount++;
            if (level.TryGet(transform, ref level)) foundCount++;
            return foundCount;
        }
    }
}