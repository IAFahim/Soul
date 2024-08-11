using UnityEditor;
using UnityEngine;

// Create a foldable menu that hides/shows the selected transform position.
// if no Transform is selected, the Foldout item will be folded until a transform is selected.
namespace _Root.Scripts.Model.Editor.Example
{
    public class EditorGUIFoldout : EditorWindow
    {
        public bool showPosition = true;
        public string status = "Select a GameObject";
        [MenuItem("Examples/Foldout Usage")]
        static void Init()
        {
            UnityEditor.EditorWindow window = GetWindow(typeof(EditorGUIFoldout));
            window.position = new Rect(0, 0, 250, 250);
            window.Show();
        }

        void OnGUI()
        {
            showPosition = EditorGUI.Foldout(new Rect(3, 3, position.width - 6, 15), showPosition, status);
            if (showPosition)
                if (Selection.activeTransform)
                {
                    Selection.activeTransform.position = EditorGUI.Vector3Field(new Rect(3, 25, position.width - 6, 40), "Position", Selection.activeTransform.position);
                    status = Selection.activeTransform.name;
                }

            if (!Selection.activeTransform)
            {
                status = "Select a GameObject";
                showPosition = false;
            }
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}