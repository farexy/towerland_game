﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ai;
using Assets.Scripts.Models.Client;
using Assets.Scripts.Models.GameActions;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using Assets.Scripts.Models.Resolvers;
using Assets.Scripts.Models.State;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Models;
using Helpers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using IActionResolver = Assets.Scripts.Models.Resolvers.IActionResolver;

public class FieldManager : MonoBehaviour
{
	public const float TickSecond = 0.5f;
	private readonly Quaternion Quaternion = Quaternion.Euler(90, 90, 270);
	
	public GameObject Ground;
	public GameObject Road;
	public GameObject Entrance;
	public GameObject Castle;
	public GameObject Stone;
	public GameObject Tree;

	private Coroutine _resolver;
	private Coroutine _tickTimer;
	private int _revision;
	private Guid _battleId;
	private string _session;
	private ObjectPool _pool;
	private int _tickCount;
	private bool _isEnding;
	private IStatsLibrary _statsLibrary;
	private GameProcessNetworkWorker _gameProcessNetworkWorker;
	private AiService _aiService;
	
	private MonstersManager _monstersManager;
	private TowerManager _towerManager;
	
	public GameObjectType Selected { get; set; }
	public ResourcesCache Resources { get; set; }
	
	public PlayerSide Winner { get; set; }
	public PlayerSide Side { get; set; }
	public Field Field { get; private set; }
	public int Width { get; private set; }
	public int Height { get; private set; }
	public HashSet<GameObjectType> AvailableObjects { get; private set; }
	
	public int PlayerMoney =>  Side.IsMonsters() ? Field.State.MonsterMoney : Field.State.TowerMoney;

	private Dictionary<int, GameObjectScript> _gameObjects;

	private IActionResolver _stateResolver;
	private IActionResolver _viewResolver;
	
	// Use this for initialization
	void Start ()
	{
		AvailableObjects = new HashSet<GameObjectType>
		{
			GameObjectType.Unit_Skeleton,
			GameObjectType.Unit_BarbarianMage,
			GameObjectType.Unit_Orc,
			GameObjectType.Unit_Goblin,
			GameObjectType.Unit_Dragon,
			GameObjectType.Unit_Demon,
			GameObjectType.Unit_Necromancer,

			GameObjectType.Tower_Usual,
			GameObjectType.Tower_Frost,
			GameObjectType.Tower_Cannon,
			GameObjectType.Tower_FortressWatchtower,
			GameObjectType.Tower_Magic,
			GameObjectType.Tower_Poisoning,
			GameObjectType.Tower_Orc
		};

		_gameProcessNetworkWorker = new GameProcessNetworkWorker();
		_battleId = LocalStorage.CurrentBattleId;
		_session = LocalStorage.Session;
		Side = LocalStorage.CurrentSide;
		_statsLibrary = LocalStorage.StatsLibrary;
		_pool = GetComponent<ObjectPool>();
		_gameObjects = new Dictionary<int, GameObjectScript>();
		_monstersManager = GetComponent<MonstersManager>();
		_towerManager = GetComponent<TowerManager>();
		_aiService = new AiService(_statsLibrary, this);
		Resources = new ResourcesCache();

		StartCoroutine(Init());
		StartCoroutine(NetworkWorker());
		if (ComputerPlayer.Active)
		{
			StartCoroutine(ComputerPlayerWorker());
		}
	}
	
