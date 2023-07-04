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

        protected Dictionary<float, List<Obstacle>> SeparateByPriority(List<Obstacle> elementList)
        {
            var dictionary = new Dictionary<float, List<Obstacle>>();
            foreach (var element in elementList)
            {
                var instances = new List<Obstacle>();
                var amount = Random.Range(element.Amount.Min, element.Amount.Max + 1);
                for (var i = 0; i < amount; i++)
                {
                    instances.Add(element);
                }
                
                var priority = element.Priority;
                if (dictionary.ContainsKey(priority))
                    dictionary[priority].AddRange(instances);
                else
                    dictionary.Add(priority, instances);
            }

            return dictionary;
        }
        
        protected void ShuffleAndPlaceElement(Dictionary<float, List<Obstacle>> separatedByPriority)
        {
            var shuffledElementList = new List<Obstacle>();
            foreach (var element in separatedByPriority)
            {
                shuffledElementList.AddRange(element.Value.OrderBy(x => Random.value));
            }

            PlaceElementOnGrid(shuffledElementList);
        }
        
        private void PlaceElementOnGrid(List<Obstacle> elementList)
        {
            foreach (var element in elementList)
            {
                if (TilesCount <= 0) break;
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
        
        private MazeTile GetTileForElement()
        {
            bool doNotSkipForPathTiles = true;
            foreach (var index in Grid.IndexOfPath)
            {
                if (index == 3 && inventoryRule != null)
                {
                    doNotSkipForPathTiles = inventoryRule.Apply(null); 
                    if (!doNotSkipForPathTiles) continue;
                }
                var tile = GetRandomTileFromPath(index);
                if (tile == null || tile.HasObstacle) continue;
                
                _walkableTiles[index].Remove(tile);
                return tile;
            }

            return GetEmptyTile(doNotSkipForPathTiles);
        }
        
        private MazeTile GetTileForAssociateElement()
        {
            foreach (var index in Grid.IndexOfPath)
            {
                var tile = GetRandomTileFromPath(index);
                if (tile == null || tile.HasObstacle || tile.Neighbors.AnyNeighbourHasCollectable()) continue;
                
                _walkableTiles[index].Remove(tile);
                return tile;
            }

            return GetEmptyTile();
        }
        
        private MazeTile GetEmptyTile(bool doNotSkipForPathTiles = true)
        {
            foreach (var tile in Grid.PathMappingTiles())
            {
                if (!doNotSkipForPathTiles) break;
                if (tile.Value != MazeConstants.TileOnlyId) continue;

                return tile;
            }
            
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