using _Root.Scripts.Model.Runtime.Modifiers;
using UnityEditor;
using UnityEngine;

namespace _Root.Scripts.Model.Editor.Modifiers
{
    [CustomPropertyDrawer(typeof(Modifier))]
    public class ModifierDrawer : PropertyDrawer
    {
        private const int LineCount = 4;
        private static readonly string[] PropertyNames = { "baseValue", "multiplier", "additive" };
        private static readonly Color ValueColor = new(0.2f, 0.8f, 0.2f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var modifier = GetModifierFromProperty(property);
            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            // Custom foldout with colored value
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);
            var valueRect = new Rect(foldoutRect.xMax - 60, foldoutRect.y, 60, foldoutRect.height);
            var style = new GUIStyle(EditorStyles.label)
                { alignment = TextAnchor.MiddleRight, normal = { textColor = ValueColor } };
            EditorGUI.LabelField(valueRect, $"{modifier.Value:F2}", style);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                DrawProperties(position, property);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private void DrawProperties(Rect position, SerializedProperty property)
        {
            for (int i = 0; i < PropertyNames.Length; i++)
            {
                var propertyName = PropertyNames[i];
                var rect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + 2) * (i + 1),
                    position.width, EditorGUIUtility.singleLineHeight);
                var content = new GUIContent(ObjectNames.NicifyVariableName(propertyName));

                // Add subtle alternative row coloring
                if (i % 2 == 1)
                {
                    EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.1f));
                }

                EditorGUI.PropertyField(rect, property.FindPropertyRelative(propertyName), content);
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
            return property.isExpanded
                ? EditorGUIUtility.singleLineHeight * LineCount + 6
                : EditorGUIUtility.singleLineHeight;
        }
    }
}