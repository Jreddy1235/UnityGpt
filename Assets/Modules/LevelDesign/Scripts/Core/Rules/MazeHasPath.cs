namespace UnityGPT
{
    public class MazeHasPath : MazeBaseRule
    {
        public override bool Apply(MazeTile tile)
        {
            return tile is {IsAvailable: false} && tile.Value != MazeConstants.NoTileId;
        }
    }
}