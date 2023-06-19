using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;

namespace UnityGPT
{
    public class MazeBaseAction : ActionBase
    {
        public MazeBaseRule ActionRule { get; set; }

        protected MazeGridController GridController { get; private set; }
        protected MazeGrid Grid => GridController.Grid;
        protected MazeGridConfiguration Configuration => GridController.Configuration;

        protected override void OnInit()
        {
            base.OnInit();
            if (Owner != null)
                GridController = Owner.GetComponent<MazeGridController>();
            
            ActionRule?.SetData(Grid);
        }

        protected override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}