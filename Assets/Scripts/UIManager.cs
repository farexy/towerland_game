﻿using System;
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
	private const int Width = 150;
	private const int Height = 140;
	private const int SmallHeight = 40;
	private const int NormalHeight = 80;

	private IEnumerable<GameObjectType> _monsterTypes;
	private IEnumerable<GameObjectType> _towerTypes;
	private FieldManager _fieldManager;
	private IStatsLibrary _statsLibrary;

	private PlayerSide _side;
	private Texture2D _coinImg;
	private Texture2D _castleImg;
	private SkillTextBuilder _skillTextBuilder;

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
		_coinImg = Resources.Load<Texture2D>("coin");
		_castleImg = Resources.Load<Texture2D>("castle");
		_fieldManager = GetComponent<FieldManager>();
		_statsLibrary = LocalStorage.StatsLibrary;
		_monsterTypes = _fieldManager.AvailableObjects
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Unit && t != GameObjectType.Unit)
			.OrderBy(t => _statsLibrary.GetUnitStats(t).Cost);
		_towerTypes = _fieldManager.AvailableObjects
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Tower && t != GameObjectType.Tower)
			.OrderBy(t => _statsLibrary.GetTowerStats(t).Cost);
		_skillTextBuilder = new SkillTextBuilder();
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
				GUI.Box(new Rect(StartX + x, Y, Width, Height), _fieldManager.Resources.LoadTexture(t.ToString()));
				
				var cost = _side.IsTowers()
					? _statsLibrary.GetTowerStats(t).Cost
					: _statsLibrary.GetUnitStats(t).Cost;

				bool notEnoughMoney = _fieldManager.PlayerMoney < cost;
				GUI.backgroundColor = notEnoughMoney 
					? Color.black
					: selected == t ? Color.red : Color.clear;
				
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
				GUI.skin.box.fontSize = 22;

				GUI.Box(new Rect(StartX + x, Y + Height, Width, SmallHeight), new GUIContent(cost.ToString()));
				x += Width;
			}
			GUI.Box(new Rect(StartX + x, Y, Width, NormalHeight), new GUIContent(_fieldManager.PlayerMoney.ToString(), _coinImg));
			
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
			
			var timeDelta = _fieldManager.Field.StaticData.EndTimeUtc - ServerTime.Now;
			var lastTime = timeDelta >= TimeSpan.Zero ? timeDelta : TimeSpan.Zero;
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
		DetailsPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 3f);
		DetailsPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / 1.5f);
		
		_iconImg = GameObject.Find("icon_img").GetComponent<Image>();
		_nameText = GameObject.Find("name_text").GetComponent<Text>();
		_costText = GameObject.Find("cost_text").GetComponent<Text>();
		_speedText = GameObject.Find("speed_text").GetComponent<Text>();
		_priorityText = GameObject.Find("priority_text").GetComponent<Text>();
		_damageText = GameObject.Find("damage_text").GetComponent<Text>();
		_healthRangeText = GameObject.Find("healthRange_text").GetComponent<Text>();
		_offDefTypeText = GameObject.Find("offDef_text").GetComponent<Text>();
		_specialText = GameObject.Find("special_text").GetComponent<Text>();
		
		DetailsPanel.SetActive(false);
	}

	private void LoadDetailsIcon()
	{
		var texture = _fieldManager.Resources.LoadTexture(_fieldManager.Selected.ToString());
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
			_specialText.text = _skillTextBuilder.BuildSkillText(stats.Skill, selected);
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
			_specialText.text = _skillTextBuilder.BuildSkillText(stats.Skill, selected);
		}
	}

	private string GetName(GameObjectType type)
	{
		return GameObjectLogical.ResolveType(type) == GameObjectType.Unit || GameObjectType.Tower_FortressWatchtower == type
		? type.ToString().Split('_')[1]
		: type.ToString().Replace('_', ' ');
	}
}
