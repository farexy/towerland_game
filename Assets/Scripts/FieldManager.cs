﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.Resolvers;
using Assets.Scripts.Models.State;
using Helpers;
using UnityEngine;
using IActionResolver = Assets.Scripts.Models.Resolvers.IActionResolver;

public class FieldManager : MonoBehaviour
{
	public const float TickSecond = 0.5f;
	
	public GameObject Ground;
	public GameObject Road;
	public GameObject Entrance;
	public GameObject Castle;
	
	private ObjectPool _pool;
	
	public GameObjectType Selected { get; set; }
	
	public PlayerSide Side { get; set; }
	public Field Field { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }

	private Dictionary<int, GameObjectScript> _gameObjects;

	private IActionResolver _stateResolver;
	private IActionResolver _viewResolver;
	
	// Use this for initialization
	void Start ()
	{
		Side = PlayerSide.Monsters;
		IFieldFactory fact = new FieldFactoryStub();
		Field = fact.ClassicField;
		_pool = GetComponent<ObjectPool>();
		_gameObjects = new Dictionary<int, GameObjectScript>();
		_stateResolver = new FieldStateActionResolver(Field);
		_viewResolver = new ViewActionResolver(this);
		
		Width = Field.StaticData.Width;
		Height = Field.StaticData.Height;
		CoordinationHelper.Init(Width, Height);
		InstantiateField();

		Field.AddGameObject(new Unit
		{
			Type = GameObjectType.Unit_Impling,
			Position = new Point(5,5)
		});
		Field.AddGameObject(new Unit
		{
			Type = GameObjectType.Unit_Skeleton,
			Position = new Point(1,1)
		});
		Field.AddGameObject(new Tower
		{
			Type = GameObjectType.Tower_Usual,
			Position = new Point(4, 4)
		});
		RenderFieldState();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void InstantiateField()
	{
		GameObject tmp;
		//look through all the cells in array, wich reprsents the maze, and draw it
		//the coords of the graphical cells match the index of array elements
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
			{

				//draw wall or floor
				try
				{
					var point = new Point(i, j);
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Entrance)
					{
						tmp = Instantiate(Entrance, CoordinationHelper.GetViewPoint(point), Quaternion.identity) as GameObject;
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Road)
					{
						tmp = Instantiate(Road, CoordinationHelper.GetViewPoint(point), Quaternion.identity) as GameObject;
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Ground)
					{
						tmp = Instantiate(Ground, CoordinationHelper.GetViewPoint(point), Quaternion.identity) as GameObject;
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Castle)
					{
						tmp = Instantiate(Castle, CoordinationHelper.GetViewPoint(point), Quaternion.identity) as GameObject;
						tmp.transform.parent = transform;
					}
				}
				catch (IndexOutOfRangeException e)
				{
					Debug.Log(e.Message);
				}

			}
		}
	}

	private void RenderFieldState()
	{
		foreach (var tower in Field.State.Towers)
		{
			if(_gameObjects.ContainsKey(tower.GameId))
				continue;
			
			var obj = _pool.GetFromPool(tower.Type);
			obj.GameId = tower.GameId;
			_gameObjects.Add(tower.GameId, obj);
			obj.transform.position = CoordinationHelper.GetViewPoint(tower.Position);
		}

		foreach (var unit in Field.State.Units)
		{
			if (_gameObjects.ContainsKey(unit.GameId))
			{
				var obj = _gameObjects[unit.GameId];
				obj.transform.position = CoordinationHelper.GetViewPoint(unit.Position);
			}
			
			var obj1 = _pool.GetFromPool(unit.Type);
			obj1.GameId = unit.GameId;
			_gameObjects.Add(unit.GameId, obj1);
			obj1.transform.position = CoordinationHelper.GetViewPoint(unit.Position);	
		}
	}

	public GameObjectScript GetGameObjectById(int id)
	{
		return _gameObjects[id];
	}

	public void RemoveGameObject(int id)
	{
		var obj = _gameObjects[id];
		_gameObjects.Remove(id);
		_pool.PutToPool(obj);
	}
	
	private IEnumerator ResolveActions(IEnumerable<IEnumerable<GameAction>> actionList)
	{
		foreach (var actions in actionList)
		{
			foreach (var action in actions)
			{
				_viewResolver.Resolve(action);
				_stateResolver.Resolve(action);
			}
			yield return new WaitForSeconds(1);
		}
	}

}
