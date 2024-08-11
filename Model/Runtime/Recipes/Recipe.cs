﻿using System;
using UnityEngine;

namespace _Root.Scripts.Model.Runtime
{
    public enum IngredientUnit
    {
        Spoon,
        Cup,
        Bowl,
        Piece
    }

// Custom serializable class
    [Serializable]
    public class Ingredient
    {
        public string name;
        public int amount = 1;
        public IngredientUnit unit;
    }

    public class Recipe : MonoBehaviour
    {
        public Ingredient potionResult;
        public Ingredient[] potionIngredients;
    }
}