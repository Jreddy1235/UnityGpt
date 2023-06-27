using System.Linq;

namespace UnityGPT
{
    public class MazeCanTileBeShortcut : MazeBaseRule
    {
        public override bool Apply(MazeTile tile)
        {
            return tile.IsAvailable && AreAdjacentTilesOccupied(tile);
        }

        private bool AreAdjacentTilesOccupied(MazeTile tile)
        {
            var neighbors = tile.Neighbors.ToArray().Where(neighbor => neighbor is {IsAvailable: false}).ToList();
            if (neighbors.Count == 2)
            {
                if (neighbors[0].RowIndex == neighbors[1].RowIndex ||
                    neighbors[0].ColumnIndex == neighbors[1].ColumnIndex)
                {
                    return true;
                }

                var commonNeighbors = neighbors[0].Neighbors.ToArray()
                    .Intersect(neighbors[1].Neighbors.ToArray()).ToList();

                if (commonNeighbors.Count == 1 && commonNeighbors[0].IsAvailable)
                    return true;
            }

            return false;
        }
    }
}