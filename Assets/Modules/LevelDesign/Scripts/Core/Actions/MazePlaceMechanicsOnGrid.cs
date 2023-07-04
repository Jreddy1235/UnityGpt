using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePlaceMechanicsOnGrid : MazeBasePlaceElementsOnGrid
    {
        protected override TaskStatus OnUpdate()
        {
            ElementPlacementPreProcess();
            inventoryRule = null;
            var mechanics = Configuration.Obstacles.Where(x => x.AssociateElementIds.Length > 0).ToList();
            mechanics.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            ShuffleAndPlaceElement(SeparateByPriority(mechanics));
            return base.OnUpdate();
        }
    }
}