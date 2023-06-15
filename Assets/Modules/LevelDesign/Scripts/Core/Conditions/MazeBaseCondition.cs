using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazeBaseCondition : ConditionBase
    {
        protected MazeGridController GridController { get; private set; }

        protected override void OnInit()
        {
            base.OnInit();
            if (Owner != null)
                GridController = Owner.GetComponent<MazeGridController>();
        }

        protected override bool OnUpdate()
        {
            return true;
        }
    }
}