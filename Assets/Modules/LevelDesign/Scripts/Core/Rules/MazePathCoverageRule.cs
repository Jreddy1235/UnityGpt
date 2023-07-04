using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace UnityGPT
{
    public class MazePathCoverageRule : MazeBaseRule
    {
        private readonly MazeGridBindingTiles _bindingTiles = new();

        private MazeTile _nearestTile;
        private float _currentDistance = float.MaxValue;

        protected override void OnInit()
        {
            base.OnInit();
            var edgePoints = new List<MazeTile>();
            for (var i = 0; i < Grid.RowCount; i++)
            {
                for (var j = 0; j < Grid.ColumnCount; j++)
                {
                    if (i == 0 || j == 0 || i == Grid.RowCount - 1 || j == Grid.ColumnCount - 1)
                        edgePoints.Add(Grid[i, j]);
                }
            }

            var sideTile = _bindingTiles.SideTile = edgePoints[Random.Range(0, edgePoints.Count)];
            if (sideTile.RowIndex == 0)
            {
                CheckForPreBinding(0, 1, 0, Grid.ColumnCount);
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, 0];
                _bindingTiles.SecondCornerTile = Grid[Grid.RowCount - 1, Grid.ColumnCount - 1];
            }
            else if (sideTile.RowIndex == Grid.RowCount - 1)
            {
                CheckForPreBinding(Grid.RowCount - 1, Grid.RowCount, 0, Grid.ColumnCount);
                _bindingTiles.FirstCornerTile = Grid[0, 0];
                _bindingTiles.SecondCornerTile = Grid[0, Grid.ColumnCount - 1];
            }
            else if (sideTile.ColumnIndex == 0)
            {
                CheckForPreBinding(0, Grid.RowCount - 1, 0, 1);
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, Grid.ColumnCount - 1];
                _bindingTiles.SecondCornerTile = Grid[0, Grid.ColumnCount - 1];
            }
            else if (sideTile.ColumnIndex == Grid.ColumnCount - 1)
            {
                CheckForPreBinding(0, Grid.RowCount - 1, Grid.ColumnCount - 1, Grid.ColumnCount);
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, 0];
                _bindingTiles.SecondCornerTile = Grid[0, 0];
            }
        }

        public override void Reset()
        {
            _nearestTile = null;
            _currentDistance = float.MaxValue;
        }

        private void CheckForPreBinding(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            for (var i = rowStart; i < rowEnd; i++)
            for (var j = colStart; j < colEnd; j++)
            {
                if (Grid[i, j].IsAvailable || _bindingTiles.SideTile == Grid[i, j]) continue;
                _bindingTiles.SideTile = Grid[i, j];
                Grid.BindingTiles.Bind(Grid[i, j], _bindingTiles);
                return;
            }
        }

        public override bool Apply(MazeTile tile)
        {
            return CheckPathCoverage(tile);
        }

        private bool CheckPathCoverage(MazeTile currentTile)
        {
            float distance;
            if (_nearestTile == null)
            {
                foreach (var tile in _bindingTiles.ToArray())
                {
                    if (Grid.BindingTiles.HasValue(tile, _bindingTiles)) continue;

                    distance = tile.GetDistance(currentTile);
                    if (!(distance < _currentDistance)) continue;
                    _currentDistance = distance;
                    _nearestTile = tile;
                }
            }

            if (_nearestTile == null) return true;

            distance = _nearestTile.GetDistance(currentTile);
            if (distance == 0)
            {
                _nearestTile = null;
                _currentDistance = float.MaxValue;
                Grid.BindingTiles.Bind(currentTile, _bindingTiles);
                return true;
            }

            if (distance > _currentDistance)
            {
                return false;
            }

            _currentDistance = distance;
            return true;
        }
    }
}