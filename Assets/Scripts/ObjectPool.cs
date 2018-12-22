using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Network;
using Helpers;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	private static readonly Vector3 PoolPosition = new Vector3(8.5f, 47f, 11f);
	
	private Dictionary<string, GameObjectScript> _gameObjectsPool;
	 
	private void InitializeObjects()
	{
		_gameObjectsPool = new Dictionary<string, GameObjectScript>();
		foreach (var x in GetSupportedObjects())
		{
			var cnt = GameObjectLogical.ResolveType(x) == GameObjectType.Whizzbang ? 10 : 4;
			for (int i = 1; i <= cnt; i++)
			{
				var gameObjName = string.Format("{0}_{1}", x.ToString(), i);
				var gameObj = GameObject.Find(gameObjName);
				if (gameObj == null)
				{
					continue;
				}
				_gameObjectsPool.Add(gameObjName, gameObj.GetComponent<GameObjectScript>());
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
		var namePrefix = type.ToString();
		var cnt = GameObjectLogical.ResolveType(type) == GameObjectType.Whizzbang ? 10 : 4;

		for (int i = 1; i <= cnt; i++)
		{
			var name = string.Format("{0}_{1}", namePrefix, i);
			var obj = _gameObjectsPool[name];
			if (obj != null && !obj.IsUsed)
			{
				obj.IsUsed = true;
				return obj;
			}

			if (obj == null)
			{
				throw new ArgumentNullException("GameObjectScript is missing");
			}
		}

		return CreateCopy(namePrefix);
	}

	public void PutToPool(GameObjectScript obj)
	{
		if (obj.name.Contains("_COPIED"))
		{
			Destroy(obj);
		}
		obj.IsUsed = false;
		obj.GameId = 0;
		obj.transform.position = PoolPosition;
	}

	private GameObjectScript CreateCopy(string namePrefix)
	{
		var obj = GameObject.Find(namePrefix + "_1");
		var newObj = Instantiate(obj);
		newObj.name += "_COPIED" + GameMath.Rand.Next();
		return newObj.GetComponent<GameObjectScript>();
	}

	private IEnumerable<GameObjectType> GetSupportedObjects()
	{
		return Enum.GetValues(typeof(GameObjectType)).Cast<GameObjectType>();
	}
}
