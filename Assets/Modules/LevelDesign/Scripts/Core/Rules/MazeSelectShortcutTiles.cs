using System;
using System.Linq;

namespace UnityGPT
{
    public class MazeSelectShortcutTiles : MazeBaseRule
    {
        public override void Apply()
        {
            Grid.IndexOfPath.Add(0);
        }
    }
}