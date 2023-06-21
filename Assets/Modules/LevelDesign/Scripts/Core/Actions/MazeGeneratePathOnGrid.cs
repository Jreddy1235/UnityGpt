using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UnityGPT
{
    public class MazeGeneratePathOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            foreach (var collectable in Configuration.Collectables)
            {
                var random = collectable.Amount.Min;
                var pathLength = collectable.PathLength.Min + 1;
                for (var i = 0; i < random; i++)
                {
                    if (!Grid.BindingTiles.HasValue())
                    {
                        Grid.BindingTiles.Reset();
                    }

                    var startTile = GetPathStartTile(collectable.Id);
                    if (startTile == null || !GeneratePath(startTile, pathLength, out var path))
                        continue;
                    var endTile = path.Peek();
                    endTile.Value = collectable.Id;
                    Grid.PathsMapping[startTile].Paths.Add(endTile, path);
                    break;
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
            if (currentTile == null || GridController.PathFindingRules.Any(rule => !rule.Apply(currentTile)))
            {
                RevertTileInPath(path, currentTile);
                return false;
            }

            if (IsPathCompleted(path, pathLength)) return true;

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

            if (IsPathCompleted(path, pathLength)) return true;

            RevertTileInPath(path, currentTile);
            return false;
        }

        private void RevertTileInPath(Stack<MazeTile> path, MazeTile currentTile)
        {
            path.Pop();
            Grid.BindingTiles.Reset(currentTile);
        }

        private bool IsPathCompleted(ICollection path, int pathLength)
        {
            return path.Count >= pathLength && Grid.BindingTiles.HasValue();
        }
    }
}