using UnityEngine;


namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Level Design/Maze Visual Designer Tool", fileName = "MazeVisualDesigner")]
    public class MazeVisualDesigner : ScriptableObject
    {
        [SerializeField] private MazeSettings settings;
        [SerializeField] private string gridString;
        [SerializeField] private int rows;
        [SerializeField] private int columns;

        public MazeSettings Settings => settings;
        
        private void Reset()
        {
            settings = Resources.Load<MazeSettings>("MazeSettings");
        }
    }
}
