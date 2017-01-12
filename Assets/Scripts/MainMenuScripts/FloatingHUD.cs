using UnityEngine;
using System.Collections;

public class FloatingHUD : MonoBehaviour 
{
	//	Public Variables
	public float boundary;
	public float speed;

	//	Private Variables
	private float _Top;
	private float _Bottom;
	private Vector3 _Position;

	void Start () 
	{
		_Position = transform.position;
		_Top = transform.position.y + boundary;
		_Bottom = transform.position.y - boundary;
	}

	// Update is called once per frame
	void Update () 
	{
		_Position.y+= speed; 

		if (_Position.y > _Top || _Position.y < _Bottom)
		{
			speed *= -1;
		}

		transform.position = _Position;
	}
}
