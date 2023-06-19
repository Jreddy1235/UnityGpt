using System;
using System.Linq;

namespace UnityGPT
{
    public class MazeSelectRandomTile : MazeBaseRule
    {
        public override void Apply()
        {
            Grid.SelectedTile = Grid.ToList()
                .Where(t => t.IsAvailable && t.Neighbors.ToArray().All(x => x?.IsAvailable ?? true))
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}