using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using UnityEngine;

namespace UnityGPT
{
    public class MazePlaceShortcutsOnGrid : MazeBaseAction
    {
        protected override TaskStatus OnUpdate()
        {
            foreach (var tile in Grid.ToList().Where(t => t.IsAvailable))
            {
                if (Rules.Any(t => !t.Apply(tile)) || !CanPlaceShortcut()) continue;

                var shortcut = new Stack<MazeTile>();
                shortcut.Push(tile);
                Debug.LogError(tile.RowIndex + " x " + tile.ColumnIndex);
                shortcut.MarkPathTiles();
                Grid.Shortcuts.Add(shortcut);
            }

            return base.OnUpdate();
        }

        private bool CanPlaceShortcut()
        {
            return Random.value < Configuration.ShortcutsInfo.Frequency;
        }
    }
}