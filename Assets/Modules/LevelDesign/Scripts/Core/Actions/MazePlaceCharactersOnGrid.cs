using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazePlaceCharactersOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            foreach (var character in Configuration.Characters)
            {
                Grid.SelectedTile.Value = character.Id; 
            }
            return base.OnUpdate();
        }
    }
}