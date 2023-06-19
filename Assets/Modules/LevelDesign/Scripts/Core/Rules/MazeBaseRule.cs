namespace UnityGPT
{
    public abstract class MazeBaseRule
    {
        protected MazeGrid Grid { get; private set; }

        public abstract void Apply();
        
        public void SetData(MazeGrid grid)
        {
            Grid = grid;
        }
    }
}