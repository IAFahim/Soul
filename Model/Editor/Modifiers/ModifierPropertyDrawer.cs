using UnityEditor;
using UnityEngine;
using Soul.Model.Runtime.Modifiers;

namespace Soul.Model.Editor.Modifiers
{
    [CustomPropertyDrawer(typeof(Modifier))]
    public class ModifierDrawer : PropertyDrawer
    {
        private const float PropertySpacing = 2f;
        private const float HeaderHeight = 22f;
        private const float PropertyHeight = 18f;
        private static readonly Color HeaderColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);
        private static readonly Color ValueColor = new Color(0.2f, 0.8f, 0.2f);
        private static readonly Color AlternateRowColor = new Color(0.5f, 0.5f, 0.5f, 0.1f);

        private static readonly GUIContent[] PropertyLabels =
        {
            new GUIContent("Base", "The base value of the modifier"),
            new GUIContent("Multiplier", "The multiplier applied to the base value"),
            new GUIContent("Additive", "The value added after multiplication")
        };

        private static readonly string[] PropertyNames = { "baseValue", "multiplier", "additive" };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modifier = GetModifierFromProperty(property);
            var headerRect = new Rect(position.x, position.y, position.width, HeaderHeight);

            DrawHeader(headerRect, property, label, modifier);

            if (property.isExpanded)
            {
                DrawProperties(position, property);
            }

            EditorGUI.EndProperty();
        }

        private void DrawHeader(Rect rect, SerializedProperty property, GUIContent label, Modifier modifier)
        {
            EditorGUI.DrawRect(rect, HeaderColor);

            // Foldout
            bool isSmall = rect.width < 200;
            float sideWidth = isSmall ? 40 : 200;
            var foldoutRect = new Rect(rect.x, rect.y, rect.width - sideWidth, rect.height);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            // Value display

            var valueRect = new Rect(foldoutRect.xMax, foldoutRect.y, sideWidth, foldoutRect.height);

            var style = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = ValueColor },
                fontStyle = FontStyle.Bold
            };
            EditorGUI.LabelField(valueRect, LabelString(modifier, isSmall), style);
        }

        private string LabelString(Modifier modifier, bool isSmall)
        {
            return isSmall
                ? $"{modifier.Value:F2}"
                : $"{modifier.BaseValue}x{modifier.Multiplier}+{modifier.Additive} = {modifier.Value:F2}";
        }

        private void DrawProperties(Rect position, SerializedProperty property)
        {
            var propertyRect = new Rect(position.x, position.y + HeaderHeight, position.width, PropertyHeight);

            for (int i = 0; i < PropertyNames.Length; i++)
            {
                propertyRect.y += PropertySpacing;

                if (i % 2 == 1)
                {
                    EditorGUI.DrawRect(propertyRect, AlternateRowColor);
                }

                EditorGUI.PropertyField(propertyRect, property.FindPropertyRelative(PropertyNames[i]),
                    PropertyLabels[i]);
                propertyRect.y += PropertyHeight;
            }
        }

        private Modifier GetModifierFromProperty(SerializedProperty property)
        {
            return new Modifier(
                property.FindPropertyRelative("baseValue").floatValue,
                property.FindPropertyRelative("multiplier").floatValue,
                property.FindPropertyRelative("additive").floatValue
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = HeaderHeight;
            if (property.isExpanded)
            {
                height += (PropertyHeight + PropertySpacing) * PropertyNames.Length;
            }

            return height;
        }
    }
}