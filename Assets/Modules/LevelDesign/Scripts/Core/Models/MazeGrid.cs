using System.Collections.Generic;

namespace UnityGPT
{
    public class MazeGrid
    {
        public Dictionary<MazeTile, MazePathInfo> PathsMapping { get; } = new();
        public List<Stack<MazeTile>> MockPaths { get; } = new();
        public List<Stack<MazeTile>> CoveragePaths { get; } = new();
        public List<Stack<MazeTile>> Shortcuts { get; } = new();
        private MazeTile[,] Grid { get; }
        public MazeTile this[int i, int j] => Grid[i, j];
        public MazeTile SelectedTile { get; set; }
        public MazeGridBindingTiles BindingTiles { get; set; } = new();
        public int RowCount => Grid.GetLength(0);
        public int ColumnCount => Grid.GetLength(1);

        public MazeGrid(int[,] grid)
        {
            Grid = new MazeTile[grid.GetLength(0), grid.GetLength(1)];
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    Grid[i, j] = new MazeTile
                    {
                        RowIndex = i,
                        ColumnIndex = j,
                        Value = grid[i, j],
                    };
                }
            }

            SetNeighbours();
        }

        private void SetNeighbours()
        {
            for (var i = 0; i < Grid.GetLength(0); i++)
            {
                for (var j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid[i, j].Neighbors.LeftTile = j > 0 ? Grid[i, j - 1] : null;
                    Grid[i, j].Neighbors.RightTile = j < Grid.GetLength(1) - 1 ? Grid[i, j + 1] : null;
                    Grid[i, j].Neighbors.TopTile = i > 0 ? Grid[i - 1, j] : null;
                    Grid[i, j].Neighbors.BottomTile = i < Grid.GetLength(0) - 1 ? Grid[i + 1, j] : null;
                }
            }
        }

        public int[,] ToIntArray()
        {
            var grid = new int[RowCount, ColumnCount];
            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    grid[i, j] = Grid[i, j].Value;
                }
            }

            return grid;
        }

        public IEnumerable<MazeTile> ToList()
        {
            var tiles = new List<MazeTile>();
            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    tiles.Add(Grid[i, j]);
                }
            }

            return tiles;
        }
    }
}