using UnityEditor;

// Simple script that creates a new non-dockable window.
namespace Soul.Model.Editor.Example
{
    public class EditorWindowTest : EditorWindow
    {
        [MenuItem("Examples/Display simple Window")]
        static void Initialize()
        {
            var window = (EditorWindowTest)GetWindow(typeof(EditorWindowTest), true, "My Empty Window");
        }
    }
}