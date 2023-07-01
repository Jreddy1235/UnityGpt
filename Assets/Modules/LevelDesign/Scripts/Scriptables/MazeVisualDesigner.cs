using System;
using UnityEditor;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Visual Designer Tool", fileName = "MazeVisualDesigner")]
    public class MazeVisualDesigner : ScriptableObject
    {
        [SerializeField] private MazePresetSelector presetSelector;
        [SerializeField] private GptCompletions completions;
        [SerializeField] [HideInInspector] private string gridString;
        [SerializeField] [HideInInspector] private int rows;
        [SerializeField] [HideInInspector] private int columns;

        public MazeSettings Settings => presetSelector == null ? null : presetSelector.Settings;
        public MazeGridConfiguration GridConfiguration => GridCreator.Configuration;

        private MazeGridCreator GridCreator => presetSelector.GetGridCreator(_presetName);

        private string _presetName;

        public void SendRequest(string input)
        {
            if (completions != null)
                completions.SendRequest(input);
        }

        public void SetGridString(string value)
        {
            gridString = value;
            EditorUtility.SetDirty(this);
        }

        public void GenerateGrid(Action onGridCreated)
        {
            void OnGridCreated(string gridStr, int rowCount, int columnCount)
            {
                gridString = gridStr;
                rows = rowCount;
                columns = columnCount;
                onGridCreated?.Invoke();
            }

            if (GridCreator != null)
                GridCreator.CreateGrid(OnGridCreated);
        }

        public void PrintGrid()
        {
            Debug.Log(gridString);
        }

        public void ResetGrid()
        {
            if (GridCreator != null)
                GridCreator.DoReset();
        }
    }
}