using System;
using Assets.Scripts.Models.Client;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Helpers;
using UnityEngine;

public class MonstersManager : MonoBehaviour
{
	private const int Width = 50;
	private const int Height = 30;
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
				var healthIndicator = $"{unit.Health}/{statsHealth}";
				GUI.backgroundColor = Color.clear;
				var screenPos = Camera.main.WorldToScreenPoint(unitObj.transform.position);
				
				SetUnitEffectColor(unit);
				if (unit.Effect != null && unit.Effect.Id != EffectId.None)
				{
					var effectImg = _fieldManager.Resources.LoadTexture(unit.Effect.Id.ToString());
					GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y - Height / 2, Width, Height), new GUIContent(effectImg));
				}
				
				SetUnitInfoColor(unit, statsHealth);
				GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y - Height, Width, Height), healthIndicator);
				

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

	public void ShowAnimation(MonsterAnimation animationType)
	{
		foreach (var unit in _fieldManager.Field.State.Units)
		{
			_fieldManager.GetGameObjectById(unit.GameId).GetComponent<MonsterController>().ShowAnimation(animationType);
		}
	}

	private void SetUnitInfoColor(Unit unit, int statsHealth)
	{
		GUI.contentColor = unit.Health < statsHealth * 0.4 ? Color.red : Color.black;
	}

	private void SetUnitEffectColor(Unit unit)
		{
			if (unit.Effect != null && unit.Effect.Id != EffectId.None)
			{
				switch (unit.Effect.Id)
				{
					case EffectId.UnitFreezed:
						GUI.contentColor = Color.cyan;
						break;
					case EffectId.UnitPoisoned:
						GUI.contentColor = Color.green;
						break;
				}
			}
	}
}
