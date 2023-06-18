using System.Collections.Generic;

namespace UnityGPT
{
    public abstract class MazeBasePathRule
    {
        protected MazeGrid Grid { get; private set; }
        
        public void SetData(MazeGrid grid, int characterId)
        {
            Grid = grid;
            if(!grid.Paths.ContainsKey(characterId))
                grid.Paths.Add(characterId, new Stack<MazeTile>());
        }

        public abstract void Apply();
    }
}