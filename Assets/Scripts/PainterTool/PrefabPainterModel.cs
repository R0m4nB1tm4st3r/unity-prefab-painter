using System.Collections.Generic;
using PainterTool.Enums;
using UnityEngine;

namespace PainterTool
{
    public class PrefabPainterModel
    {
        private bool isPainterEnabled = false;
        private float brushArea = 2f;
        private Vector3 scaleFactor = Vector3.one;
        private LayerMask targetLayers = new LayerMask();
        private EPainterMode mode = EPainterMode.Standard;
        private List<GameObject> prefabs = new ();
        
        public bool IsPainterEnabled { get => isPainterEnabled; set => isPainterEnabled = value; }
        public float BrushArea { get => brushArea; set => brushArea = value; }
        public Vector3 ScaleFactor { get => scaleFactor; set => scaleFactor = value; }
        public LayerMask TargetLayers { get => targetLayers; set => targetLayers = value; }
        public EPainterMode Mode { get => mode; set => mode = value; }
        public List<GameObject> Prefabs { get => prefabs; }
    }
}
