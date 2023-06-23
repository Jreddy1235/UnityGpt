using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePlaceObstaclesOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            var sortedObstacles = Configuration.Obstacles.ToList();
            sortedObstacles.Sort((a, b) => b.Weight.CompareTo(a.Weight));
            var walkableTiles = GetWalkableTiles();
            foreach (var obstacle in sortedObstacles)
            {
                var amount = Random.Range(obstacle.Amount.Min, obstacle.Amount.Max + 1);
                for (var i = 0; i < amount; i++)
                {
                    if (walkableTiles.Count <= 0) break;

                    var tile = walkableTiles[Random.Range(0, walkableTiles.Count)];
                    tile.Value = obstacle.Id;
                    walkableTiles.Remove(tile);
                }
            }

            return base.OnUpdate();
        }

        private List<MazeTile> GetWalkableTiles()
        {
            var walkableTiles = new List<MazeTile>();
            foreach (var pathInfo in Grid.PathsMapping.Values)
            {
                walkableTiles.AddRange(pathInfo.Paths.Values.SelectMany(path =>
                    path.Where(tile => tile != path.First() && tile != path.Last())));
            }

            return walkableTiles;
        }
    }
}