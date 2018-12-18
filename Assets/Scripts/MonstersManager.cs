﻿using System;
using Assets.Scripts.Models.Client;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.Interfaces;
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
	
	void Start ()
	{
		_fieldManager = GetComponent<FieldManager>();
		_statsLibrary = LocalStorage.StatsLibrary;
	}

	private void OnGUI()
	{
		if (_fieldManager.Field == null)
		{
			return;
		}
		try
		{
			foreach (var unit in _fieldManager.Field.State.Units)
			{
				GameObjectScript unitObj;
				if (!_fieldManager.TryGetGameObjectById(unit.GameId, out unitObj))
				{
					continue;
				}
				var statsHealth = _statsLibrary.GetUnitStats(unit.Type).Health;
				var healthIndicator = string.Format("{0}/{1}", unit.Health, statsHealth);
				GUI.contentColor = unit.Health < statsHealth * 0.4 ? Color.red : Color.black;
				GUI.backgroundColor = Color.clear;
				var screenPos = Camera.main.WorldToScreenPoint(unitObj.transform.position);
				if (unit.Effect != null && unit.Effect.Id != EffectId.None)
				{
					var effectImg = _fieldManager.Resources.LoadTexture(unit.Effect.Id.ToString());
					GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y - Up, Width, Height), new GUIContent(healthIndicator, effectImg));
				}
				else
				{
					GUI.Box(new Rect(screenPos.x, Screen.height - screenPos.y - Up, Width, Height), healthIndicator);
				}

				var healthBar = unitObj.GetComponentInChildren<ProgressBarController>();
				if (healthBar != null)
				{
					healthBar.SetProgressRate((float) unit.Health / statsHealth);
				}
			}
		}catch(InvalidOperationException){}
	}

	public void MoveUnit(int gameId, Point pos, EffectId effect)
	{
		var  obj = _fieldManager.GetGameObjectById(gameId);
		var speed = _statsLibrary.GetUnitStats(obj.Type).Speed;
		bool end = _fieldManager.Field.StaticData.Finish == pos;
		var relativeSpeed = end ? 0 : FixedUpdate / FieldManager.TickSecond / speed;
//		relativeSpeed *= effect == EffectId.UnitFreezed ? SpecialEffect.FreezedSlowCoeff : 1;
		obj.GetComponent<MonsterController>().SetMovement(relativeSpeed, CoordinationHelper.GetViewPoint(pos));
	}

	public void ShowAnimation(int gameId, MonsterAnimation animationType)
	{
		_fieldManager.GetGameObjectById(gameId).GetComponent<MonsterController>().ShowAnimation(animationType);
	}
}
