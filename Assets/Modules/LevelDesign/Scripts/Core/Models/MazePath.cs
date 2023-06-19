using System.Collections.Generic;

namespace UnityGPT
{
    public class MazePath
    {
        public MazeTile StartTile { get; set; }
        public Dictionary<MazeTile, Stack<MazeTile>> Routes { get; set; } = new();
    }
}