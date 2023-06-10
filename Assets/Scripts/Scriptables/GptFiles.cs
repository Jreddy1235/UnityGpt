using System.Net.Http;
using System.Net.Http.Headers;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace UnityGPT
{
    [CreateAssetMenu(menuName = "Gpt Assets/GPT Files Asset", fileName = "GptFiles")]
    public class GptFiles : GptApiController
    {
        [SerializeField] private string filesUrl = "files";
        [SerializeField] private string purpose = "fine-tune";
        [SerializeField] private TextAsset file;
        [SerializeField] private string fileId;

        private string FilesUrl => Settings.BaseUrl + filesUrl;

        [UsedImplicitly]
        [Button]
        private void ListAllFiles()
        {
            SendGetRequest<string>(FilesUrl, PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private async void UploadFile()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Settings.ApiKey);
            var content = new MultipartFormDataContent
            {
                { new StringContent(purpose), "purpose" },
                { new ByteArrayContent(file.bytes), "file", file.name }
            };
            
            var response = await httpClient.PostAsync(FilesUrl, content);
            var responseString = response.Content.ReadAsStringAsync().Result;
            PrintResponse(responseString);
        }
        
        [UsedImplicitly]
        [Button]
        private void RetrieveFileContent()
        {
            SendGetRequest<string>($"{FilesUrl}/{fileId}/content", PrintResponse);
        }

        [UsedImplicitly]
        [Button]
        private void DeleteFile()
        {
            SendDeleteRequest<string>($"{FilesUrl}/{fileId}", PrintResponse);
        }
    }
}