	private void InstantiateField()
	{
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
					GameObject tmp;
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Entrance)
					{
						tmp = Instantiate(Entrance, CoordinationHelper.GetViewPoint(point), Quaternion);
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Road)
					{
						tmp = Instantiate(Road, CoordinationHelper.GetViewPoint(point), Quaternion);
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Ground)
					{
						tmp = Instantiate(Ground, CoordinationHelper.GetViewPoint(point), Quaternion);
						tmp.GetComponent<CellController>().Point = new Point(i, j);
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Castle)
					{
						tmp = Instantiate(Castle, CoordinationHelper.GetViewPoint(point), Quaternion);
						tmp.transform.parent = transform;
					}
					if (Field.StaticData.Cells[i, j].Object == FieldObject.Stone)
					{
						tmp = Instantiate(Stone, CoordinationHelper.GetViewPoint3(point, -0.2f), Quaternion);
						tmp.transform.parent = transform;
						tmp = Instantiate(Ground, CoordinationHelper.GetViewPoint(point), Quaternion);
						tmp.GetComponent<CellController>().Point = new Point(i, j);
						tmp.GetComponent<CellController>().SetFieldObject(FieldObject.Stone);
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
	
	public void RenderFieldState()
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
		foreach (var unit in Field.State.Units.ToArray())
		{
			if (_gameObjects.ContainsKey(unit.GameId))
			{
				var obj = _gameObjects[unit.GameId];
				
				obj.GetComponent<MonsterController>().SetMovement(CoordinationHelper.GetViewPoint(unit.Position));
				continue;
			}

			var obj1 = _pool.GetFromPool(unit.Type);
			obj1.GameId = unit.GameId;
			_gameObjects.Add(unit.GameId, obj1);
			_monstersManager.ShowAnimation(unit.GameId, MonsterAnimation.Spawn);
			obj1.transform.position = _statsLibrary.GetUnitStats(obj1.Type).IsAir
				? CoordinationHelper.GetViewPoint3(unit.Position, -0.7f)
				: (Vector3) CoordinationHelper.GetViewPoint(unit.Position);
		}
		//delete unexisting
		foreach (var gId in _gameObjects.Keys.ToArray())
		{
			if (!Field.HasObject(gId))
			{
				Debug.Log("Garbage collected " + gId);
				RemoveGameObject(gId);
			}
		}
	}

	public GameObjectScript GetGameObjectById(int id)
	{
		return _gameObjects.ContainsKey(id) ? _gameObjects[id] : null;
	}

	public bool TryGetGameObjectById(int id, out GameObjectScript obj)
	{
		obj = null;
		if (!_gameObjects.ContainsKey(id))
		{
			return false;
		}
		obj = _gameObjects[id];
		return true;
	}

	public void RemoveGameObject(int id)
	{
		try
		{
			var obj = _gameObjects[id];
			_gameObjects.Remove(id);
			_pool.PutToPool(obj);
		}
		catch (KeyNotFoundException)
		{
			Debug.Log($"Object {id} not found when remove");
		}
	}

	public void RemoveGameObjectWithDelay(int id, float delaySec)
	{
		StartCoroutine(RemoveWithDelay(id, delaySec));
	}

	public void SwitchSide()
	{
		Side = Side.Invert();
		_session = _session == LocalStorage.Session ? LocalStorage.HelpSession : LocalStorage.Session;
	}
	
	public void Command(Point? position = null)
	{
		if (PlayerMoney - _statsLibrary.GetStats(Selected).Cost < 0)
		{
			return;
		}
		StartCoroutine(PostCommand(Selected, position, LocalStorage.Session));
	}
	
	private IEnumerator StartShow()
	{
		yield return new WaitForSeconds(3);
		RenderFieldState();

		//var clc = new StateCalculator(LocalStorage.StatsLibrary, Field);
		//StartCoroutine(ResolveActions(clc.CalculateActionsByTicks()));
	}

	private IEnumerator RemoveWithDelay(int id, float delaySec)
	{
		yield return new WaitForSeconds(delaySec);
		RemoveGameObject(id);
	}

	private IEnumerator PostCommand(GameObjectType type, Point? position, string session, string cheat = null)
	{
		var command = new StateChangeCommandRequestModel
		{
			BattleId = _battleId,
			CurrentTick	= _tickCount + 1,
			TowerCreationOptions = GameObjectLogical.ResolveType(type) == GameObjectType.Tower
				? new []{new TowerCreationOption{Type = type, Position = position.Value}}
				: null,
			UnitCreationOptions = GameObjectLogical.ResolveType(type) == GameObjectType.Unit
				? new []{new UnitCreationOption{Type = type}}
				: null,
			CheatCommand = cheat
		};

		yield return _gameProcessNetworkWorker.PostCommand(command, session);
	}

