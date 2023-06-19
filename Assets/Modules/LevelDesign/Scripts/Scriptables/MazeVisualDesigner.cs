using System;
using UnityEngine;


namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Visual Designer Tool", fileName = "MazeVisualDesigner")]
    public class MazeVisualDesigner : ScriptableObject
    {
        [SerializeField] private MazeSettings settings;
        [SerializeField] private MazeGridCreator gridCreator;
        [SerializeField] private string gridString;
        [SerializeField] private int rows;
        [SerializeField] private int columns;

        public MazeSettings Settings => settings;


        public void GenerateGrid(Action onGridCreated)
        {
            void OnGridCreated(string gridStr, int rowCount, int columnCount)
            {
                gridString = gridStr;
                rows = rowCount;
                columns = columnCount;
                onGridCreated?.Invoke();
            }
            
            if (gridCreator != null) 
                gridCreator.CreateGrid(OnGridCreated);
        }

        public void ResetGrid()
        {
            gridCreator.DoReset();
        }
        
        
        private void Reset()
        {
            settings = Resources.Load<MazeSettings>("MazeSettings");
        }
    }
}