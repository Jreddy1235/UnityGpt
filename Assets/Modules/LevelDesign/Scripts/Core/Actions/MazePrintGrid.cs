using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePrintGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            var gridSize = Configuration.GridSize;
            var grid = GridController.Grid;
            var output = "";
            for (var x = 0; x < gridSize.x; x++)
            {
                output += "\n";
                for (var y = 0; y < gridSize.y; y++)
                {
                    output += grid[x, y].Value + " ";
                }
            }

            Debug.Log(output);

            return base.OnUpdate();
        }
    }
}