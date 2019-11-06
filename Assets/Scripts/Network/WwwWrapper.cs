using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Network
{
  public class WwwWrapper
  {
    private const string SessionHeader = "user-session";	
    private readonly WWW _www;

    public WwwWrapper(string url)
    {
      _www = new WWW(url);
    }
    
    public WwwWrapper(string url, string session)	
    {	
      _www = new WWW(url, null, new Dictionary<string, string>{{SessionHeader, session ?? string.Empty}});	
    }	
 
    public WwwWrapper(string url, string postData, string session)
    {	
      Dictionary<string, string> headers = new Dictionary<string, string>	
      {	
        {"Content-Type", "application/json"},
        {SessionHeader, session ?? string.Empty}
      };	
      byte[] pData = Encoding.ASCII.GetBytes(postData.ToCharArray());

      _www = new WWW(url, pData, headers);
    }

    public WWW SendRequest() => _www;
    public string ResponseString => _www.text;
  }
}