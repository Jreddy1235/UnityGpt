using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace UnityGPT
{
    [CustomEditor(typeof(MazeVisualDesigner))]
    public class MazeVisualDesignerEditor : Editor
    {
        private MazeVisualDesigner _visualDesigner;
        private string _gridString;
        private int _width;

        private void OnEnable()
        {
            _visualDesigner = (MazeVisualDesigner) target;
            _gridString = serializedObject.FindProperty("gridString").stringValue;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);

            if (GUILayout.Button("Generate Grid"))
            {
                _visualDesigner.GenerateGrid(OnGridCreated);
            }

            if (GUILayout.Button("Clear Grid"))
            {
                _gridString = null;
            }

            GenerateGrid();
        }

        private void OnGridCreated()
        {
            Observable.ReturnUnit().DelayFrame(1).Subscribe(_ =>
            {
                _gridString = serializedObject.FindProperty("gridString").stringValue;
                GUI.changed = true;
            });
        }

        private void GenerateGrid()
        {
            if (string.IsNullOrEmpty(_gridString)) return;

            try
            {
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
                        var pathIndicatorTexture = _visualDesigner.Settings.GetSpriteMapping(-3).Sprite.texture;
                        if (spriteMapping != null)
                        {
                            GUI.color = spriteMapping.Color;
                            var content = spriteMapping.HasBackground
                                ? GetCombinedContent(spriteMapping.Sprite.texture, pathIndicatorTexture)
                                : new GUIContent(spriteMapping.Sprite.texture);
                            if (GUILayout.Button(content, new GUIStyle(GUI.skin.button), GUILayout.Width(width),
                                    GUILayout.Height(width)))
                            {
                                DoOnGridCellClicked(i, j);
                            }

                            GUI.color = Color.white;
                        }
                        else
                        {
                            if (GUILayout.Button(numbers[index] + "", new GUIStyle(GUI.skin.button),
                                    GUILayout.Width(width),
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
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private GUIContent GetCombinedContent(Texture2D image1, Texture2D image2)
        {
            var combinedTexture = new Texture2D(image1.width, image1.height);
            var image1Pixels = image1.GetPixels();
            var image2Pixels = image2.GetPixels();
            var combinedPixels = new Color[image1Pixels.Length];
            for (int i = 0; i < image1Pixels.Length; i++)
            {
                combinedPixels[i] = Color.Lerp(image2Pixels[i], image1Pixels[i], image1Pixels[i].a);
            }

            combinedTexture.SetPixels(combinedPixels);
            combinedTexture.Apply();
            return new GUIContent(combinedTexture);
        }
        
        private void DoOnGridCellClicked(int row, int column)
        {
        }
    }
}