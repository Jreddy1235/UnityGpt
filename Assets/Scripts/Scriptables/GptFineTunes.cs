using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Fine-Tunes Asset", fileName = "GptFineTunes")]
    public class GptFineTunes : GptApiController
    {
        [SerializeField] private List<string> trainingModels = new()
        {
            "davinci",
            "curie",
            "babbage",
            "ada"
        };

        [SerializeField] private string fineTunesUrl = "fine-tunes";
        [SerializeField] private string modelsUrl = "models";
        [SerializeField] private string fileId;
        [SerializeField] private string fineTuneId;
        [SerializeField] private string model;

        [SerializeField] [Dropdown("trainingModels")]
        private string trainingModel = "curie";

        private string FineTunesUrl => Settings.BaseUrl + fineTunesUrl;
        private string ModelsUrl => Settings.BaseUrl + modelsUrl;

        [UsedImplicitly]
        [Button]
        private void ListFineTunes()
        {
            SendGetRequest<string>(FineTunesUrl, PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private void CreateFineTune()
        {
            var requestString = JsonConvert.SerializeObject(new GptFineTunesRequest
            {
                TrainingFileId = fileId,
                Model = trainingModel,
            });
            SendPostRequest<string>(FineTunesUrl, requestString, PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private void RetrieveFineTuneInfo()
        {
            SendGetRequest<string>($"{FineTunesUrl}/{fineTuneId}", PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private void CancelFineTune()
        {
            var requestString = JsonConvert.SerializeObject(new GptFineTunesCancelRequest
            {
                FineTuneId = fineTuneId
            });
            SendPostRequest<string>($"{FineTunesUrl}/{fineTuneId}/cancel", requestString, PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private void DeleteFineTuneModel()
        {
            SendDeleteRequest<string>($"{ModelsUrl}/{model}", PrintResponse);
        }
    }
}