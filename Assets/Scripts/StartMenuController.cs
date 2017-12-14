using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Hosting;
using Assets.Scripts.Network;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class StartMenuController : MonoBehaviour
{
	private bool _searchForBattle;
	private const int Width = 200;
	private const int Height = 100;
	
	// Use this for initialization
	void Start ()
	{
		_searchForBattle = false;
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
			var www1 = new WWW(string.Format(ConfigurationManager.SearchBattleUrl, LocalStorage.PlayerId));
			yield return www1;
		}
		var resp = "";
		while (resp != "true")
		{
			var www2 = new WWW(string.Format(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.PlayerId));
			resp = www2.text;
		}
		SceneManager.LoadScene("battle");
	}
}
