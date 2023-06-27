using System;
using System.Linq;

namespace UnityGPT
{
    public class MazeSelectMockPathTiles : MazeBaseRule
    {
        public override void Apply()
        {
            Grid.IndexOfPath.Add(2);
        }
    }
}