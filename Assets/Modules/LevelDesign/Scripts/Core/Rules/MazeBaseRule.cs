namespace UnityGPT
{
    public abstract class MazeBaseRule
    {
        public virtual bool IsSkipFirstTile { get; set; }
        protected MazeGrid Grid { get; private set; }
        protected MazeGridConfiguration Configuration { get; private set; }
        public virtual bool Apply(MazeTile tile) => true;
        public virtual void Apply(){}

        protected virtual void OnInit()
        {
        }

        public void SetData(MazeGrid grid, MazeGridConfiguration configuration)
        {
            Grid = grid;
            Configuration = configuration;
            OnInit();
        }

        public virtual void Reset(){}
    }
}