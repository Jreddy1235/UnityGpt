using JetBrains.Annotations;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Completions Asset", fileName = "GptCompletions")]
    public class GptCompletions : GptApiController
    {
        [SerializeField] private string completionsUrl = "completions";
        [SerializeField] private GptCompletionsRequest completionRequest;
        [SerializeField] private TextAsset userPrompt;
        [SerializeField] [ResizableTextArea] private string userResponse = "What is 1 + 1 ?";

        [SerializeField] [Dropdown("AvailableModels")]
        private string model = "text-davinci-003";

        private string CompletionsUrl => Settings.BaseUrl + completionsUrl;

        public void SendRequest(string input)
        {
            userResponse = input;
            SendRequest();
        }
        
        [UsedImplicitly]
        [Button]
        private void SendRequest()
        {
            completionRequest.Model = model;
            completionRequest.Prompt = GetUserPrompt();
            var requestString = JsonConvert.SerializeObject(completionRequest);
            SendPostRequest<GptCompletionsResponse>(CompletionsUrl, requestString, DoOnCompletions);
        }

        private void DoOnCompletions(GptCompletionsResponse response)
        {
            var completionText = response?.Choices?[0]?.Text;
            if (string.IsNullOrWhiteSpace(completionText)) return;
            Debug.Log(completionText.TrimStart());
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