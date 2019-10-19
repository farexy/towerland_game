using System;
using System.Collections;
using Assets.Scripts.Models.State;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class BattleSelectorController : MonoBehaviour
    {
        public GameObject MainMenu;

        private bool _searchForBattle;
        
        private const int Width = 200;
        private const int Height = 100;
        
        // Use this for initialization
        void Start ()
        {
            _searchForBattle = false;
        }

        private void OnGUI()
        {
            if (_searchForBattle)
            {
                //var defaultSize = GUI.skin.box.fontSize;
                GUI.skin.box.fontSize = 24;
                GUI.Box(new Rect(Screen.width / 2 - Width / 2, Screen.height / 2 - Height / 2, Width, Height), "Search for battle");
                GUI.skin.box.fontSize = 14;
            }
        }

        public void OnPvPButtonClick()
        {
            StartCoroutine(FindBattle());
        }
	
        public void OnPvEButtonClick()
        {
            StartCoroutine(FindPvEBattle());
        }

        public void OnBackButton()
        {
            _searchForBattle = false;
            MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        
        private IEnumerator FindBattle()
        {
            _searchForBattle = true;
            var www = new HttpRequest(ConfigurationManager.SearchBattleUrl, null, LocalStorage.Session);
            yield return www.Send();
            if (ConfigurationManager.Debug)
            {
                var www1 = new HttpRequest(ConfigurationManager.SearchBattleUrl, null, LocalStorage.HelpSession);
                yield return www1.Send();
            }
            var resp = new BattleSearchCheckResponseModel();
            while (!resp.Found)
            {
                if (!_searchForBattle)
                {
                    yield break;
                }
                yield return new WaitForSeconds(2);
                var www2 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.Session);
                yield return www2.Send();
                resp = JsonConvert.DeserializeObject<BattleSearchCheckResponseModel>(www2.ResponseString);
            }
            if (ConfigurationManager.Debug)
            {
                var www3 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.HelpSession);
                yield return www3.Send();
            }

            LoadBattle(resp.BattleId, resp.Side, false);
        }

        private IEnumerator FindPvEBattle()
        {
            _searchForBattle = true;
            var www = new HttpRequest(ConfigurationManager.SearchSinglePlay, null, LocalStorage.Session);
            yield return www.Send();

            var resp = new BattleSearchCheckResponseModel();
            while (!resp.Found)
            {
                yield return new WaitForFixedUpdate();
                var www2 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.Session);
                yield return www2.Send();
                resp = JsonConvert.DeserializeObject<BattleSearchCheckResponseModel>(www2.ResponseString);
            }

            var www3 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, ComputerPlayer.SessionKey);
            yield return www3.Send();

            LoadBattle(resp.BattleId, resp.Side, false);
        }

        private IEnumerator FindMultiBattle()
        {
            _searchForBattle = true;
            var www = new HttpRequest(ConfigurationManager.SearchMultiBattleUrl, null, LocalStorage.Session);
            yield return www.Send();

            var www1 = new HttpRequest(ConfigurationManager.SearchMultiBattleUrl, null, ComputerPlayer.SessionKey);
            yield return www1.Send();

            var resp = new BattleSearchCheckResponseModel();
            while (!resp.Found)
            {
                yield return new WaitForFixedUpdate();
                var www2 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.Session);
                yield return www2.Send();
                resp = JsonConvert.DeserializeObject<BattleSearchCheckResponseModel>(www2.ResponseString);
            }

            var www3 = new HttpRequest(ConfigurationManager.CheckSearchBattleUrl, ComputerPlayer.SessionKey);
            yield return www3.Send();

            LoadBattle(resp.BattleId, resp.Side, true);
        }

        private void LoadBattle(Guid id, PlayerSide side, bool multiBattle)
        {
            _searchForBattle = false;
            LocalStorage.CurrentBattleId = id;
            LocalStorage.CurrentSide = side;
            ComputerPlayer.Active = multiBattle;
            SceneManager.LoadScene("battle");
        }
    }
}