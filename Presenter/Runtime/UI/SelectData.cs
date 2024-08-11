using System;
using _Root.Scripts.Model.Runtime.Interfaces;
using _Root.Scripts.Model.Runtime.Levels;
using _Root.Scripts.Model.Runtime.Variables;
using UnityEngine;

namespace _Root.Scripts.Presenter.Runtime.UI
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