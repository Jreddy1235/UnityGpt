using System;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityGPT
{
    [Serializable]
    public class GptCompletionsRequest
    {
        private const int DefaultMaxTokens = 120;

        [JsonIgnore] [SerializeField] private int maxTokens = DefaultMaxTokens;

        [JsonIgnore] [SerializeField] [Range(0, 1)]
        private float temperature;

        [JsonIgnore] [SerializeField] [Range(0, 1)]
        private float topP;

        [JsonIgnore] [SerializeField] [Range(-2, 2)]
        private float frequencyPenalty;

        [JsonIgnore] [SerializeField] [Range(-2, 2)]
        private float presencePenalty;

        [JsonProperty("frequency_penalty")]
        public float FrequencyPenalty
        {
            get => frequencyPenalty;
            set => frequencyPenalty = value;
        }

        [JsonProperty("presence_penalty")]
        public float PresencePenalty
        {
            get => presencePenalty;
            set => presencePenalty = value;
        }

        [JsonProperty("temperature")]
        public float Temperature
        {
            get => temperature;
            set => temperature = value;
        }

        [JsonProperty("top_p")]
        public float TopP
        {
            get => topP;
            set => topP = value;
        }

        [JsonProperty("max_tokens")]
        public int MaxTokens
        {
            get => maxTokens;
            set => maxTokens = value;
        }

        [JsonProperty("model")]
        public string Model
        {
            get => _model;
            set => _model = value;
        }

        [JsonProperty("prompt")]
        public string Prompt
        {
            get => _prompt;
            set => _prompt = value;
        }

        [JsonIgnore] private string _prompt;
        [JsonIgnore] private string _model;

        public bool ShouldSerializeTemperature()
        {
            return Temperature != 0;
        }

        public bool ShouldSerializeTopP()
        {
            return TopP != 0;
        }

        public bool ShouldSerializePresencePenalty()
        {
            return PresencePenalty != 0;
        }

        public bool ShouldSerializeFrequencyPenalty()
        {
            return FrequencyPenalty != 0;
        }
    }
}