using System.Collections.Generic;
using PainterTool.Enums;
using UnityEngine;

namespace PainterTool.EditorWindow
{
    public class PrefabPainterModel
    {
        public bool IsPainterEnabled { get; set; } = false;
        public float BrushRadius { get; set; } = 2f;
        public float FillRadius { get; set; } = 6f;
        public Vector3 ScaleFactor { get; set; } = Vector3.one;
        public LayerMask TargetLayers { get; set; } = new ();
        public EPainterMode Mode { get; set; } = EPainterMode.Standard;
        public GameObject SinglePrefab { get; set; } = null;
        public List<GameObject> Prefabs { get; } = new ();
    }
}
