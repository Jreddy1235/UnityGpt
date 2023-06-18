using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazeCreateEmptyGrid : MazeBaseAction
    {
        private const int NoTileId = -2;

        protected override TaskStatus OnUpdate()
        {
            var gridSize = Configuration.GridSize;
            var grid = new int[gridSize.x, gridSize.y];
            for (var x = 0; x < gridSize.x; x++)
            {
                for (var y = 0; y < gridSize.y; y++)
                {
                    grid[x, y] = NoTileId;
                }
            }
            GridController.SetGrid(grid);
            return TaskStatus.Success;
        }

    }
}