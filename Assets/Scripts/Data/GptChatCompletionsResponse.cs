using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace UnityGPT
{
    public class GptChatCompletionsResponse
    {
        [JsonProperty("usage")] [CanBeNull] public ChatGptUsage Usage { get; set; }
        [JsonProperty("created")] public int Created { get; set; }
        [JsonProperty("choices")] [CanBeNull] public List<ChatGptChoice> Choices { get; set; }
        [JsonProperty("id")] [CanBeNull] public string Id { get; set; }
        [JsonProperty("object")] [CanBeNull] public string ResponseObject { get; set; }

        public class ChatGptUsage
        {
            [JsonProperty("completion_token")] public int CompletionTokens { get; set; }
            [JsonProperty("prompt_tokens")] public int PromptTokens { get; set; }
            [JsonProperty("total_tokens")] public int TotalTokens { get; set; }
        }

        public class ChatGptChoice
        {
            [JsonProperty("index")] public int Index { get; set; }

            [JsonProperty("finish_reason")]
            [CanBeNull]
            public string FinishReason { get; set; }

            [JsonProperty("message")] [CanBeNull] public ChatMessage Message { get; set; }
        }
    }
}