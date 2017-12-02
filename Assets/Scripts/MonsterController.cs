using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(90, 180, 0);
	    _direction = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}

    public void SetMovement(float speed, Vector2 direction)
    {
        _direction = direction;
        _speed = speed;
    }

    private void Move()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, _direction, _speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
        
    }

}
