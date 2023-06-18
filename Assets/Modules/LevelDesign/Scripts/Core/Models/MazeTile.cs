using System.Collections.Generic;

namespace UnityGPT
{
    public class MazeTile
    {
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public int Value { get; set; }
        public bool HasNoTile { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAvailable => !HasNoTile && !IsLocked;
    }
}