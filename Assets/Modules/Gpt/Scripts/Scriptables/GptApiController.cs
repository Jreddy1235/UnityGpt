using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UniRx;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityGPT
{
    public abstract class GptApiController : ScriptableObject, IDisposable
    {
        [SerializeField] private GptSettings settings;

        protected CompositeDisposable CompositeDisposable { get; } = new();

        protected GptSettings Settings => settings;
        [UsedImplicitly] protected List<string> AvailableModels => Settings.AvailableModels;

        void IDisposable.Dispose()
        {
            CompositeDisposable?.Dispose();
        }

        protected void PrintResponse(string response)
        {
            Debug.Log(response);
        }
        
        protected void SendGetRequest<T>(string url, Action<T> callback) where T : class
        {
            StartCoroutine(SendGetRequestCoroutine(url, callback));
        }

        protected void SendDeleteRequest<T>(string url, Action<T> callback) where T : class
        {
            StartCoroutine(SendDeleteRequestCoroutine(url, callback));
        }

        protected void SendPostRequest<T>(string url, string json, Action<T> callback) where T : class
        {
            StartCoroutine(SendPostRequestCoroutine(url, json, callback));
        }

        private void Reset()
        {
            settings = Resources.Load<GptSettings>("GptSettings");
        }

        private IEnumerator SendGetRequestCoroutine<T>(string url, Action<T> callback) where T : class
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return SendWebRequest(webRequest);

            DoOnResponseReceived(webRequest, callback);
        }

        private IEnumerator SendDeleteRequestCoroutine<T>(string url, Action<T> callback) where T : class
        {
            var webRequest = UnityWebRequest.Delete(url);
            yield return SendWebRequest(webRequest);

            DoOnResponseReceived(webRequest, callback);
        }

        private IEnumerator SendPostRequestCoroutine<T>(string url, string json, Action<T> callback) where T : class
        {
            var webRequest = UnityWebRequest.Put(url, json);
            webRequest.method = "POST";
            yield return SendWebRequest(webRequest);

            DoOnResponseReceived(webRequest, callback);
        }

        private UnityWebRequestAsyncOperation SendWebRequest(UnityWebRequest webRequest)
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {Settings.ApiKey}");
            return webRequest.SendWebRequest();
        }

        private void DoOnResponseReceived<T>(UnityWebRequest webRequest, Action<T> callback) where T : class
        {
            var responseString = webRequest.downloadHandler?.text;
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                if (string.IsNullOrEmpty(responseString))
                    responseString = webRequest.error;
                Debug.LogError(responseString);
                return;
            }

            if (string.IsNullOrWhiteSpace(responseString))
            {
                Debug.LogError("Response string is null or empty");
                return;
            }

            var type = typeof(T);
            var isPrimitiveType = type.IsPrimitive || type.IsValueType || type == typeof(string);
            if (isPrimitiveType)
                callback?.Invoke(responseString as T);
            else
                callback?.Invoke(JsonConvert.DeserializeObject<T>(responseString));
        }
        
        private void StartCoroutine(IEnumerator coroutine)
        {
#if UNITY_EDITOR
            EditorCoroutineUtility.StartCoroutineOwnerless(coroutine);
#endif
        }
    }
}