using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGPT
{
    public class MazeBasePlaceElementsOnGrid : MazeBaseAction
    {
        protected MazeBaseRule inventoryRule;
            
        private List<List<MazeTile>> _walkableTiles;
        private int TilesCount => _walkableTiles.Sum(list => list.Count);
        
        protected void ElementPlacementPreProcess()
        {
            Grid.IndexOfPath.Clear();
            foreach (var rule in Rules)
            {
                if (rule is MazeHasInventoryItems)
                    inventoryRule = rule;
                else
                    rule.Apply();
            }
            Grid.IndexOfPath.Sort((a,b)=> a.CompareTo(b));
            
            _walkableTiles = Grid.PathTileForElementPlacement();
            foreach (var path in _walkableTiles)
            {
                path.RemoveAll(x => x.Value != MazeConstants.TileOnlyId);
            }
        }

        protected void PlaceElementOnGrid(List<Obstacle> elementList)
        {
            foreach (var element in elementList)
            {
                var amount = Random.Range(element.Amount.Min, element.Amount.Max + 1);
                for (var i = 0; i < amount; i++)
                {
                    if (TilesCount <= 0 || (inventoryRule != null && !inventoryRule.Apply(null))) break;
                    if (TilesCount < element.AssociateElementIds.Length + 1) continue;
                    
                    var tile = GetTileForElement();
                    if (tile == null) break;
                    tile.Value = element.Id;
                    tile.HasObstacle = true;
                    foreach (var associateElement in element.AssociateElementIds)
                    {
                        var tile1 = GetTileForAssociateElement();
                        if (tile1 == null) break;
                        tile1.Value = associateElement;
                        tile1.HasObstacle = true;
                    }
                }
            }
        }
        
        private MazeTile GetTileForElement()
        {
            foreach (var index in Grid.IndexOfPath)
            {
                var tile = GetRandomTileFromPath(index);
                if (tile == null || tile.HasObstacle) continue;
                
                _walkableTiles[index].Remove(tile);
                return tile;
            }

            return GetEmptyTile();
        }
        
        private MazeTile GetTileForAssociateElement()
        {
            foreach (var index in Grid.IndexOfPath)
            {
                if (index != 3) continue;
                
                var tile = GetRandomTileFromPath(index);
                if (tile == null || tile.HasObstacle || tile.Neighbors.AnyNeighbourHasCollectable()) continue;
                
                _walkableTiles[index].Remove(tile);
                return tile;
            }

            return GetEmptyTile();
        }
        
        private MazeTile GetEmptyTile()
        {
            foreach (var tile in Grid.ToList())
            {
                if (tile.Value != MazeConstants.TileOnlyId) continue;

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