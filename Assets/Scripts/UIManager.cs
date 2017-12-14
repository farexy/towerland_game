using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.State;
using Assets.Scripts.Models.Stats;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject DetailsPanel;
	
	private const int StartX = 200;
	private const int Y = 5;
	private const int Width = 75; 
	private const int Height = 70;
	private const int SmallHeight = 20;
	private const int NormalHeight = 40;

	private IEnumerable<GameObjectType> _monsterTypes;
	private IEnumerable<GameObjectType> _towerTypes;
	private FieldManager _fieldManager;
	private StatsLibrary _statsLibrary;

	private PlayerSide _side;
	private GameObjectType _selected;
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
		_side = _fieldManager.Side;
		_statsLibrary = new StatsLibrary();
		_monsterTypes = Enum.GetValues(typeof(GameObjectType))
			.Cast<GameObjectType>()
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Unit && t != GameObjectType.Unit);
		_towerTypes = Enum.GetValues(typeof(GameObjectType))
			.Cast<GameObjectType>()
			.Where(t => GameObjectLogical.ResolveType(t) == GameObjectType.Tower && t != GameObjectType.Tower);
	}
	
	private void OnGUI()
	{
		//creates a GUI rect in left corner, where we may see the current number of collected coins
		var playerSideTypes = _side == PlayerSide.Monsters ? _monsterTypes : _towerTypes;
		try
		{
			var x = 0;
			foreach (var t in playerSideTypes)
			{
				GUI.Box(new Rect(StartX + x, Y, Width, Height), GetGameObjectImage(t));
				
				var cost = _side == PlayerSide.Towers
					? _statsLibrary.GetTowerStats(t).Cost
					: _statsLibrary.GetUnitStats(t).Cost;

				bool notEnoughMoney = PlayerMoney() < cost;
				GUI.backgroundColor = notEnoughMoney 
					? Color.black
					:_selected == t ? Color.red : Color.clear;
				
				if (GUI.Button(new Rect(StartX + x, Y, Width, Height), "") && !notEnoughMoney)
				{
					if (_selected == t)
					{
						_selected = GameObjectType.Undefined;
						_fieldManager.Selected = GameObjectType.Undefined;
						DetailsPanel.SetActive(false);
					}
					else
					{
						_selected = t;
						_fieldManager.Selected = t;
						DetailsPanel.SetActive(true);
					}
				}
				GUI.backgroundColor = Color.gray;

				GUI.Box(new Rect(StartX + x, Y + Height, Width, SmallHeight), new GUIContent(cost.ToString()));
				x += Width;
			}
			GUI.Box(new Rect(StartX + x, Y, Width, NormalHeight), new GUIContent(PlayerMoney().ToString(), _coinImg));
			
			GUI.backgroundColor = Color.blue;
			if(_side == PlayerSide.Monsters && _selected != GameObjectType.Undefined 
			   && GUI.Button(new Rect(StartX + x, Y + NormalHeight, Width, NormalHeight), "Buy!"))
			{
				
				_selected = GameObjectType.Undefined;
			}
			GUI.backgroundColor = Color.gray;
			
			if (_fieldManager.Field.State.Castle.Health < 20)
			{
				GUI.contentColor = _side == PlayerSide.Monsters ? Color.green : Color.red;
			}
			else
			{
				GUI.contentColor = Color.white;
			}
			GUI.Box(new Rect(Screen.width - Width - 10, Y, Width + 10, NormalHeight), new GUIContent(string.Format("{0}/{1}", _fieldManager.Field.State.Castle.Health, "100"), _castleImg));
			
			var lastTime = _fieldManager.Field.StaticData.EndTimeUtc - DateTime.UtcNow;
			if (lastTime < TimeSpan.FromMinutes(5))
			{
				GUI.contentColor = _side == PlayerSide.Monsters ? Color.green : Color.red;
			}
			else
			{
				GUI.contentColor = Color.white;
			}
			GUI.Box(new Rect(Screen.width - Width - 10, Y + NormalHeight, Width + 10, NormalHeight), 
				new GUIContent(string.Format("{0}:{1}", lastTime.Minutes, lastTime.Seconds), _castleImg));
			SetDetails();

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
	
	private void SetDetails()
	{
		if (_selected == GameObjectType.Undefined)
		{
			return;
		}
		var texture = GetGameObjectImage(_selected);
		_iconImg.overrideSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
		_nameText.text = GetName(_selected);
		if (GameObjectLogical.ResolveType(_selected) == GameObjectType.Tower)
		{
			var stats = _statsLibrary.GetTowerStats(_selected);
			_costText.text = "Cost: " + stats.Cost;
			_speedText.text = "Attack speed: " + stats.AttackSpeed;
			_priorityText.text = "Target priority: " + stats.TargetPriority;
			_offDefTypeText.text = "Attack type: " + stats.Attack;
			_damageText.text = "Damage: " + stats.Damage;
			_healthRangeText.text = "Range: " + stats.Range;
			_specialText.text = GetSpecialEffectText(stats.SpecialEffects);
		}
		else
		{
			var stats = _statsLibrary.GetUnitStats(_selected);
			_costText.text = "Cost: " + stats.Cost;
			_speedText.text = "Speed: " + stats.Speed;
			_priorityText.text = "Movement priority: " + stats.MovementPriority;
			_offDefTypeText.text = "Defence type: " + stats.Defence;
			_damageText.text = "Damage: " + stats.Damage;
			_healthRangeText.text = "Health: " + stats.Health;
			_specialText.text = stats.IsAir ? "Air monster" : string.Empty;
		}
	}

	private string GetSpecialEffectText(SpecialEffect[] specialEffects)
	{
		if (specialEffects.Any())
		{
			var effect = specialEffects.First();
			switch (effect.Effect)
			{
					case EffectId.UnitFreezed:
						return string.Format("Freezing monsters, duration: {0} ticks", effect.Duration);
					case EffectId.Unit10xDamage_10PercentProbability:
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
		return Resources.Load<Texture2D>(type.ToString());
	}

	private int PlayerMoney()
	{
		var money = _side == PlayerSide.Monsters
			? _fieldManager.Field.State.MonsterMoney
			: _fieldManager.Field.State.TowerMoney;

		return money;
	}
}
