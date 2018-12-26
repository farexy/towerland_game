using System;
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
        public HttpRequest GetCheckBattleStateChange(Guid battleId, int version)
        {
            var url = string.Format(ConfigurationManager.GameCheckStateChanged, battleId, version);
            var www = new HttpRequest(url, LocalStorage.Session);
            return www;
        }

        // TODO use HttpRequest
        public WWW PostCommand(StateChangeCommandRequestModel requestModel, string session)
        {
            var www = new WwwWrapper(ConfigurationManager.GameProcessCommandUrl, requestModel.ToJson(), session);
            return www.SendRequest();
        }
    }
}
