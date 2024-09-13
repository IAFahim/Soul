using System;
using Soul.Controller.Runtime.RequiresAndRewards;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Items;
using Soul.Model.Runtime.Levels;
using Soul.Model.Runtime.Variables;
using UnityEngine;

namespace Soul.Presenter.Runtime.Manager
{
    [Serializable]
    public class SelectData
    {
        public int foundCount;
        public ComponentFinder<ITitle> titleReference;
        public ComponentFinder<ILevel> levelReference;
        
        public int Level => levelReference.Value.Level;
        
        


        public int GetDataFrom(Transform transform)
        {
            foundCount = 0;
            if (titleReference.TryGet(transform, ref titleReference)) foundCount++;
            if (levelReference.TryGet(transform, ref levelReference)) foundCount++;
            return foundCount;
        }
    }
}