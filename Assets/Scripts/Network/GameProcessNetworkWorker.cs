using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;

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

        public WWW GetActionsByTicks(Guid battleId)
        {
            var url = string.Format(ConfigurationManager.ActionsByTicksUrl, battleId);
            WWW www = new WWW(url);
            return www;
        }

        public WWW GetCheckBattleStateChange(Guid battleId, int version)
        {
            var url = string.Format(ConfigurationManager.GameCheckStateChanged, battleId, version);
            WwwWrapper www = new WwwWrapper(url, LocalStorage.Session);
            return www.WWW;
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
