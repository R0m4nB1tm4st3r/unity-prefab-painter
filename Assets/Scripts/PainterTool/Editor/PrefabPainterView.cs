using UnityEditor;

namespace PainterTool.Editor
{
    public class PrefabPainterView : EditorWindow
    {
        [MenuItem("/Tools/Prefab Painter")]
        private static void ShowWindow()
        {
            GetWindow<PrefabPainterView>();
            DrawView();
        }

        private static void DrawView()
        {
        
        }
    }
}


