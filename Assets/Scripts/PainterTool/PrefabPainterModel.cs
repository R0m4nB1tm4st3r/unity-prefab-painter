using System.Collections.Generic;
using PainterTool.Enums;
using UnityEngine;

namespace PainterTool
{
    public class PrefabPainterModel
    {
        public bool IsPainterEnabled { get; set; } = false;
        public float BrushArea { get; set; } = 2f;
        public Vector3 ScaleFactor { get; set; } = Vector3.one;
        public LayerMask TargetLayers { get; set; } = new ();
        public EPainterMode Mode { get; set; } = EPainterMode.Standard;
        public List<GameObject> Prefabs { get; } = new ();
    }
}
