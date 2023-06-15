using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;

namespace UnityGPT
{
    public class MazeBaseAction : ActionBase
    {
        protected MazeGridController GridController { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            if (Owner != null)
                GridController = Owner.GetComponent<MazeGridController>();
        }

        protected override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}