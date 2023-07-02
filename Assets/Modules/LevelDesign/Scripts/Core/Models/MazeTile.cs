using System.Collections.Generic;

namespace UnityGPT
{
    public class MazeTile
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public int Value { get; set; }
        public bool HasCharacter { get; set; }
        public bool HasObstacle { get; set; }
        public bool HasCollectable { get; set; }
        public bool HasNoTile { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsAvailable => !HasNoTile && !IsFrozen && Value < MazeConstants.TileOnlyId;
        public Neighbours Neighbors { get; set; } = new();

        public class Neighbours
        {
            public MazeTile LeftTile { get; set; }
            public MazeTile RightTile { get; set; }
            public MazeTile TopTile { get; set; }
            public MazeTile BottomTile { get; set; }
            
            public IEnumerable<MazeTile> ToArray()
            {
                return new[] {LeftTile, RightTile, TopTile, BottomTile};
            }
            
            public bool AnyNeighbourHasCollectable()
            {
                var neighbourHasCollectable = false;
                foreach (var tile in ToArray())
                {
                    if (tile == null) continue;
                    
                    neighbourHasCollectable |= tile.HasCollectable;
                }

                return neighbourHasCollectable;
            }
        }
    }
}