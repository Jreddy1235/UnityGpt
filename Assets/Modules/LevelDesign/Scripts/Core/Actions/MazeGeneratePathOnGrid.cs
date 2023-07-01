using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using Random = UnityEngine.Random;

namespace UnityGPT
{
    public class MazeGeneratePathOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            foreach (var collectable in Configuration.Collectables)
            {
                var random = Random.Range(collectable.Amount.Min, collectable.Amount.Max);
                var pathLength = Random.Range(collectable.PathLength.Min, collectable.PathLength.Max) + 1;
                for (var i = 0; i < random; i++)
                {
                    var startTile = GetPathStartTile(collectable.Id);
                    if (startTile == null) continue;
                    var iterations = Configuration.Iterations;
                    var isPathGenerated = false;
                    var path = new Stack<MazeTile>();
                    while (iterations-- > 0)
                    {
                        isPathGenerated =
                            GeneratePath(startTile, pathLength - (Configuration.Iterations - iterations), out path);
                        if (isPathGenerated || pathLength < Configuration.Iterations - iterations + 2) break;
                    }

                    if (!isPathGenerated) continue;
                    var endTile = path.Peek();
                    endTile.Value = collectable.Id;
                    Grid.PathsMapping[startTile].Paths.Add(endTile, path);
                }
            }

            return TaskStatus.Success;
        }

        private MazeTile GetPathStartTile(int collectableId)
        {
            var characterId = Configuration.GetMappedCharacter(collectableId);
            if (characterId == -1) return null;
            var characterTiles = Grid.PathsMapping.Where(t => t.Key.Value == characterId).Select(t => t.Key).ToList();
            return characterTiles.Count == 0 ? null : characterTiles[Random.Range(0, characterTiles.Count)];
        }

        private bool GeneratePath(MazeTile startTile, int pathLength, out Stack<MazeTile> path)
        {
            path = new Stack<MazeTile>();
            path.Push(startTile);
            var isPathGenerated = GeneratePath(path, pathLength);
            if (isPathGenerated)
                path.MarkPathTiles();
            Grid.ToList().UnFreezeTiles();
            return isPathGenerated;
        }

        private bool GeneratePath(Stack<MazeTile> path, int pathLength)
        {
            var currentTile = path.Peek();
            if (currentTile == null || Rules.Any(rule => !rule.Apply(currentTile)))
            {
                path.Pop();
                return false;
            }

            if (path.Count >= pathLength) return true;

            currentTile.IsFrozen = true;
            Grid.SelectedTile = currentTile;
            var neighbors = currentTile.Neighbors.ToArray().OrderBy(_ => Guid.NewGuid());
            foreach (var neighbor in neighbors)
            {
                if (neighbor == null || neighbor.IsFrozen) continue;

                path.Push(neighbor);
                if (GeneratePath(path, pathLength))
                    break;

                neighbor.IsFrozen = false;
            }

            if (path.Count >= pathLength) return true;

            path.Pop();
            return false;
        }
    }
}