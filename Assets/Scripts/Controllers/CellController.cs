﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.State;
using UnityEngine;

public class CellController : MonoBehaviour
{
	public FieldObject Object;
	
	public Point Point { get; set; }

	private MeshRenderer _renderer;
	private FieldManager _manager;
	private Color _naturalColor;
	
	// Use this for initialization
	void Start ()
	{
		_manager = GetComponentInParent<FieldManager>();
		_renderer = GetComponent<MeshRenderer>();
		_naturalColor = _renderer.material.color;
	}

	private void OnMouseDown()
	{
		//Debug.Log("X:" + Point + " Y:" + Point);
		if (GameObjectLogical.ResolveType(_manager.Selected) == GameObjectType.Tower)
		{
			_manager.Command(Point);
			_manager.Selected = GameObjectType.Undefined;
		}
	}
	
	private void OnMouseOver()
	{
		if (_manager.Selected != GameObjectType.Undefined && _manager.Side == PlayerSide.Towers)
		{
			_renderer.material.color = Object == FieldObject.Ground || _manager.Field.State.Towers.All(t => t.Position != Point)
				? Color.gray : Color.red;
		}
	}

	private void OnMouseExit()
	{
		if (_renderer.material.color != _naturalColor)
		{
			_renderer.material.color = _naturalColor;
		}
	}
}