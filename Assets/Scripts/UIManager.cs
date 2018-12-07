using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.State;
using Helpers;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject DetailsPanel;
	
	private const int StartX = 200;
	private const int Y = 5;
//	private const int Width = 125; 
//	private const int Height = 120;
//	private const int SmallHeight = 30;
//	private const int NormalHeight = 80;
	private const int Width = 75;
	private const int Height = 70;
	private const int SmallHeight = 20;
	private const int NormalHeight = 40;

	private Dictionary<GameObjectType, Texture2D> _texturesCache;
	private IEnumerable<GameObjectType> _monsterTypes;
	private IEnumerable<GameObjectType> _towerTypes;
	private FieldManager _fieldManager;
	private IStatsLibrary _statsLibrary;

	private PlayerSide _side;
	private Texture2D _coinImg;
	private Texture2D _castleImg;

	private Image _iconImg;
	private Text _speedText;
	private Text _nameText;
	private Text _costText;
	private Text _priorityText;
	private Text _offDefTypeText;
	private Text _damageText;
	private Text _healthRangeText;
	private Text _specialText;

	void Start()
	{
		InitDetailsPanel();
		DetailsPanel.SetActive(false);
		_coinImg = Resources.Load<Texture2D>("coin");
		_castleImg = Resources.Load<Texture2D>("castle");
		_fieldManager = GetComponent<FieldManager>();
		_statsLibrary = LocalStorage.StatsLibrary;
		_texturesCache = new Dictionary<GameObjectType, Texture2D>();
		_monsterTypes = _fieldManager.AvailableObjects
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Unit && t != GameObjectType.Unit);
		_towerTypes = _fieldManager.AvailableObjects
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Tower && t != GameObjectType.Tower);
	}
	
	private void OnGUI()
	{
		if (_fieldManager.Field == null)
		{
			return;
		}
		var selected = _fieldManager.Selected;
		_side = _fieldManager.Side; // todo remove
		DetailsPanel.SetActive(selected != GameObjectType.Undefined);
		//creates a GUI rect in left corner, where we may see the current number of collected coins
		var playerSideTypes = _side.IsMonsters() ? _monsterTypes : _towerTypes;
		try
		{
			var x = 0;
			foreach (var t in playerSideTypes)
			{
				GUI.Box(new Rect(StartX + x, Y, Width, Height), GetGameObjectImage(t));
				
				var cost = _side.IsTowers()
					? _statsLibrary.GetTowerStats(t).Cost
					: _statsLibrary.GetUnitStats(t).Cost;

				bool notEnoughMoney = PlayerMoney() < cost;
				GUI.backgroundColor = notEnoughMoney 
					? Color.black
					:selected == t ? Color.red : Color.clear;
				
				if (GUI.Button(new Rect(StartX + x, Y, Width, Height), "") && !notEnoughMoney)
				{
					if (selected == t)
					{
						
						selected = GameObjectType.Undefined;
						_fieldManager.Selected = GameObjectType.Undefined;
					}
					else
					{
						_fieldManager.Selected = t;
						LoadDetailsIcon();
					}
				}
				GUI.backgroundColor = Color.gray;

				GUI.Box(new Rect(StartX + x, Y + Height, Width, SmallHeight), new GUIContent(cost.ToString()));
				x += Width;
			}
			GUI.Box(new Rect(StartX + x, Y, Width, NormalHeight), new GUIContent(PlayerMoney().ToString(), _coinImg));
			
			GUI.backgroundColor = Color.blue;
			if(_side.IsMonsters() && _fieldManager.Selected != GameObjectType.Undefined 
			   && GUI.Button(new Rect(StartX + x, Y + NormalHeight, Width, NormalHeight), "Buy!"))
			{
				_fieldManager.Command();
				_fieldManager.Selected = GameObjectType.Undefined;
			}
			GUI.backgroundColor = Color.gray;
			
			if (_fieldManager.Field.State.Castle.Health < 20)
			{
				GUI.contentColor = _side.IsMonsters() ? Color.green : Color.red;
			}
			else
			{
				GUI.contentColor = Color.white;
			}
			GUI.Box(new Rect(Screen.width - Width - 10, Y, Width + 10, NormalHeight), new GUIContent(string.Format("{0}/{1}", _fieldManager.Field.State.Castle.Health, "100"), _castleImg));
			
			var lastTime = _fieldManager.Field.StaticData.EndTimeUtc - ServerTime.Now;
			if (lastTime < TimeSpan.FromMinutes(5))
			{
				GUI.contentColor = _side.IsMonsters() ? Color.red : Color.green;
			}
			else
			{
				GUI.contentColor = Color.white;
			}
			if (lastTime < TimeSpan.Zero)
			{
				_fieldManager.LeaveBattle();
			}
			GUI.Box(new Rect(Screen.width - Width - 10, Y + NormalHeight, Width + 10, NormalHeight), 
				new GUIContent(string.Format("{0}:{1}", lastTime.Minutes, lastTime.Seconds), _castleImg));
			SetDetails();
			if (_fieldManager.Winner != PlayerSide.Undefined)
			{
				GUI.skin.box.fontSize = 24;
				GUI.Box(new Rect(Screen.width / 2 - Width / 2, Screen.height / 2 - Height / 2, Width * 5, Height), 
					string.Format("{0} player wins!", _fieldManager.Winner));
				GUI.skin.box.fontSize = 14;
			}

		}
		catch (NullReferenceException e)
		{
			Debug.Log(e.Message);
		}
	}

	private void InitDetailsPanel()
	{
		_iconImg = GameObject.Find("icon_img").GetComponent<Image>();
		_nameText = GameObject.Find("name_text").GetComponent<Text>();
		_costText = GameObject.Find("cost_text").GetComponent<Text>();
		_speedText = GameObject.Find("speed_text").GetComponent<Text>();
		_priorityText = GameObject.Find("priority_text").GetComponent<Text>();
		_damageText = GameObject.Find("damage_text").GetComponent<Text>();
		_healthRangeText = GameObject.Find("healthRange_text").GetComponent<Text>();
		_offDefTypeText = GameObject.Find("offDef_text").GetComponent<Text>();
		_specialText = GameObject.Find("special_text").GetComponent<Text>();
	}

	private void LoadDetailsIcon()
	{
		var texture = GetGameObjectImage(_fieldManager.Selected);
		_iconImg.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
	}
	
	private void SetDetails()
	{
		var selected = _fieldManager.Selected;
		if (selected == GameObjectType.Undefined)
		{
			return;
		}
		_nameText.text = GetName(selected);
		if (GameObjectLogical.ResolveType(selected) == GameObjectType.Tower)
		{
			var stats = _statsLibrary.GetTowerStats(selected);
			_costText.text = "Cost: " + stats.Cost;
			_speedText.text = "Attack speed: " + stats.AttackSpeed;
			_priorityText.text = "Target priority: " + stats.TargetPriority;
			_offDefTypeText.text = "Attack type: " + stats.Attack;
			_damageText.text = "Damage: " + stats.Damage;
			_healthRangeText.text = "Range: " + stats.Range;
			_specialText.fontSize = 12;
			_specialText.text = GetAbilityText(stats.Ability);
		}
		else
		{
			var stats = _statsLibrary.GetUnitStats(selected);
			_costText.text = "Cost: " + stats.Cost;
			_speedText.text = "Speed: " + stats.Speed;
			_priorityText.text = "Movement priority: " + stats.MovementPriority;
			_offDefTypeText.text = "Defence type: " + stats.Defence;
			_damageText.text = "Damage: " + stats.Damage;
			_healthRangeText.text = "Health: " + stats.Health;
			_specialText.fontSize = 14;
			_specialText.text = stats.IsAir ? "Air monster" : string.Empty;
		}
	}

	private string GetAbilityText(AbilityId abilityId)
	{
		if (abilityId != AbilityId.None)
		{
			switch (abilityId)
			{
					case AbilityId.Tower_FreezesUnit:
						return string.Format("Freezing monsters");
					case AbilityId.Tower_10xDamage_10PercentProbability:
						return string.Format("10% possibility of 10x damage");
			}
		}
		return string.Empty;
	}

	private string GetName(GameObjectType type)
	{
		return GameObjectLogical.ResolveType(type) == GameObjectType.Unit || GameObjectType.Tower_FortressWatchtower == type
		? type.ToString().Split('_')[1]
		: type.ToString().Replace('_', ' ');
	}
	
	private Texture2D GetGameObjectImage(GameObjectType type)
	{
		if (_texturesCache.ContainsKey(type))
		{
			return _texturesCache[type];
		}
		
		return _texturesCache[type] = Resources.Load<Texture2D>(type.ToString());
	}

	private int PlayerMoney()
	{
		var money = _side.IsMonsters()
			? _fieldManager.Field.State.MonsterMoney
			: _fieldManager.Field.State.TowerMoney;

		return money;
	}
}
