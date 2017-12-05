using System;
using System.Collections;
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

        public IEnumerator GetCheckSeacrhBattle(Guid playerId)
        {
            var url = string.Format(ConfigurationManager.CheckSearchBattleUrl, playerId);
            WWW www = new WWW(url);
            yield return www;
        }

        public IEnumerator GetActionsByTicks(Guid battleId)
        {
            var url = string.Format(ConfigurationManager.ActionsByTicksUrl, battleId);
            WWW www = new WWW(url);
            yield return www;
        }
    }
}
