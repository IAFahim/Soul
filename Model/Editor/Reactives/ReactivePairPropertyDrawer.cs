using UnityEditor;
using UnityEngine;
using Soul.Model.Runtime.Reactives;

namespace Soul.Model.Editor.Reactives
{
    [CustomPropertyDrawer(typeof(ReactivePair<,>))]
    public class ReactivePairDrawer : PropertyDrawer
    {
        private const float HeaderHeight = 22f;
        private const float PropertyHeight = 18f;
        private const float PropertySpacing = 2f;
        private static readonly Color HeaderColor = new Color(0.1f, 0.1f, 0.1f, 0.2f);
        private static readonly Color ValueColor = new Color(0.2f, 0.8f, 0.2f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var pairProperty = property.FindPropertyRelative("pair");
            var keyProperty = pairProperty.FindPropertyRelative("first");
            var valueProperty = pairProperty.FindPropertyRelative("second");

            var headerRect = new Rect(position.x, position.y, position.width, HeaderHeight);
            DrawHeader(headerRect, property, label, valueProperty);

            if (property.isExpanded)
            {
                DrawProperties(position, keyProperty, valueProperty);
            }

            EditorGUI.EndProperty();
        }

        private void DrawHeader(Rect rect, SerializedProperty property, GUIContent label,
            SerializedProperty valueProperty)
        {
            EditorGUI.DrawRect(rect, HeaderColor);

            // Foldout
            bool isSmall = rect.width < 200;
            float sideWidth = isSmall ? 40 : 200;
            var foldoutRect = new Rect(rect.x, rect.y, rect.width - sideWidth, rect.height);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            // Key-Value preview
            var previewRect = new Rect(foldoutRect.xMax, foldoutRect.y, sideWidth, foldoutRect.height);
            var style = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = ValueColor },
                fontStyle = FontStyle.Bold
            };
            string preview = $"{GetPreviewString(valueProperty)}";
            EditorGUI.LabelField(previewRect, preview, style);
        }

        private void DrawProperties(Rect position, SerializedProperty keyProperty, SerializedProperty valueProperty)
        {
            var propertyRect = new Rect(position.x, position.y + HeaderHeight, position.width, PropertyHeight);

            // Draw Key
            propertyRect.y += PropertySpacing;
            EditorGUI.PropertyField(propertyRect, keyProperty, new GUIContent("first"));
            propertyRect.y += PropertyHeight + PropertySpacing;

            // Draw Value
            EditorGUI.PropertyField(propertyRect, valueProperty, new GUIContent("second"));
        }

        private string GetPreviewString(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue.ToString();
                case SerializedPropertyType.Float:
                    return property.floatValue.ToString("F2");
                case SerializedPropertyType.String:
                    return property.stringValue;
                case SerializedPropertyType.Enum:
                    return property.enumNames[property.enumValueIndex];
                default:
                    return property.type;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = HeaderHeight;
            if (property.isExpanded)
            {
                height += (PropertyHeight + PropertySpacing) * 2; // For Key and Value
            }

            return height;
        }
    }
}