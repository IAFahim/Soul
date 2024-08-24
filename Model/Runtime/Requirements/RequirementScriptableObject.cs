﻿using UnityEngine;

namespace Soul.Model.Runtime.Requirements
{
    public class RequirementScriptableObject<T> : ScriptableObject
    {
        [SerializeField] protected T[] requirements;

        public T GetRequirement(int index) => requirements[index];
    }
}