using JetBrains.Annotations;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Chat Completions Asset", fileName = "GptChatCompletions")]
    public class GptChatCompletions : GptApiController
    {
        [SerializeField] private string chatCompletionsUrl = "chat/completions";
        [SerializeField] private GptChatCompletionsRequest completionRequest;
        [SerializeField] private TextAsset userPrompt;
        [SerializeField] private string role = "user";
        [SerializeField] [ResizableTextArea] private string userResponse = "What is 1 + 1 ?";

        [SerializeField] [Dropdown("AvailableModels")]
        private string model = "text-davinci-003";

        private string ChatCompletionsUrl => Settings.BaseUrl + chatCompletionsUrl;

        [UsedImplicitly]
        [Button]
        private void SendRequest()
        {
            completionRequest.Model = model;
            completionRequest.Messages = new[]
            {
                new ChatMessage()
                {
                    Role = role,
                    Content = GetUserPrompt()
                }
            };
            var requestString = JsonConvert.SerializeObject(completionRequest);
            SendPostRequest<GptChatCompletionsResponse>(ChatCompletionsUrl, requestString, DoOnCompletions);
        }

        private void DoOnCompletions(GptChatCompletionsResponse response)
        {
            var completionText = response?.Choices?[0]?.Message?.Content;
            Debug.Log(completionText);
        }


        private string GetUserPrompt()
        {
            var chatPrompt = userPrompt != null ? userPrompt.text : "";
            if (!string.IsNullOrEmpty(userResponse))
            {
                chatPrompt = !string.IsNullOrEmpty(chatPrompt) ? string.Format(chatPrompt, userResponse) : userResponse;
            }

            return chatPrompt;
        }
    }
}