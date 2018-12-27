using System;
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

	private const float WhizzbangSpeed = 0.25f;
	private const float BurstSpeed = 0.1f;

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

	public void ShowAttack(Vector2 to, int towerId)
	{
		var towerType = _fieldManager.GetGameObjectById(towerId).Type;
		var whizzbang = GetWhizzbang(towerType);
		var isBurst = _statsLibrary.GetTowerStats(towerType).Attack == TowerStats.AttackType.Burst;
		try
		{
			var body = whizzbang.GetComponent<Rigidbody2D>();
			var pos = _fieldManager.GetGameObjectById(towerId).GetComponent<Rigidbody2D>().position;
			whizzbang.transform.position = new Vector3(pos.x, pos.y, -0.5f);
			var speed = isBurst ? BurstSpeed : WhizzbangSpeed;
			StartCoroutine(WhizzbangMovement(body, to, speed, isBurst));
		}
		catch (NullReferenceException)
		{
		}
	}

	public void ShowCollapse(int towerId)
	{
		StartCoroutine(ShowExplosion(CoordinationHelper.GetViewPoint3(_fieldManager.Field[towerId].Position), 1.5f));
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

	private GameObjectScript GetWhizzbang(GameObjectType towerType)
	{
		var stats = _statsLibrary.GetTowerStats(towerType);
		var attackType = stats.Attack;
		return stats.Skill != SkillId.None && stats.Skill == SkillId.FreezesUnit
			? _pool.GetFromPool(GameObjectType.Whizzbang_Frost)
			: stats.Skill != SkillId.None && stats.Skill == SkillId.PoisonsUnit
				? _pool.GetFromPool(GameObjectType.Whizzbang_Poison)
				: attackType == TowerStats.AttackType.Magic
					? _pool.GetFromPool(GameObjectType.Whizzbang_Magic)
					: _pool.GetFromPool(GameObjectType.Whizzbang_Usual);
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
