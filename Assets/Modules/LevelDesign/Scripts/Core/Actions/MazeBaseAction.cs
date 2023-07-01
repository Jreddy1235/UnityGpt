using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;

namespace UnityGPT
{
    public class MazeBaseAction : ActionBase
    {
        public MazeBaseRule[] Rules { get; set; }
        public MazeBaseRule FirstRule => Rules.Length > 0 ? Rules[0] : null;
        protected MazeGridController GridController { get; private set; }
        protected MazeGrid Grid => GridController.Grid;
        protected MazeGridConfiguration Configuration => GridController.Configuration;

        protected override void OnInit()
        {
            base.OnInit();
            if (Owner != null)
                GridController = Owner.GetComponent<MazeGridController>();

            if (!(Rules?.Length > 0)) return;
            
            foreach (var rule in Rules)
            {
                rule.SetData(Grid, Configuration);
            }
        }

        protected override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}