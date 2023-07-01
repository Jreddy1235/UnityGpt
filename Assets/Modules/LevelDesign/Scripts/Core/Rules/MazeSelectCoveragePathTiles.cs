using System;
using System.Linq;

namespace UnityGPT
{
    public class MazeSelectCoveragePathTiles : MazeBaseRule
    {
        public override void Apply()
        {
            Grid.IndexOfPath.Add(1);
        }
    }
}