﻿using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.Stats;
using Helpers;
using UnityEngine;

public class MonstersManager : MonoBehaviour
{
	private const int Up = 30;
	private const int Width = 70; 
	private const int Height = 40;
	private const float FixedUpdate = 0.02f;
	
	private IStatsLibrary _statsLibrary;
	private FieldManager _fieldManager;
	
	// Use this for initialization
	void Start ()
	{
		_fieldManager = GetComponent<FieldManager>();
		_statsLibrary = new StatsLibrary();
		
	}

	private void OnGUI()
	{
		foreach (var unit in _fieldManager.Field.State.Units)
		{
			GameObjectScript unitObj;
			if (!_fieldManager.TryGetGameObjectById(unit.GameId, out unitObj))
			{
				continue;
			}
			var statsHealth = _statsLibrary.GetUnitStats(unit.Type).Health;
			var helthIndicator = string.Format("{0}/{1}", unit.Health, statsHealth);
			GUI.contentColor = unit.Health < statsHealth * 0.4 ? Color.red : Color.green;
			GUI.backgroundColor = Color.clear;
			var screenPos = Camera.main.WorldToScreenPoint(unitObj.transform.position);
			GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y - Up, Width, Height), helthIndicator);
		}
	}

	public void MoveUnit(int gameId, Point pos, EffectId effect)
	{
		var  obj = _fieldManager.GetGameObjectById(gameId);
		var speed = _statsLibrary.GetUnitStats(obj.Type).Speed;
		var relativeSpeed =  FixedUpdate / FieldManager.TickSecond / speed;
		relativeSpeed *= effect == EffectId.UnitFreezed ? SpecialEffect.FreezedSlowCoeff : 1;
		bool end = _fieldManager.Field.StaticData.Finish == pos;
		obj.GetComponent<MonsterController>().SetMovement(end ? relativeSpeed / 2 : relativeSpeed, CoordinationHelper.GetViewPoint(pos));
	}
	
}