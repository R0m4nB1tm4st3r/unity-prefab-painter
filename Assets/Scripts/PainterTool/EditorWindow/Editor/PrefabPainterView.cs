using System;
using UnityEditor;
using UnityEngine;
using EPainterMode = PainterTool.Enums.EPainterMode;

namespace PainterTool.EditorWindow.Editor
{
    public class PrefabPainterView : UnityEditor.EditorWindow
    {
        #region Constant Values

        private const int DefaultFontSize = 16;
        private const int GreetingFontSize = 32;
        private const int SettingsVerticalGap = 20;
        private const float StandardPainterGizmoRadius = 0.3f;
        private const float GizmoAlpha = 0.3f;
        private const float BrushRadiusMin = 2f;
        private const float BrushRadiusMax = 5f;
        private const float FillRadiusMin = 6f;
        private const float FillRadiusMax = 15f;
        private const string GreetingMessage = "Hello to the Prefab Painter.";
        private const string SelectModeLabel = "Select Painter Mode";
        private const string SetScaleFactorLabel = "Set Prefab Scale";
        private const string SetTargetLayerLabel = "Set Target Layer";
        private const string SetBrushRadiusLabel = "Set Brush Radius";
        private const string SetFillRadiusLabel = "Set Fill Radius";
        private const string SetSinglePrefabLabel = "Select Prefab";
        private const string SetPrefabListLabel = "Add/Remove Prefabs";
        private const string RemovePrefabLabel = "Remove";
        private const string AddPrefabLabel = "Add";
        private const string CouldNotRemovePrefabErrorMessage = "Could not remove Prefab for some reason!";

        #endregion

        #region Dynamic Literals

        private static readonly Func<bool, string> TogglePainterLabel =
            (isPainterOn) => $"{(isPainterOn ? "Dea" : "A")}ctivate Painter";

        #endregion

        #region Properties

        public PrefabPainterViewModel ViewModel { get; private set; } = new ();

        #endregion

        #region Instance Fields

        private string[] painterModeTexts;
        private RaycastHit painterRaycastHit;

        #endregion

        #region Static Fields

        #endregion

        #region Instance Methods

