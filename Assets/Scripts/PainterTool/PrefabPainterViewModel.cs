using UnityEngine;

namespace PainterTool
{
    public class PrefabPainterViewModel
    {
        public PrefabPainterModel Model { get; private set; } = new();

        public void AddPrefabToList()
        {
            Model.Prefabs.Add(null);
        }

        public bool RemovePrefabFromList(GameObject prefab)
        {
            var hasBeenRemoved = Model.Prefabs.Remove(prefab);
            return hasBeenRemoved;
        }
    }
}