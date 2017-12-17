using System;
using System.Collections;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
	private bool _searchForBattle;
	private const int Width = 200;
	private const int Height = 100;
	
	// Use this for initialization
	void Start ()
	{
		_searchForBattle = false;
		LocalStorage.PlayerId = new Guid("71dc126b-f804-4cd5-93ec-dfa5087ba2da");
		LocalStorage.HelpPlayerId = new Guid("c4920571-89a1-43cc-9888-b267b415bf40");
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void OnStartButtonClick()
	{
		StartCoroutine(FindBattle());
	}
	
	public void OnRatingButtonClick()
	{
		
	}

	private IEnumerator FindBattle()
	{
		_searchForBattle = true;
		var www = new WWW(string.Format(ConfigurationManager.SearchBattleUrl, LocalStorage.PlayerId));
		yield return www;
		if (ConfigurationManager.Debug)
		{
			var www1 = new WWW(string.Format(ConfigurationManager.SearchBattleUrl, LocalStorage.HelpPlayerId));
			yield return www1;
		}
		var resp = new BattleSearchCheckResponseModel();
		while (!resp.Found)
		{
			var www2 = new WWW(string.Format(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.PlayerId));
			yield return www2;
			resp = JsonConvert.DeserializeObject<BattleSearchCheckResponseModel>(www2.text);
		}
		if (ConfigurationManager.Debug)
		{
			var www3 = new WWW(string.Format(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.HelpPlayerId));
			yield return www3;
		}
		LocalStorage.CurrentBattleId = resp.BattleId;
		LocalStorage.CurrentSide = resp.Side;
		SceneManager.LoadScene("battle");
	}
}
