using System.Collections.Generic;

namespace UnityGPT
{
    public class MazePathInfo
    {
        public MazeTile StartTile { get; set; }
        public Dictionary<MazeTile, Stack<MazeTile>> Paths { get; set; } = new();
    }
}