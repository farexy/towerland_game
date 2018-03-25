using Helpers;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
	private Direction _curDir;
    private Vector2 _direction;
    private float _speed;

	// Use this for initialization
	void Start ()
	{
		_curDir = Direction.Down;
		transform.rotation = Quaternion.Euler(90, 180, 0);
	    _direction = transform.position;
	}
	
	// Update is called once per frame
	private void FixedUpdate()
	{
		Move();
	}

    public void SetMovement(float speed, Vector2 direction)
    {
        _direction = direction;
        _speed = speed;
	    ChangeDirection(transform.position, direction);
    }

    private void Move()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, _direction, _speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }

	private void ChangeDirection(Vector2 oldPos, Vector2 newPos)
	{
		if (CoordinationHelper.DifferentFloats(oldPos.x, newPos.x) && oldPos.x > newPos.x)
		{
			GetComponent<Rigidbody2D>().MoveRotation(90);
			_curDir = Direction.Left;
		}
		if (CoordinationHelper.DifferentFloats(oldPos.x, newPos.x) && oldPos.x < newPos.x)
		{
			GetComponent<Rigidbody2D>().MoveRotation(270);
			_curDir = Direction.Right;
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y < newPos.y)
		{
			GetComponent<Rigidbody2D>().MoveRotation(0);
			_curDir = Direction.Up;
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y > newPos.y)
		{
			GetComponent<Rigidbody2D>().MoveRotation(180);
			_curDir = Direction.Down;
		}
	}
	
	private enum Direction
	{
		Down, Left, Up, Right
	}
}
