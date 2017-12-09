using System.Collections.Generic;
using Assets.Scripts.Models.GameObjects;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private static readonly Vector3 PoolPosition = new Vector3(8.5f, 47f, 11f);
	
	private static readonly Dictionary<GameObjectType, string> TypeIdConfirmity = new Dictionary<GameObjectType, string>
	{
		{GameObjectType.Whizzbang_Usual, "UsualWhizzbang"},
		{GameObjectType.Whizzbang_Frost, "FrostWhizzbang"},
		{GameObjectType.Whizzbang_Magic, "MagicWhizzbang"},

		{GameObjectType.Unit_Impling, "Impling"},
		{GameObjectType.Unit_Skeleton, "Skeleton"},
		
		{GameObjectType.Tower_Usual, "UsualTower"},
		{GameObjectType.Tower_Cannon, "CannonTower"},
		{GameObjectType.Tower_Frost, "FrostTower"}

	};

	private Dictionary<string, GameObjectScript> _gameObjectsPool;
	
	private void InitializeObjects()
	{
		_gameObjectsPool = new Dictionary<string, GameObjectScript>();
		foreach (var x in TypeIdConfirmity)
		{
			var cnt = GameObjectLogical.ResolveType(x.Key) == GameObjectType.Whizzbang ? 10 : 4;
			for (int i = 1; i <= cnt; i++)
			{
				var name = string.Format("{0}_{1}", x.Value, i);
				_gameObjectsPool.Add(name, GameObject.Find(name).GetComponent<GameObjectScript>());			
			}
		}
	}


	// Use this for initialization
	void Start () {
		InitializeObjects();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObjectScript GetFromPool(GameObjectType type)
	{
		var namePrefix = TypeIdConfirmity[type];
		var cnt = GameObjectLogical.ResolveType(type) == GameObjectType.Whizzbang ? 2 : 4;

		for (int i = 1; i <= cnt; i++)
		{
			var name = string.Format("{0}_{1}", namePrefix, i);
			var obj = _gameObjectsPool[name];
			if (obj != null && !obj.IsUsed)
			{
				obj.IsUsed = true;
				return obj;
			}
		}

		return null;
	}

	public void PutToPool(GameObjectScript obj)
	{
		obj.IsUsed = false;
		obj.GameId = 0;
		obj.transform.position = PoolPosition;
	}
}
