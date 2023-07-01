using System;
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
        [SerializeField] private string replaceStr;
        [SerializeField] [ResizableTextArea] private string userResponse = "What is 1 + 1 ?";

        [SerializeField] [Dropdown("AvailableModels")]
        private string model = "text-davinci-003";

        private string CompletionsUrl => Settings.BaseUrl + completionsUrl;

        private Action<string> _onComplete;

        public void SendRequest(string input, Action<string> onComplete = null)
        {
            userResponse = input;
            _onComplete = onComplete;
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
            completionText = completionText.TrimStart();
            Debug.Log(completionText);
            _onComplete?.Invoke(completionText);
            _onComplete = null;
        }


        private string GetUserPrompt()
        {
            var chatPrompt = userPrompt != null ? userPrompt.text : "";
            if (string.IsNullOrEmpty(userResponse)) return chatPrompt;
            if (!string.IsNullOrEmpty(chatPrompt) && !string.IsNullOrEmpty(replaceStr))
            {
                return chatPrompt.Replace(replaceStr, userResponse);
            }

            return userResponse;
        }
    }
}