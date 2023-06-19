using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

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
                    var startTile = GetPathStartTile(collectable.Id);
                    if (startTile == null || !GeneratePath(startTile, pathLength, out var path))
                        continue;
                    var endTile = path.Peek();
                    endTile.Value = collectable.Id;
                    Grid.Paths[startTile].Routes.Add(endTile, path);
                    break;
                }
            }

            return TaskStatus.Success;
        }

        private MazeTile GetPathStartTile(int collectableId)
        {
            var characterId = Configuration.GetMappedCharacter(collectableId);
            if (characterId == -1) return null;
            var characterTiles = Grid.Paths.Where(t => t.Key.Value == characterId).Select(t => t.Key).ToList();
            return characterTiles.Count == 0 ? null : characterTiles[Random.Range(0, characterTiles.Count)];
        }

        private bool GeneratePath(MazeTile startTile, int pathLength, out Stack<MazeTile> path)
        {
            path = new Stack<MazeTile>();
            path.Push(startTile);
            var isPathGenerated = GeneratePath(path, pathLength);
            if (isPathGenerated)
                MarkPathTiles(path);
            UnFreezeTiles();
            return isPathGenerated;
        }

        private bool GeneratePath(Stack<MazeTile> path, int pathLength)
        {
            var currentTile = path.Peek();
            if (currentTile == null || GridController.PathFindingRules.Any(rule => !rule.Apply(currentTile)))
            {
                path.Pop();
                return false;
            }

            if (path.Count >= pathLength)
                return true;

            currentTile.IsFrozen = true;
            var neighbors = currentTile.Neighbors.ToArray();
            foreach (var neighbor in neighbors)
            {
                if (neighbor == null || neighbor.IsFrozen) continue;

                path.Push(neighbor);
                if (GeneratePath(path, pathLength))
                    break;

                neighbor.IsFrozen = false;
            }

            if (path.Count >= pathLength)
                return true;

            path.Pop();
            return false;
        }

        private void MarkPathTiles(IEnumerable<MazeTile> path)
        {
            foreach (var tile in path)
            {
                if (tile.Value == MazeConstants.NoTileId)
                    tile.Value = MazeConstants.TileOnlyId;
            }
        }

        private void UnFreezeTiles()
        {
            foreach (var tile in Grid.ToList())
            {
                tile.IsFrozen = false;
            }
        }
    }
}