using UnityEngine;


namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Visual Designer Tool", fileName = "GptVisualDesigner")]
    public class GptVisualDesigner : ScriptableObject
    {
        [SerializeField] private GptSettings settings;
        [SerializeField] private string gridString;
        [SerializeField] private int rows;
        [SerializeField] private int columns;

        public GptSettings Settings => settings;
        
        private void Reset()
        {
            settings = Resources.Load<GptSettings>("GptSettings");
        }
    }
}
