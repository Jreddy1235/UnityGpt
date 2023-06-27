using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePlaceMechanicsOnGrid : MazeBaseAction
    {
        private List<List<MazeTile>> _walkableTiles;
        private int TilesCount => _walkableTiles.Sum(list => list.Count);
        protected override TaskStatus OnUpdate()
        {
            foreach (var rule in Rules)
            {
                rule?.Apply();
            }
            Grid.IndexOfPath.Sort((a,b)=> a.CompareTo(b));
            
            var mechanics = Configuration.Obstacles.Where(x => x.AssociateElementIds.Length > 0).ToList();
            mechanics.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            _walkableTiles = Grid.PathTileForElementPlacement();
            foreach (var obstacle in mechanics)
            {
                var amount = Random.Range(obstacle.Amount.Min, obstacle.Amount.Max + 1);
                for (var i = 0; i < amount; i++)
                {
                    if (TilesCount <= 0) break;
                    if (TilesCount < obstacle.AssociateElementIds.Length + 1) continue;
                    
                    var tile = GetTileForMechanic();
                    if (tile == null) break;
                    tile.Value = obstacle.Id;
                    tile.IsFrozen = true;
                    foreach (var associateElement in obstacle.AssociateElementIds)
                    {
                        var tile1 = GetTileForMechanic();
                        if (tile1 == null) break;
                        tile1.Value = associateElement;
                        tile1.IsFrozen = true;
                    }
                }
            }

            return base.OnUpdate();
        }
        
        private MazeTile GetTileForMechanic()
        {
            foreach (var index in Grid.IndexOfPath)
            {
                var tile = GetRandomTileFromPath(index);
                if (tile == null || tile.IsFrozen) continue;
                
                _walkableTiles[index].Remove(tile);
                return tile;
            }

            return null;
        }

        private MazeTile GetRandomTileFromPath(int index)
        {
            if (index >= _walkableTiles[index].Count) return null;
            
            return _walkableTiles[index][Random.Range(0, _walkableTiles[index].Count)];
        }
    }
}