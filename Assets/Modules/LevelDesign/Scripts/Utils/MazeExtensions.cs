using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace UnityGPT
{
    public static class MazeExtensions
    {
        public static void MarkPathTiles(this IEnumerable<MazeTile> path)
        {
            foreach (var tile in path)
            {
                if (tile.Value == MazeConstants.NoTileId)
                    tile.Value = MazeConstants.TileOnlyId;
            }
        }

        public static void UnFreezeTiles(this IEnumerable<MazeTile> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.IsFrozen = false;
            }
        }

        public static float GetDistance(this MazeTile tile, MazeTile otherTile)
        {
            return Mathf.Pow(tile.RowIndex - otherTile.RowIndex, 2) +
                   Mathf.Pow(tile.ColumnIndex - otherTile.ColumnIndex, 2);
        }

        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            return new Stack<T>(new Stack<T>(stack));
        }
    }
}