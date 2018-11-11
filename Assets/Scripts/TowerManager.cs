using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.Stats;
using Helpers;
using UnityEngine;

public class TowerManager: MonoBehaviour
{
    private const float WhizzbangSpeed = 0.25f;
	private const float BurstSpeed = 0.1f;

    private FieldManager _fieldManager;
    private IStatsLibrary _statsLibrary;
    private ObjectPool _pool;

    void Start ()
    {
        _fieldManager = GetComponent<FieldManager>();
        _statsLibrary = LocalStorage.StatsLibrary;
        _pool = GetComponent<ObjectPool>();
    }

	public void ShowAttack(Vector2 to, int towerId)
	{
		var stats = _statsLibrary.GetTowerStats(_fieldManager.GetGameObjectById(towerId).Type);
		var attackType = stats.Attack;
		var whizzbang = stats.SpecialEffects != null && stats.SpecialEffects.Id == EffectId.UnitFreezed
			? _pool.GetFromPool(GameObjectType.Whizzbang_Frost)
			: attackType == TowerStats.AttackType.Magic
				? _pool.GetFromPool(GameObjectType.Whizzbang_Magic)
				: _pool.GetFromPool(GameObjectType.Whizzbang_Usual);

		try
		{
			var body = whizzbang.GetComponent<Rigidbody2D>();
			var pos = _fieldManager.GetGameObjectById(towerId).GetComponent<Rigidbody2D>().position;
			whizzbang.transform.position = new Vector3(pos.x, pos.y, -0.5f);
			var speed = attackType == TowerStats.AttackType.Burst ? BurstSpeed : WhizzbangSpeed;
			StartCoroutine(WhizzbangMovement(body, to, speed, attackType == TowerStats.AttackType.Burst));
		}catch(NullReferenceException){}
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
			whizzbang.gameObject.transform.localScale *= 1.7f;
			yield return new WaitForSeconds(0.05f);
			whizzbang.gameObject.transform.localScale *= 1.7f;
			yield return new WaitForSeconds(0.05f);
			whizzbang.gameObject.transform.localScale *= 1.7f;
			yield return new WaitForSeconds(0.05f);
			whizzbang.gameObject.transform.localScale /= 4.913f;
		}
		_pool.PutToPool(whizzbang.GetComponent<GameObjectScript>());
	}
	
}
