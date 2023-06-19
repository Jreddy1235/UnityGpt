namespace UnityGPT
{
    public abstract class MazeBasePathRule : MazeBaseRule
    {
        public virtual bool Apply(MazeTile tile)
        {
            return true;
        }

        public override void Apply()
        {
        }
    }
}