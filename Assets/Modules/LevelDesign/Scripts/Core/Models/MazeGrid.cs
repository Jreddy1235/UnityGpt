using System.Collections.Generic;

namespace UnityGPT
{
    public class MazeGrid
    {
        public Dictionary<int, Stack<MazeTile>> Paths { get; } = new();
        public MazeTile[,] Grid { get; private set; }
        public MazeTile this[int i, int j] => Grid[i, j];
        public MazeTile SelectedTile { get; set; }
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
                        ColIndex = j,
                        Value = grid[i, j],
                    };
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
        
        public List<MazeTile> ToList()
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