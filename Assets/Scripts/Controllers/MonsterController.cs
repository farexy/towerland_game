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
			SetCurDir(Direction.Left);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.x, newPos.x) && oldPos.x < newPos.x)
		{
			GetComponent<Rigidbody2D>().MoveRotation(270);
			SetCurDir(Direction.Right);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y < newPos.y)
		{
			GetComponent<Rigidbody2D>().MoveRotation(0);
			SetCurDir(Direction.Up);
		}
		if (CoordinationHelper.DifferentFloats(oldPos.y, newPos.y) && oldPos.y > newPos.y)
		{
			GetComponent<Rigidbody2D>().MoveRotation(180);
			SetCurDir(Direction.Down);
		}
	}

	private void SetCurDir(Direction dir)
	{
		if (_curDir == Direction.Down && dir == Direction.Right)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, -90);
		}
		if (_curDir == Direction.Down && dir == Direction.Left)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, 90);
		}
		if (_curDir == Direction.Up && dir == Direction.Right)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, 90);
		}
		if (_curDir == Direction.Up && dir == Direction.Left)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, -90);
		}
		if (_curDir == Direction.Right && dir == Direction.Down)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, -90);
		}
		if (_curDir == Direction.Right && dir == Direction.Up)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, 90);
		}
		if (_curDir == Direction.Left && dir == Direction.Up)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, 90);
		}
		if (_curDir == Direction.Left && dir == Direction.Down)
		{
			GetComponentInChildren<ProgressBarController>()
				.transform.parent.GetComponent<RectTransform>().Rotate(Vector3.up, -90);
		}
		_curDir = dir;
	}

	private enum Direction
	{
		Down, Left, Up, Right
	}
}
