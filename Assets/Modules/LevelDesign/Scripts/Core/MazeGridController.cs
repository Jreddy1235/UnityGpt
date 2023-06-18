using System;
using TypeReferences;
using UnityEngine;

namespace UnityGPT
{
    public class MazeGridController : MonoBehaviour
    {
        public MazeBasePathRule[] PathRules { get; private set; }
        public MazeGrid Grid { get; private set; }
        public MazeGridConfiguration Configuration { get; private set; }

        public void SetData(MazeGridConfiguration configuration, MazeBasePathRule[] pathRules)
        {
            Configuration = configuration;
            PathRules = pathRules;
        }

        public void SetGrid(int[,] grid)
        {
            Grid = new MazeGrid(grid);
        }
    }
}