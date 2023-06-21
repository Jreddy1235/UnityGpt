using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using Random = UnityEngine.Random;

namespace UnityGPT
{
    public class MazeCreateMockPaths : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            GenerateMockPaths();
            return base.OnUpdate();
        }

        private void GenerateMockPaths()
        {
            foreach (var pathInfo in Grid.PathsMapping)
            {
                foreach (var tile in pathInfo.Value.Paths.SelectMany(paths => paths.Value))
                {
                    if (!CanCreateMockPath()) continue;

                    var path = new Stack<MazeTile>();
                    path.Push(tile);
                    var isPathGenerated = GeneratePath(path, true);
                    if (isPathGenerated)
                        path.MarkPathTiles();
                    Grid.MockPaths.Add(path);
                }
            }

            Grid.ToList().UnFreezeTiles();
        }

        private bool CanCreateMockPath()
        {
            return Random.value < Configuration.MockPathInfo.Frequency;
        }

        private bool CanExitMockPath()
        {
            return Random.value < Configuration.MockPathInfo.ExitChances;
        }

        private bool GeneratePath(Stack<MazeTile> path, bool isFirstTile = false)
        {
            var currentTile = path.Peek();
            if (currentTile == null ||
                (!isFirstTile && GridController.PathFindingRules.Any(rule => !rule.Apply(currentTile))))
            {
                path.Pop();
                return false;
            }

            if (CanExitMockPath()) return path.Count > 1;

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

            return path.Count > 1;
        }
    }
}