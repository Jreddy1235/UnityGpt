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
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private int bingoItemsRequired;
        [SerializeField] private int collectableCount;

        public MazeSettings Settings => presetSelector == null ? null : presetSelector.Settings;
        public MazeGridConfiguration GridConfiguration => GridCreator.Configuration;

        private MazeGridCreator GridCreator => presetSelector.GetGridCreator(_presetName);

        private string _presetName;
        private Action _onGridCreated;

        public void SendRequest(string input, Action onGridCreated)
        {
            _onGridCreated = onGridCreated;
            if (completions != null)
                completions.SendRequest(input, OnCompletionReceived);
        }

        public void SetGridString(string value)
        {
            gridString = value;
            EditorUtility.SetDirty(this);
        }

        public void GenerateGrid(Action onGridCreated)
        {
            _onGridCreated = onGridCreated;
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
            _presetName = null;
        }

        public void OverrideGridConfiguration()
        {
            GridConfiguration.GridSize = gridSize;
            GridConfiguration.BingoItemsRequired = bingoItemsRequired;
            GridConfiguration.Collectables[0].Amount.Min =
                GridConfiguration.Collectables[0].Amount.Max = collectableCount;
        }

        private void OnGridCreated(string gridStr, int rowCount, int columnCount)
        {
            gridString = gridStr;
            rows = rowCount;
            columns = columnCount;
            _onGridCreated?.Invoke();
        }

        private void OnCompletionReceived(string response)
        {
            _presetName = response;
            GenerateGrid(_onGridCreated);
        }
    }
}