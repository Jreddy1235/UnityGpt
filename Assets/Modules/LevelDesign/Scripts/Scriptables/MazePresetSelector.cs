using System;
using System.Linq;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Preset Selector", fileName = "MazePresetSelector")]
    public class MazePresetSelector : ScriptableObject
    {
        [SerializeField] private MazeSettings settings;
        [SerializeField] private GridCreatorPreset[] presets;

        public MazeSettings Settings => settings;

        public MazeGridCreator GetGridCreator(string presetName)
        {
            var gridCreator = presets.Where(preset => preset.Name == presetName)
                .Select(preset => preset.GridCreator)
                .FirstOrDefault();

            if (gridCreator == null)
                gridCreator = presets.First().GridCreator;
            return gridCreator;
        }

        private void Reset()
        {
            settings = Resources.Load<MazeSettings>("MazeSettings");
        }

        [Serializable]
        private class GridCreatorPreset
        {
            [SerializeField] private string name;
            [SerializeField] private MazeGridCreator gridCreator;

            public string Name => name;
            public MazeGridCreator GridCreator => gridCreator;
        }
    }
}