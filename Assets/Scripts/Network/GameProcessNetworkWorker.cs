using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network
{
    class GameProcessNetworkWorker
    {
        public IEnumerator GetSearchBattle(Guid playerId)
        {
            var url = string.Format(ConfigurationManager.SearchBattleUrl, playerId);
            WWW www = new WWW(url);
            yield return www;
        }

        public WWW GetCheckSeacrhBattle(Guid playerId)
        {
            var url = string.Format(ConfigurationManager.CheckSearchBattleUrl, playerId);
            WWW www = new WWW(url);
            return www;
        }

        public UnityWebRequest GetActionsByTicks(Guid battleId)
        {
            var url = string.Format(ConfigurationManager.ActionsByTicksUrl, battleId);
            var www = new HttpRequest(url).Request;
            return www;
        }

        public UnityWebRequest GetCheckBattleStateChange(Guid battleId, int version)
        {
            var url = string.Format(ConfigurationManager.GameCheckStateChanged, battleId, version);
            var www = new HttpRequest(url, LocalStorage.Session);
            return www.Request;
        }

        public WWW PostCommand(StateChangeCommandRequestModel requestModel)
        {
            var postData = JsonConvert.SerializeObject(requestModel);
            Dictionary<string, string> headers = new Dictionary<string, string> {{"Content-Type", "application/json"}};

            byte[] pData = Encoding.ASCII.GetBytes(postData.ToCharArray());
		
            WWW www = new WWW(ConfigurationManager.GameProcessCommandUrl, pData, headers);
            return www;   
        }
    }
}
