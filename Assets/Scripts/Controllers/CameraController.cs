using System;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GameField;
using Assets.Scripts.Models.GameObjects;
using Assets.Scripts.Models.Interfaces;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private FieldManager _fieldManager;

    public GameObject Camera;

	// Use this for initialization
	void Start ()
	{
		_fieldManager = GetComponent<FieldManager>();
		//var obj = _pool.GetFromPool(GameObjectType.Unit_Skeleton);
		//obj.transform.position = new Vector2(1f, 1f);

		//var obj2 = _pool.GetFromPool(GameObjectType.Unit_Skeleton);
		//obj2.transform.position = new Vector2(2f, 2f);
		// GetComponentInChildren();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //float zoom = Input.GetAxis("Zoom");
        Vector3 position = Camera.transform.position;

        //float DEADZONE = 0.1f;
        if (horizontal < 0 && position.x > _fieldManager.Width / -3) position.x -= .1f;
        if (horizontal > 0 && position.x < _fieldManager.Width / 3) position.x += .1f;
        if (vertical < 0 && position.y > _fieldManager.Height / -2) position.y -= .1f;
        if (vertical > 0 && position.y < _fieldManager.Height / 2) position.y += .1f;

        Camera.transform.position = position;
    }

   
}
