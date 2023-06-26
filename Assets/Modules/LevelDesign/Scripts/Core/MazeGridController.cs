using UnityEngine;

namespace UnityGPT
{
    public class MazeGridController : MonoBehaviour
    {
        public MazeGrid Grid { get; private set; }
        public MazeGridConfiguration Configuration { get; private set; }

        public void SetData(MazeGridConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void SetGrid(int[,] grid)
        {
            Grid = new MazeGrid(grid);
        }
    }
}