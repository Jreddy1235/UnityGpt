using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazeExpandPathCoverage : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            ExpandExistingPaths();
            return TaskStatus.Success;
        }

        private void ExpandExistingPaths()
        {
            var repeatOnce = true;
            while (true)
            {
                foreach (var tile in Grid.PathsMapping.SelectMany(t => t.Value.Paths.SelectMany(paths => paths.Value)
                             .OrderBy(_ => Guid.NewGuid()).Concat(Grid.CoveragePaths.SelectMany(paths => paths))))
                {
                    var path = new Stack<MazeTile>();
                    path.Push(tile);

                    foreach (var rule in Rules) rule.Reset();
                    var isPathGenerated = GeneratePath(path, true) && path.Count > 1;
                    if (!isPathGenerated) continue;

                    path.MarkPathTiles();
                    Grid.CoveragePaths.Add(path);
                    Grid.ToList().UnFreezeTiles();

                    if (Grid.BindingTiles.HasValue()) break;
                }

                if (!repeatOnce || Grid.BindingTiles.HasValue()) break;
                repeatOnce = false;
            }

            Grid.ToList().UnFreezeTiles();
        }

        private bool GeneratePath(Stack<MazeTile> path, bool isFirstTile = false)
        {
            var currentTile = path.Peek();
            var value = Grid.BindingTiles.Value;

            if (currentTile == null || Rules.Any(t => !((t.IsSkipFirstTile && isFirstTile) || t.Apply(currentTile))))
            {
                path.Pop();
                return false;
            }

            if (Grid.BindingTiles.Value > value || Grid.BindingTiles.HasValue()) return true;

            currentTile.IsFrozen = true;
            Grid.SelectedTile = currentTile;
            var neighbors = currentTile.Neighbors.ToArray().OrderBy(_ => Guid.NewGuid());
            foreach (var neighbor in neighbors)
            {
                if (neighbor == null || neighbor.IsFrozen) continue;

                path.Push(neighbor);
                if (GeneratePath(path))
                    break;

                neighbor.IsFrozen = false;
            }

            return Grid.BindingTiles.Value > value || Grid.BindingTiles.HasValue();
        }
    }
}