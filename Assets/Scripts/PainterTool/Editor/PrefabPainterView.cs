using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using EPainterMode = PainterTool.Enums.EPainterMode;

namespace PainterTool.Editor
{
    public class PrefabPainterView : EditorWindow
    {
        private const int DefaultFontSize = 16;
        private const int GreetingFontSize = 32;
        private const int SettingsVerticalGap = 20;
        private const string GreetingMessage = "Hello to the Prefab Painter.";
        private const string SelectModeLabel = "Select Painter Mode";
        private const string SetScaleFactorLabel = "Set Prefab Scale";
        private const string SetTargetLayerLabel = "Set Target Layer";
        private const string SetBrushAreaLabel = "Set Brush Area";
        private const string SetSinglePrefabLabel = "Select Prefab";
        private const string SetPrefabListLabel = "Add/Remove Prefabs";
        private const string RemovePrefabLabel = "Remove";
        private const string AddPrefabLabel = "Add";
        private const string CouldNotRemovePrefabErrorMessage = "Could not remove Prefab for some reason!";
        private static readonly Func<bool, string> TogglePainterLabel =
            (isPainterOn) => $"{(isPainterOn ? "Dea" : "A")}ctivate Painter";

        private PrefabPainterViewModel viewModel;
        private string[] painterModeTexts;

        [MenuItem("/Tools/Prefab Painter")]
        private static void ShowWindow()
        {
            GetWindow<PrefabPainterView>();
        }

        private void OnEnable()
        {
            viewModel = new PrefabPainterViewModel();
            painterModeTexts = Enum.GetNames(typeof(EPainterMode));
        }

        private void OnGUI()
        {
            var model = viewModel.Model;

            // print Greeting Text
            GUILayout.Label(ApplyColorToString(GreetingMessage, nameof(Color.cyan)),
                GetGUIStyleFrom(true, GreetingFontSize, FontStyle.Bold));
            EditorGUILayout.Space(SettingsVerticalGap);

            // (de)-activate Painter
            EmbedSettingGUI(() => model.IsPainterEnabled = EditorGUILayout.Toggle(model.IsPainterEnabled),
                TogglePainterLabel(model.IsPainterEnabled));

            // select Painter Mode
            EmbedSettingGUI(() => model.Mode =
                (EPainterMode)GUILayout.SelectionGrid((byte)viewModel.Model.Mode, painterModeTexts,
                    painterModeTexts.Length), SelectModeLabel);

            // set Prefab Scale Factor
            EmbedSettingGUI(() => model.ScaleFactor = EditorGUILayout.Vector3Field("", model.ScaleFactor), SetScaleFactorLabel);

            // set target Layer(s)
            EmbedSettingGUI(() => model.TargetLayers = EditorGUILayout.LayerField(model.TargetLayers), SetTargetLayerLabel);

            // set Brush Area if Painter Mode is Brush
            if (model.Mode == EPainterMode.Brush)
            {
                EmbedSettingGUI(() => model.BrushArea = EditorGUILayout.FloatField(model.BrushArea), SetBrushAreaLabel);
            }
            
            // select single Prefab in Standard Mode or add/remove multiple Prefabs
            if (model.Mode == EPainterMode.Standard)
            {
                EmbedSettingGUI(() => model.SinglePrefab = (GameObject)EditorGUILayout.ObjectField(model.SinglePrefab, typeof(GameObject)), SetSinglePrefabLabel);
            }
            else
            {
                EmbedSettingGUI(() =>
                {
                    for (int i = 0; i < model.Prefabs.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        model.Prefabs[i] =
                            (GameObject)EditorGUILayout.ObjectField(model.Prefabs[i], typeof(GameObject));
                        if (GUILayout.Button(RemovePrefabLabel))
                        {
                            var hasBeenRemoved = viewModel.RemovePrefabFromList(model.Prefabs[i]);

                            if (!hasBeenRemoved)
                            {
                                Debug.LogError(CouldNotRemovePrefabErrorMessage);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button(AddPrefabLabel))
                    {
                        viewModel.AddPrefabToList();
                    }
                }, SetPrefabListLabel);
            }
        }

        private void EmbedSettingGUI(Action drawGUIElement, string label)
        {
            GUILayout.Label(ApplyColorToString(label), GetGUIStyleFrom(true));
            drawGUIElement.Invoke();
            EditorGUILayout.Space(SettingsVerticalGap);
        }

        private GUIStyle GetGUIStyleFrom(bool richText = false, int fontSize = DefaultFontSize,
            FontStyle fontStyle = FontStyle.Normal)
        {
            return new GUIStyle() { fontSize = fontSize, fontStyle = fontStyle, richText = richText };
        }

        /// <summary>
        /// Wraps string with HTML-like color-Tags. Make sure you use this with a GUIStyle that has richText = true.
        /// </summary>
        /// <param name="str">The string to be wrapped.</param>
        /// <param name="colorName">The Color name string.</param>
        /// <returns>The new string with color-Tags.</returns>
        private static string ApplyColorToString(string str, string colorName = nameof(Color.white))
        {
            return $"<color={colorName}>{str}</color>";
        }
    }
}
