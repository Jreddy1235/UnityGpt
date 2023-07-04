using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazeRemoveShorterPaths : MazeBaseAction
    {
        private readonly Dictionary<Stack<MazeTile>, int> _paths = new();

        protected override TaskStatus OnUpdate()
        {
            foreach (var startTile in Grid.PathsMapping.Select(t => t.Key))
            {
                var path = new Stack<MazeTile>();
                path.Push(startTile);
                FindPaths(path);
            }

            RemoveShorterPaths();
            return TaskStatus.Success;
        }

        private void FindPaths(Stack<MazeTile> path)
        {
            var currentTile = path.Peek();
            foreach (var neighbor in currentTile.Neighbors.ToArray())
            {
                if (path.Contains(neighbor)) continue;

                var newPath = path.Clone();
                newPath.Push(neighbor);

                if (neighbor is {HasCollectable: true})
                {
                    AddPath(newPath);
                    continue;
                }

                if (Rules.All(rule => rule.Apply(neighbor)))
                {
                    FindPaths(newPath);
                }
            }
        }

        private void AddPath(Stack<MazeTile> path)
        {
            _paths[path] = path.Count(t => t.HasObstacle);
        }

        private void RemoveShorterPaths()
        {
            foreach (var (path, reqCount) in _paths)
            {
                if (reqCount >= Configuration.ReqInventoryItems.Min) continue;
                
                var tiles = Grid.Shortcuts.SelectMany(t => t);
                foreach (var tile in tiles)
                {
                    if (!path.Contains(tile)) continue;
                    tile.Value = MazeConstants.NoTileId;
                }
            }
        }
    }
}