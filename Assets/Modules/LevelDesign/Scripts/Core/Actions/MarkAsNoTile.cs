using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MarkAsNoTile : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            if (Owner != null)
                Debug.LogError(Owner.name);
            
            Debug.LogError(GetType().Name);
            return TaskStatus.Success;
        }
    }
}