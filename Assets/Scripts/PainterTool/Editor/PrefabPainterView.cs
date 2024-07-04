using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabPainterView : EditorWindow
{
    [MenuItem("/Tools/Prefab Painter")]
    private static void ShowWindow()
    {
        GetWindow<PrefabPainterView>();
    }
}
