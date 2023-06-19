using UnityEngine;

namespace UnityGPT
{
    public class MazeGridController : MonoBehaviour
    {
        public MazeBasePathRule[] PathFindingRules { get; private set; }
        public MazeGrid Grid { get; private set; }
        public MazeGridConfiguration Configuration { get; private set; }

        public void SetData(MazeGridConfiguration configuration, MazeBasePathRule[] pathRules)
        {
            Configuration = configuration;
            PathFindingRules = pathRules;
        }

        public void SetGrid(int[,] grid)
        {
            Grid = new MazeGrid(grid);

            foreach (var rule in PathFindingRules)
            {
                rule.SetData(Grid);
            }
        }
    }
}