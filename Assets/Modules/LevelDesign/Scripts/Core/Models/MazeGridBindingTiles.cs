namespace UnityGPT
{
    public class MazeGridBindingTiles
    {
        public MazeTile SideTile { get; set; }
        public MazeTile FirstCornerTile { get; set; }
        public MazeTile SecondCornerTile { get; set; }

        public MazeTile[] ToArray()
        {
            return new[] {SideTile, FirstCornerTile, SecondCornerTile};
        }

        public bool HasValue()
        {
            return SideTile != null && FirstCornerTile != null && SecondCornerTile != null;
        }

        public bool HasValue(MazeTile tile, MazeGridBindingTiles bindingTiles)
        {
            return (SideTile != null && bindingTiles.SideTile == tile)
                   || (FirstCornerTile != null && bindingTiles.FirstCornerTile == tile)
                   || (SecondCornerTile != null && bindingTiles.SecondCornerTile == tile);
        }

        public void Bind(MazeTile tile, MazeGridBindingTiles bindingTiles)
        {
            if (SideTile == null && bindingTiles.SideTile == tile)
                SideTile = tile;
            else if (FirstCornerTile == null && bindingTiles.FirstCornerTile == tile)
                FirstCornerTile = tile;
            else if (SecondCornerTile == null && bindingTiles.SecondCornerTile == tile)
                SecondCornerTile = tile;
        }

        public void Reset()
        {
            SideTile = null;
            FirstCornerTile = null;
            SecondCornerTile = null;
        }

        public void Reset(MazeTile tile)
        {
            if (tile == SideTile)
                SideTile = null;
            else if (tile == FirstCornerTile)
                FirstCornerTile = null;
            else if (tile == SecondCornerTile)
                SecondCornerTile = null;
        }
    }
}