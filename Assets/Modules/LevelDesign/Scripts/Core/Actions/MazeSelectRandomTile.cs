using System;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazeSelectRandomTile : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            Grid.SelectedTile = Grid.ToList()
                .Where(t => t.IsAvailable)
                .OrderBy(t => Guid.NewGuid())
                .FirstOrDefault();
            return base.OnUpdate();
        }
    }
}