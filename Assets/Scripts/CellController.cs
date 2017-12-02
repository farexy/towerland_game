using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.State;
using UnityEngine;

public class CellController : MonoBehaviour
{
	public FieldObject Object;

	private SpriteRenderer _renderer;
	private FieldManager _manager;
	private Color _naturalColor;
	
	// Use this for initialization
	void Start ()
	{
		_manager = GetComponentInParent<FieldManager>();
		_renderer = GetComponent<SpriteRenderer>();
		_naturalColor = _renderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnMouseOver()
	{
		if (_manager.Selected != GameObjectType.Undefined && _manager.Side == PlayerSide.Towers)
		{
			_renderer.color = Object == FieldObject.Ground ? Color.cyan : Color.red;
		}
	}

	private void OnMouseExit()
	{
		if (_renderer.color != _naturalColor)
		{
			_renderer.color = _naturalColor;
		}
	}
}
