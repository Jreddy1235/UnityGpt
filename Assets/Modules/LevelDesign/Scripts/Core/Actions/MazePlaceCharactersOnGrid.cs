using CleverCrow.Fluid.BTs.Tasks;

namespace UnityGPT
{
    public class MazePlaceCharactersOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            foreach (var character in Configuration.Characters)
            {
                ActionRule.Apply();
                var random = character.Amount.Min;
                for (var i = 0; i < random; i++)
                {
                    if (Grid.SelectedTile == null) continue;

                    Grid.SelectedTile.Value = character.Id;
                    Grid.SelectedTile.HasCharacter = true;
                    Grid.PathsMapping.Add(Grid.SelectedTile, new() {StartTile = Grid.SelectedTile});
                }
            }

            return base.OnUpdate();
        }
    }
}