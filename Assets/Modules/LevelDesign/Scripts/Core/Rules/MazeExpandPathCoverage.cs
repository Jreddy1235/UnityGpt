using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityGPT
{
    public class MazeExpandPathCoverage : MazeBasePathRule
    {
        private readonly MazeGridBindingTiles _bindingTiles = new();

        private MazeTile _nearestTile;
        private float _currentDistance = float.MaxValue;

        public override void OnInit()
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
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, 0];
                _bindingTiles.SecondCornerTile = Grid[Grid.RowCount - 1, Grid.ColumnCount - 1];
            }
            else if (sideTile.RowIndex == Grid.RowCount - 1)
            {
                _bindingTiles.FirstCornerTile = Grid[0, 0];
                _bindingTiles.SecondCornerTile = Grid[0, Grid.ColumnCount - 1];
            }
            else if (sideTile.ColIndex == 0)
            {
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, Grid.ColumnCount - 1];
                _bindingTiles.SecondCornerTile = Grid[0, Grid.ColumnCount - 1];
            }
            else if (sideTile.ColIndex == Grid.ColumnCount - 1)
            {
                _bindingTiles.FirstCornerTile = Grid[Grid.RowCount - 1, 0];
                _bindingTiles.SecondCornerTile = Grid[0, 0];
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

                    distance = GetDistance(tile, currentTile);
                    if (!(distance < _currentDistance)) continue;
                    _currentDistance = distance;
                    _nearestTile = tile;
                }
            }

            if (_nearestTile == null) return true;

            distance = GetDistance(_nearestTile, currentTile);
            if (distance == 0)
            {
                _nearestTile = null;
                _currentDistance = float.MaxValue;
                Grid.BindingTiles.Bind(currentTile, _bindingTiles);
                return true;
            }

            if (distance <= _currentDistance)
            {
                _currentDistance = distance;
                return true;
            }

            return false;
        }

        private float GetDistance(MazeTile tile, MazeTile otherTile)
        {
            return Mathf.Pow(tile.RowIndex - otherTile.RowIndex, 2) + Mathf.Pow(tile.ColIndex - otherTile.ColIndex, 2);
        }
    }
}