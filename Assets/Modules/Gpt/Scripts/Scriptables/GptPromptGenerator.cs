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
        [SerializeField] private string promptFormat = "\"prompt\":\"completion\",\"{0}\":\"{1}\"";
        [SerializeField] private JsonLine[] lines;

        public string GetText()
        {
            return lines.Aggregate("",
                (current, line) => current + "{" + string.Format(promptFormat, line.Prompt, line.Completion) + "}\n");
        }

        [Button]
        [UsedImplicitly]
        private void PrintText()
        {
            var text = GetText();
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