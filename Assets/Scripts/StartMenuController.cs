using System;
using System.Collections;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
	public GameObject MainMenu;
	public GameObject RatingTable;

	public Text LevelText;
	public Text ExperienceText;
	public GameObject Progress;
	
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
		RatingTable.SetActive(true);
		MainMenu.SetActive(false);
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

	private void ShowExp(UserExperience exp)
	{
		LevelText.text = "Level: " + exp.Level;
		ExperienceText.text = string.Format("{0}/{1}", exp.RelativeExperience, exp.TotalLevelExperience);
		var newWidth = ((float) exp.RelativeExperience / exp.TotalLevelExperience) *
		               Progress.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
		Progress.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, Progress.transform.GetComponent<RectTransform>().sizeDelta.y);
		Progress.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newWidth / 2,0);
	}

	public void LoadExp()
	{
		StartCoroutine(GetExperience(LocalStorage.PlayerId));
	}
	
	private IEnumerator GetExperience(Guid playerId)
	{
		WWW www = new WWW(string.Format(ConfigurationManager.UserExpUrl, playerId));
		yield return www;
		ShowExp(JsonConvert.DeserializeObject<UserExperience>(www.text));
	}
	
	public void ToStartMenu()
	{
		MainMenu.SetActive(true);
		RatingTable.SetActive(false);
	}
}
