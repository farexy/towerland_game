using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.State;
using Assets.Scripts.Models.Stats;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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

	void Start()
	{
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
					}
					else
					{
						_selected = t;
						_fieldManager.Selected = t;
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

		}
		catch (NullReferenceException e)
		{
			Debug.Log(e.Message);
		}
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
