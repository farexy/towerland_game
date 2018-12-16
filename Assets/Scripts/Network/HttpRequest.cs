using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    public class HttpRequest
    {
        private const string SessionHeader = "user-session";

        private readonly UnityWebRequest _webRequest;

        public HttpRequest(string url, string session = null)
        {
            _webRequest = UnityWebRequest.Get(url);
            _webRequest.SetRequestHeader(SessionHeader, session ?? String.Empty);
        }

        public HttpRequest(string url, string postData, string session = null)
        {
            _webRequest = UnityWebRequest.Post(url, postData);
            _webRequest.SetRequestHeader("Content-Type", "application/json");
            _webRequest.SetRequestHeader(SessionHeader, session ?? String.Empty);
        }

        public HttpRequest(string url, object postObject, string session = null)
            : this(url, JsonConvert.SerializeObject(postObject), session)
        {
        }
        
        public UnityWebRequest Request => _webRequest;

        public UnityWebRequestAsyncOperation Send() => _webRequest.SendWebRequest();

        public string ResponseString => _webRequest.downloadHandler.text;
    }
}