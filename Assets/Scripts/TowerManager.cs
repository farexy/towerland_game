﻿using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.Stats;
using Helpers;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
	private const int Width = 50;
	private const int Height = 30;

	private const float WhizzbangSpeed = 0.1f;
	private const float BurstSpeed = 0.04f;

	private FieldManager _fieldManager;
	private IStatsLibrary _statsLibrary;
	private ObjectPool _pool;

	void Start()
	{
		_fieldManager = GetComponent<FieldManager>();
		_statsLibrary = LocalStorage.StatsLibrary;
		_pool = GetComponent<ObjectPool>();
	}

	private void OnGUI()
	{
		if (_fieldManager.Field == null)
		{
			return;
		}

		try
		{
			foreach (var tower in _fieldManager.Field.State.Towers)
			{
				GameObjectScript unitObj;
				if (!_fieldManager.TryGetGameObjectById(tower.GameId, out unitObj))
				{
					continue;
				}
				var screenPos = Camera.main.WorldToScreenPoint(unitObj.transform.position);

				SetTowerEffectColor(tower);
				if (tower.Effect != null && tower.Effect.Id != EffectId.None)
				{
					var effectImg = _fieldManager.Resources.LoadTexture(tower.Effect.Id.ToString());
					GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y - Height / 2, Width, Height), new GUIContent(effectImg));
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
	}

	public void ShowTowerAttack(Vector2 to, int towerId)
	{
		var towerType = _fieldManager.GetGameObjectById(towerId).Type;
		var whizzbang = GetWhizzbangByTower(towerType);
		var isBurst = _statsLibrary.GetTowerStats(towerType).Attack == TowerStats.AttackType.Burst;
		try
		{
			var pos = _fieldManager.GetGameObjectById(towerId).transform.position;
			ShowAttack(pos, to, whizzbang, isBurst);
		}
		catch (NullReferenceException)
		{
		}
	}
	
	public void ShowShurikenAttack(Vector2 from, Vector2 to)
	{
		var whizzbang = _pool.GetFromPool(GameObjectType.Whizzbang_Shuriken);
		ShowAttack(from, to, whizzbang, false);
	}
	
	private void ShowAttack(Vector2 from, Vector2 to, GameObjectScript whizzbang, bool isExplosion)
	{
		try
		{
			var body = whizzbang.GetComponent<Rigidbody2D>();
			whizzbang.transform.position = new Vector3(from.x, from.y, -0.5f);
			var speed = isExplosion ? BurstSpeed : WhizzbangSpeed;
			StartCoroutine(WhizzbangMovement(body, to, speed, isExplosion));
		}
		catch (NullReferenceException)
		{
		}
	}

	public void ShowCollapse(int towerId)
	{
		StartCoroutine(ShowExplosion(CoordinationHelper.GetViewPoint3(_fieldManager.Field[towerId].Position, -0.3f), 1.5f));
	}

	private IEnumerator WhizzbangMovement(Rigidbody2D whizzbang, Vector3 to, float speed, bool explosion)
	{
		while (CoordinationHelper.DifferentFloats(whizzbang.position.x, to.x, 0.01f)
		       || CoordinationHelper.DifferentFloats(whizzbang.position.y, to.y, 0.01f))
		{
			Vector3 p = Vector3.MoveTowards(whizzbang.position, to, speed);
			whizzbang.MovePosition(p);
			yield return null;
		}

		if (explosion)
		{
			StartCoroutine(ShowExplosion(to));
		}

		_pool.PutToPool(whizzbang.GetComponent<GameObjectScript>());
	}

	private GameObjectScript GetWhizzbangByTower(GameObjectType towerType)
	{
		var stats = _statsLibrary.GetTowerStats(towerType);
		switch (stats.Skill)
		{
			case SkillId.FreezesUnit:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Frost);
			case SkillId.PoisonsUnit:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Poison);
			case SkillId.ExtraDamageUnit:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Spike);
			case SkillId.ShurikenAttack:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Shuriken);
		}

		switch (stats.Attack)
		{
			case TowerStats.AttackType.Usual:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Usual);
			case TowerStats.AttackType.Magic:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Magic);
			case TowerStats.AttackType.Burst:
				return _pool.GetFromPool(GameObjectType.Whizzbang_Bomb);
		}
		
		throw new ArgumentOutOfRangeException();
	}

	private IEnumerator ShowExplosion(Vector3 pos, float scale = 1)
	{
		var explosion = _pool.GetFromPool(GameObjectType.Explosion);
		var explosionTransform = explosion.transform;

		explosionTransform.position = pos;
		explosionTransform.localScale *= scale;
		var particles = explosion.GetComponent<ParticleSystem>();
		particles.Play();
		yield return new WaitWhile(() => particles.isPlaying);
		explosionTransform.localScale /= scale;
		_pool.PutToPool(explosion);
	}
	
	private void SetTowerEffectColor(Tower tower)
	{
		if (tower.Effect.Id != EffectId.None)
		{
			switch (tower.Effect.Id)
			{
			}
		}
	}
}
