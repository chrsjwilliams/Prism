using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	FloatingHUD: Makes image float above I on title screen								*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class FloatingHUD : MonoBehaviour 
{
	//	Public Variables
	public float boundary;			//	How far the image travels from its origin
	public float speed;				//	How fast the image moves

	//	Private Variables
	private float _Top;				//	Refernece to top boundary
	private float _Bottom;			//	Reference to bottom boundary
	private Vector3 _Position;		//	temporarily hold image's position

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_Position = transform.position;
		_Top = transform.position.y + boundary;
		_Bottom = transform.position.y - boundary;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
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
