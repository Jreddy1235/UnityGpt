using System.Linq;

namespace UnityGPT
{
    public class MazeIsTileAvailable : MazeBasePathRule
    {
        public override bool Apply(MazeTile tile)
        {
            return (tile.IsAvailable || tile.HasCharacter) && !IsAdjacentTileOccupied(tile);
        }
        
        private bool IsAdjacentTileOccupied(MazeTile tile)
        {
            return tile.Neighbors.ToArray().Count(neighbor => neighbor is {IsAvailable: false}) > 1;
        }
    }
}