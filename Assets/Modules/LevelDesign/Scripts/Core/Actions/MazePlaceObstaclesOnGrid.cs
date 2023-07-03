using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePlaceObstaclesOnGrid : MazeBasePlaceElementsOnGrid
    {
        protected override TaskStatus OnUpdate()
        {
            ElementPlacementPreProcess();
            var sortedObstacles = Configuration.Obstacles.Where(x => x.AssociateElementIds.Length == 0).ToList();
            sortedObstacles.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            ShuffleAndPlaceElement(SeparateByPriority(sortedObstacles));
            return base.OnUpdate();
        }
    }
}