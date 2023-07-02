using System;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/Gpt Prompt Generator", fileName = "GptPromptGenerator")]
    public class GptPromptGenerator : ScriptableObject
    {
        [SerializeField] private string promptFormat = "\"{0}\":\"{1}\"";
        [SerializeField] private string trainingFormat = "\"prompt\":\"{0}\",\"completion\":\"{1}\"";
        [SerializeField] private JsonLine[] lines;

        public string GetPromptText()
        {
            return lines.Aggregate("",
                (current, line) => current + "{" + string.Format(promptFormat, line.Prompt, line.Completion) + "}\n");
        }
        
        private string GetTrainingText()
        {
            return lines.Aggregate("",
                (current, line) => current + "{" + string.Format(trainingFormat, line.Prompt, line.Completion) + "}\n");
        }

        [Button]
        [UsedImplicitly]
        private void PrintPromptText()
        {
            var text = GetPromptText();
            Debug.Log(text);
            GUIUtility.systemCopyBuffer = text;
        }
        
        [Button]
        [UsedImplicitly]
        private void PrintTrainingText()
        {
            var text = GetTrainingText();
            Debug.Log(text);
            GUIUtility.systemCopyBuffer = text;
        }

        [Serializable]
        private class JsonLine
        {
            [SerializeField][TextArea] private string prompt;
            [SerializeField][TextArea] private string completion;

            public string Prompt => prompt;
            public string Completion => completion;
        }
    }
}