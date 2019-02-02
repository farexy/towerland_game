using System.Collections.Generic;
using Assets.Scripts.Models.Client;
using Assets.Scripts.Models.Effects;
using Helpers;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
	private static readonly Dictionary<MonsterAnimation, bool> AnimBehaviour = new Dictionary<MonsterAnimation, bool>
	{
		{MonsterAnimation.Attack, true},
		{MonsterAnimation.Die, false},
		{MonsterAnimation.Run, true},
		{MonsterAnimation.Skill, true},
		{MonsterAnimation.Spawn, true}
	};

	private Transform _healthBarTransform;
	private Direction _curDir;
    private Vector2 _direction;
    private float _speed;

	// Use this for initialization
	void Start ()
	{
		_healthBarTransform = GetComponentInChildren<ProgressBarController>().transform.parent;
		_curDir = Direction.Down;
		transform.rotation = Quaternion.Euler(90, 180, 0);
	    _direction = transform.position;
	}
	
	// Update is called once per frame
	private void Update()
	{		
		if(_healthBarTransform.rotation != Quaternion.Euler(90, 0, 0))
		{
			_healthBarTransform.rotation = Quaternion.Euler(90, 0, 0);
		}

		Move();
	}
	
	public void SetMovement(Vector2 direction)
	{
		_direction = direction;
		ChangeDirection(transform.position, direction);
	}
	
    public void SetMovement(float speed, Vector2 direction)
    {
        _direction = direction;
        _speed = speed;
	    ChangeDirection(transform.position, direction);
    }

	public void ShowAnimation(MonsterAnimation animationType)
	{
		if (!AnimBehaviour[animationType])
		{
			SetMovement(0, transform.position);
		}
		var animator = GetComponent<Animator>();
		if (animator != null)
		{
			var animName = animationType.ToString().ToLower();
			animator.SetTrigger(animName);
		}
	}

    private void Move()
    {
	    var distance = Vector3.Distance(transform.position, _direction);
	    var currSpeed = distance > 1 ? _speed * distance : _speed;
      Vector2 p = Vector2.MoveTowards(transform.position, _direction, currSpeed * Time.deltaTime);
      GetComponent<Rigidbody2D>().MovePosition(p);
    }

	private void ChangeDirection(Vector2 oldPos, Vector2 newPos)
	{
		if (CoordinationHelper.DifferentFloats(oldPos.x, newPos.x) && oldPos.x > newPos.x)
		{
			SetCurDir(Direction.Left);
			GetComponent<Rigidbody2D>().MoveRotation(90);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.x, newPos.x) && oldPos.x < newPos.x)
		{
			SetCurDir(Direction.Right);
			GetComponent<Rigidbody2D>().MoveRotation(270);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y < newPos.y)
		{
			SetCurDir(Direction.Up);
			GetComponent<Rigidbody2D>().MoveRotation(0);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y > newPos.y)
		{
			SetCurDir(Direction.Down);
			GetComponent<Rigidbody2D>().MoveRotation(180);
		}
	}

	private void SetCurDir(Direction dir)
	{
		_curDir = dir;
	}

	private enum Direction
	{
		Down, Left, Up, Right
	}
}
