using System;
using System.Linq;

namespace UnityGPT
{
    public class MazeSelectPathMappingTiles : MazeBaseRule
    {
        public override void Apply()
        {
            Grid.IndexOfPath.Add(3);
        }
    }
}