namespace UnityGPT
{
    public abstract class MazeBaseRule
    {
        protected MazeGrid Grid { get; private set; }

        public abstract void Apply();

        public virtual void OnInit()
        {
        }

        public void SetData(MazeGrid grid)
        {
            Grid = grid;
            OnInit();
        }
    }
}