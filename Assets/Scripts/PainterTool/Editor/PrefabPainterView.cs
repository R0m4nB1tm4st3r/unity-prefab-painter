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
            // print Greeting Text
            GUILayout.Label(ApplyColorToString(GreetingMessage, nameof(Color.cyan)),
                GetGUIStyleFrom(true, GreetingFontSize, FontStyle.Bold));
            EditorGUILayout.Space(SettingsVerticalGap);
            
            // select Painter Mode
            GUILayout.Label(ApplyColorToString(SelectModeLabel), GetGUIStyleFrom(true));
            viewModel.Model.Mode =
                (EPainterMode)GUILayout.SelectionGrid((byte)viewModel.Model.Mode, painterModeTexts, painterModeTexts.Length);
            EditorGUILayout.Space(SettingsVerticalGap);
        }

        private GUIStyle GetGUIStyleFrom(bool richText = false, int fontSize = DefaultFontSize, FontStyle fontStyle = FontStyle.Normal)
        {
            return new GUIStyle() { fontSize = fontSize, fontStyle = fontStyle, richText = richText};
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
