using System;
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
        
        public UnityWebRequest Request {get { return _webRequest; }}
    }
}