	private IEnumerator NetworkWorker()
	{
		while (true)
		{
			if (Field == null)
			{
				yield return null;
				continue;
			}
			if (Field.StaticData.EndTimeUtc < ServerTime.Now)
			{
				yield return TryPostEnd();
			}
			if (Field.State.Castle.Health <= 0 && _tickCount > 0)
			{
				yield return TryPostEnd();
			}
			var www = _gameProcessNetworkWorker.GetCheckBattleStateChange(_battleId, _revision);
			yield return www.Send();
			if (!string.IsNullOrEmpty(www.ResponseString))
			{
				var ticks = JsonConvert.DeserializeObject<ActionsResponseModel>(www.ResponseString);
				_revision = ticks.Revision;
				Field.SetState(ticks.State);
				if (_resolver != null)
				{
					StopCoroutine(_resolver);
				}

				if (_tickTimer != null)
				{
					StopCoroutine(_tickTimer);
				}
				RenderFieldState();
				_resolver = StartCoroutine(ResolveActions(ticks.ActionsByTicks));
				_tickTimer = StartCoroutine(TickTimer());
			}
			yield return new WaitForSeconds(0.4f);
		}
	}

	private IEnumerator TickTimer()
	{
		while (_tickCount < int.MaxValue)
		{
			yield return new WaitForSeconds(TickSecond);
			_tickCount++;
		}
	}
	
	private IEnumerator ResolveActions(IEnumerable<GameTick> actionList)
	{
		_tickCount = 0;
		foreach (var tick in actionList)
		{
			var currentTick = _tickCount;
			foreach (var action in tick.Actions)
			{
				try
				{
					_stateResolver.Resolve(action);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
				try
				{
					_viewResolver.Resolve(action);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			//_tickCount++;
			//yield return new WaitForSeconds(TickSecond);
			yield return new WaitUntil(() => currentTick < _tickCount);
		}
	}

	public void LeaveBattle()
	{
		StartCoroutine(PostEnd(Side.Invert()));
	}

	public void Cheat()
	{
		StartCoroutine(PostCommand(GameObjectType.Undefined, null, _session, "addm"));
	}

	private IEnumerator TryPostEnd()
	{
		if (!_isEnding)
		{
			_isEnding = true;
			var www = new HttpRequest(string.Format(ConfigurationManager.TryEndUrl, _battleId), _session);
			yield return www.Send();
		}
	}
	
	private IEnumerator PostEnd(PlayerSide winner)
	{
		if (!_isEnding)
		{
			_isEnding = true;
			var www = new HttpRequest(string.Format(ConfigurationManager.TryEndUrl, _battleId), _session);
			yield return www.Send();
			StartCoroutine(Leave(winner));
		}
	}
	
	private IEnumerator Leave(PlayerSide playerSide)
	{
		Winner = playerSide;
		yield return new WaitForSeconds(5);
		_isEnding = false;
		SceneManager.LoadScene("StartPages");
	}

	public void End(PlayerSide winner)
	{
		StartCoroutine(Leave(winner));
	}

	private IEnumerator Init()
	{
		var httpRequest = new HttpRequest(string.Format(ConfigurationManager.InitFieldUrl, _battleId), _session);
		yield return httpRequest.Send();
		Field = JsonConvert.DeserializeObject<Field>(httpRequest.ResponseString);
		
		_stateResolver = new FieldStateActionResolver(Field);
		_viewResolver = new ViewActionResolver(this, _monstersManager, _towerManager);
		
		Width = Field.StaticData.Width;
		Height = Field.StaticData.Height;
		CoordinationHelper.Init(Width, Height);
		InstantiateField();
	}

	private IEnumerator ComputerPlayerWorker()
	{
		while (!_isEnding)
		{
			yield return new WaitForSeconds(TickSecond * 4);
			var objectToAdd = _aiService.TryAddGameObject();
			if (objectToAdd != GameObjectType.Undefined)
			{
				yield return StartCoroutine(PostCommand(objectToAdd, null, ComputerPlayer.SessionKey));
			}
		}
	}
}
