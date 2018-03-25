using System.Collections;
using Assets.Scripts.Models.Stats;
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
	private bool _initializing;
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
		if (_initializing)
		{
			GUI.skin.box.fontSize = 24;
			GUI.Box(new Rect(Screen.width / 2 - Width / 2, Screen.height / 2 - Height / 2, Width, Height), "Loading");
			GUI.skin.box.fontSize = 14;
		}
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
		var www = new WwwWrapper(ConfigurationManager.SearchBattleUrl, LocalStorage.Session);
		yield return www.WWW;
		if (ConfigurationManager.Debug)
		{
			var www1 = new WwwWrapper(ConfigurationManager.SearchBattleUrl, LocalStorage.HelpSession);
			yield return www1.WWW;
		}
		var resp = new BattleSearchCheckResponseModel();
		while (!resp.Found)
		{
			var www2 = new WwwWrapper(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.Session);
			yield return www2.WWW;
			resp = JsonConvert.DeserializeObject<BattleSearchCheckResponseModel>(www2.WWW.text);
		}
		if (ConfigurationManager.Debug)
		{
			var www3 = new WwwWrapper(ConfigurationManager.CheckSearchBattleUrl, LocalStorage.HelpSession);
			yield return www3.WWW;
		}
		LocalStorage.CurrentBattleId = resp.BattleId;
		LocalStorage.CurrentSide = resp.Side;
		SceneManager.LoadScene("battle");
	}

	private void ShowExp(UserExperience exp)
	{
		LevelText.text = "Level: " + exp.Level;
		ExperienceText.text = string.Format("{0}/{1}", exp.RelativeExperience, exp.TotalLevelExperience);
		Progress.GetComponent<ProgressBarController>().SetProgressRate((float)exp.RelativeExperience / exp.TotalLevelExperience);
	}

	public void LoadExp()
	{
		StartCoroutine(GetExperience(LocalStorage.Session));
	}

	public void Initialize()
	{
		_initializing = true;
		StartCoroutine(GetExperience(LocalStorage.Session));
		StartCoroutine(GetStaticData(LocalStorage.Session));
	}
	
	private IEnumerator GetExperience(string session)
	{
		var www = new WwwWrapper(ConfigurationManager.UserExpUrl, session);
		yield return www.WWW;
		ShowExp(JsonConvert.DeserializeObject<UserExperience>(www.WWW.text));
	}

	private IEnumerator GetStaticData(string session)
	{
		var www = new WwwWrapper(ConfigurationManager.StatsDataUrl, session);
		yield return www.WWW;
		if (ConfigurationManager.Debug)
		{
			var stats = new StatsFactory();
			LocalStorage.StatsLibrary = new StatsLibrary(stats.Units, stats.Towers, stats.DefenceCoeffs);
			_initializing = false;
			yield break;
		}
		var resp = JsonConvert.DeserializeObject<StatsResponseModel>(www.WWW.text);
		LocalStorage.StatsLibrary = new StatsLibrary(resp.UnitStats, resp.TowerStats, resp.DefenceCoeffs);
		_initializing = false;
	}
	
	public void ToStartMenu()
	{
		MainMenu.SetActive(true);
		RatingTable.SetActive(false);
	}
}
