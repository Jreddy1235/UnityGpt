using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Settings Asset", fileName = "GptSettings")]
    public class GptSettings : ScriptableObject
    {
        [SerializeField] private List<string> availableModels = new()
        {
            "text-davinci-003",
            "text-curie-001",
            "text-babbage-001",
            "text-ada-001"
        };

        [SerializeField] private string baseUrl = "https://api.openai.com/v1/";
        [SerializeField] private string apiKey = "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";

        public List<string> AvailableModels => availableModels;
        public string BaseUrl => baseUrl;
        public string ApiKey => apiKey;
    }
}