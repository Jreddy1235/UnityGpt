using System.Collections.Generic;

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
    }
}