        private void OnSceneGui(SceneView sceneView)
        {
            var model = ViewModel.Model;
            
            if (model.IsPainterEnabled && mouseOverWindow == sceneView)
            {
                // create Ray from current Mouse Position
                var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                // if no hit from Raycast, return
                if (!Physics.Raycast(ray, out painterRaycastHit, float.MaxValue, 1 << model.TargetLayers)) return;
            
                // else draw circle on hit position
                switch (model.Mode)
                {
                    case EPainterMode.Standard:
                        DrawPainterGizmo(painterRaycastHit.point, painterRaycastHit.normal, StandardPainterGizmoRadius, Color.cyan);
                        break;
                    case EPainterMode.Brush:
                        DrawPainterGizmo(painterRaycastHit.point, painterRaycastHit.normal, model.BrushRadius, Color.yellow);
                        break;
                    case EPainterMode.Fill:
                        DrawPainterGizmo(painterRaycastHit.point, painterRaycastHit.normal, model.FillRadius, Color.green);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (Event.current.type == EventType.MouseDown)
                {
                    Debug.Log(Event.current.button);
                    switch (Event.current.button)
                    {
                        case 0:         // Left Button
                            ViewModel.Paint(painterRaycastHit);
                            break;
                        case 1:         // Right Button
                        case 2:         // Middle Button
                            break;
                    }
                }
            }
        }

        private void DrawEditorWindow()
        {
            var model = ViewModel.Model;

            // print Greeting Text
            GUILayout.Label(ApplyColorToString(GreetingMessage, nameof(Color.cyan)),
                GetGUIStyleFrom(true, GreetingFontSize, FontStyle.Bold));
            EditorGUILayout.Space(SettingsVerticalGap);

            // (de)-activate Painter
            EmbedSettingGUI(() => model.IsPainterEnabled = EditorGUILayout.Toggle(model.IsPainterEnabled),
                TogglePainterLabel(model.IsPainterEnabled));


            // select Painter Mode
            EmbedSettingGUI(() => model.Mode =
                (EPainterMode)GUILayout.SelectionGrid((byte)ViewModel.Model.Mode, painterModeTexts,
                    painterModeTexts.Length), SelectModeLabel);

            // set Prefab Scale Factor
            EmbedSettingGUI(() => model.ScaleFactor = EditorGUILayout.Vector3Field("", model.ScaleFactor), SetScaleFactorLabel);

            // set target Layer(s)
            EmbedSettingGUI(() => model.TargetLayers = EditorGUILayout.LayerField(model.TargetLayers.value), SetTargetLayerLabel);
            
            // select single Prefab in Standard Mode or add/remove multiple Prefabs
            if (model.Mode == EPainterMode.Standard)
            {
                EmbedSettingGUI(() => model.SinglePrefab = (GameObject)EditorGUILayout.ObjectField(model.SinglePrefab, typeof(GameObject)), SetSinglePrefabLabel);
            }
            else
            {
                // set Brush Radius if Painter Mode is Brush
                if (model.Mode == EPainterMode.Brush)
                {
                    EmbedSettingGUI(() => model.BrushRadius = EditorGUILayout.Slider(model.BrushRadius, BrushRadiusMin, BrushRadiusMax), SetBrushRadiusLabel);
                }
                // set Fill Area if Painter Mode is Fill
                else if (model.Mode == EPainterMode.Fill)
                {
                    EmbedSettingGUI(() => model.FillRadius = EditorGUILayout.Slider(model.FillRadius, FillRadiusMin, FillRadiusMax), SetFillRadiusLabel);
                }
                
                EmbedSettingGUI(() =>
                {
                    for (int i = 0; i < model.Prefabs.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        model.Prefabs[i] =
                            (GameObject)EditorGUILayout.ObjectField(model.Prefabs[i], typeof(GameObject));
                        if (GUILayout.Button(RemovePrefabLabel))
                        {
                            var hasBeenRemoved = ViewModel.RemovePrefabFromList(model.Prefabs[i]);

                            if (!hasBeenRemoved)
                            {
                                Debug.LogError(CouldNotRemovePrefabErrorMessage);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button(AddPrefabLabel))
                    {
                        ViewModel.AddPrefabToList();
                    }
                }, SetPrefabListLabel);
            }
        }

        /// <summary>
        /// Encloses a GUI element with a label on the top and a space on the bottom.
        /// </summary>
        /// <param name="drawGUIElement">The function that draws the GUI to be wrapped.</param>
        /// <param name="label">The text for the label on top.</param>
        private void EmbedSettingGUI(Action drawGUIElement, string label)
        {
            GUILayout.Label(ApplyColorToString(label), GetGUIStyleFrom(true));
            drawGUIElement.Invoke();
            EditorGUILayout.Space(SettingsVerticalGap);
        }

        #endregion

        #region Static Methods

        [MenuItem("/Tools/Prefab Painter")]
        private static void ShowWindow()
        {
            GetWindow<PrefabPainterView>();
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
        
        private static GUIStyle GetGUIStyleFrom(bool richText = false, int fontSize = DefaultFontSize,
            FontStyle fontStyle = FontStyle.Normal)
        {
            return new GUIStyle() { fontSize = fontSize, fontStyle = fontStyle, richText = richText };
        }
        
        private static void DrawPainterGizmo(Vector3 center, Vector3 normal, float radius, Color c)
        {
            Handles.color = new Color(c.r, c.g, c.b, GizmoAlpha);
            Handles.DrawSolidDisc(center, normal, radius);
        }

        #endregion

        #region Methods called by Unity

        private void OnEnable()
        {
            painterModeTexts = Enum.GetNames(typeof(EPainterMode));
            SceneView.duringSceneGui += OnSceneGui;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGui;
        }

        private void OnGUI()
        {
            DrawEditorWindow();
            // HandlePainter();
        }

        #endregion
    }
}
