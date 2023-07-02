using System;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Preset Selector", fileName = "MazePresetSelector")]
    public class MazePresetSelector : ScriptableObject
    {
        [SerializeField] private MazeSettings settings;

        [SerializeField] [Dropdown("GetPresetNames")]
        private string defaultPreset;

        [SerializeField] private GridCreatorPreset[] presets;

        public MazeSettings Settings => settings;

        [UsedImplicitly]
        public string[] GetPresetNames()
        {
            return presets.Where(t => t.GridCreator != null).Select(preset => preset.Name).ToArray();
        }

        public MazeGridCreator GetGridCreator(string presetName)
        {
            if (string.IsNullOrEmpty(presetName))
                presetName = defaultPreset;

            presetName = presetName.ToLower();
            var gridCreator = presets.Where(preset => presetName.Contains(preset.Name.ToLower()))
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