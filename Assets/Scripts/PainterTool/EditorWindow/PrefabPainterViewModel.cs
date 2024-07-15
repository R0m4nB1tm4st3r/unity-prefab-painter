using System;
using PainterTool.Enums;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PainterTool.EditorWindow
{
    public class PrefabPainterViewModel
    {
        private const string NoPrefabError = "There is no Prefab selected!";
        
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

        public void Paint(RaycastHit painterInfo)
        {
            switch (Model.Mode)
            {
                case EPainterMode.Standard:
                    PaintStandard(painterInfo);
                    break;
                case EPainterMode.Brush:
                    PaintBrush(painterInfo);
                    break;
                case EPainterMode.Fill:
                    PaintFill(painterInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PaintStandard(RaycastHit painterInfo)
        {
            if (Model.SinglePrefab == null)
            {
                Debug.LogError(NoPrefabError);
                return;
            }
            
            var prefabType = PrefabUtility.GetPrefabAssetType(Model.SinglePrefab);

            switch (prefabType)
            {
                case PrefabAssetType.NotAPrefab:
                case PrefabAssetType.MissingAsset:
                    var obj = Object.Instantiate(Model.SinglePrefab, painterInfo.point, Quaternion.identity);
                    obj.transform.up = painterInfo.normal;
                    return;
                case PrefabAssetType.Regular:
                case PrefabAssetType.Model:
                case PrefabAssetType.Variant:
                    var prefObj = (GameObject)PrefabUtility.InstantiatePrefab(Model.SinglePrefab);
                    prefObj.transform.position = painterInfo.point;
                    prefObj.transform.up = painterInfo.normal;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PaintBrush(RaycastHit painterInfo)
        {
            // TODO: implement brush
        }
        
        private void PaintFill(RaycastHit painterInfo)
        {
            // TODO: implement fill
        }


    }
}