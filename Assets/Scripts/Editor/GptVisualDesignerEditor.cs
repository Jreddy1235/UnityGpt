using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityGPT
{
    [CustomEditor(typeof(GptVisualDesigner))]
    public class GptVisualDesignerEditor : Editor
    {
        private GptVisualDesigner _visualDesigner;
        private string _gridString;
        private int _width;

        private void OnEnable()
        {
            _visualDesigner = (GptVisualDesigner) target;
            _gridString = serializedObject.FindProperty("gridString").stringValue;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            if (GUILayout.Button("Generate Grid"))
            {
                _gridString = serializedObject.FindProperty("gridString").stringValue;
            }

            if (GUILayout.Button("Clear Grid"))
            {
                _gridString = null;
            }

            GenerateGrid();
        }

        private void GenerateGrid()
        {
            if (string.IsNullOrEmpty(_gridString)) return;
            GUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            var numbers = _gridString?.Split(',')?.Select(int.Parse).ToList() ?? new List<int>();
            var rows = serializedObject.FindProperty("rows").intValue;
            var columns = serializedObject.FindProperty("columns").intValue;
            var width = EditorGUIUtility.currentViewWidth / columns - 10;
            EditorGUI.BeginDisabledGroup(true);
            for (var i = 0; i < rows; i++)
            {
                GUILayout.BeginHorizontal();
                for (var j = 0; j < columns; j++)
                {
                    var index = i + rows * j;
                    if (index >= numbers.Count)
                        break;

                    var spriteMapping = _visualDesigner.Settings.GetSpriteMapping(numbers[index]);
                    if (spriteMapping != null)
                    {
                        GUI.color = spriteMapping.Color;
                        var content = new GUIContent(spriteMapping.Sprite.texture);
                        if (GUILayout.Button(content, new GUIStyle(GUI.skin.button), GUILayout.Width(width),
                                GUILayout.Height(width)))
                        {
                            DoOnGridCellClicked(i, j);
                        }
                        GUI.color = Color.white;
                    }
                    else
                    {
                        if (GUILayout.Button(numbers[index] + "", new GUIStyle(GUI.skin.button), GUILayout.Width(width),
                                GUILayout.Height(width)))
                        {
                            DoOnGridCellClicked(i, j);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            EditorGUI.EndDisabledGroup();
            GUI.color = Color.white;
            EditorGUILayout.EndVertical();
        }

        private void DoOnGridCellClicked(int row, int column)
        {
        }
    }
}