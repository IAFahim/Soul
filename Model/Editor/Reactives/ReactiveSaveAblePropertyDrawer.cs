using Soul.Model.Runtime.Reactives;
using UnityEditor;
using UnityEngine;

namespace Soul.Model.Editor.Reactives
{
    [CustomPropertyDrawer(typeof(ReactiveSaveAble<>))]
    public class ReactiveSaveAblePropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var guidProperty = property.FindPropertyRelative("guid");
            return EditorGUI.GetPropertyHeight(valueProperty) + EditorGUI.GetPropertyHeight(guidProperty) + 4;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            var enabledProperty = property.FindPropertyRelative("saveNextChange");
            var guidProperty = property.FindPropertyRelative("guid");

            EditorGUI.BeginProperty(position, label, property);
            position.width -= 24;
            // Draw the value property
            var valueRect = new Rect(position.x, position.y, position.width,
                EditorGUI.GetPropertyHeight(valueProperty));
            EditorGUI.BeginDisabledGroup(!enabledProperty.boolValue);
            EditorGUI.PropertyField(valueRect, valueProperty, label, true);
            EditorGUI.EndDisabledGroup();

            // Draw the saveNextChange toggle
            var toggleRect = new Rect(position.x + position.width + 10, position.y, 24,
                EditorGUI.GetPropertyHeight(enabledProperty));
            EditorGUI.PropertyField(toggleRect, enabledProperty, GUIContent.none);

            // Draw the guid property
            var guidRect = new Rect(position.x, position.y + valueRect.height + 2, position.width,
                EditorGUI.GetPropertyHeight(guidProperty));
            EditorGUI.PropertyField(guidRect, guidProperty, new GUIContent("GUID"));

            EditorGUI.EndProperty();
        }
    }
}