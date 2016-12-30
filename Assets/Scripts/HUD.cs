using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	HUD: Handles all HUD display, movement, and effects									*/
/*																						*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			RotateHUD (float angle)														*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class HUD : MonoBehaviour
{
	//	Private VAriables
	private float _speed;					//	Speed of the rotation
	private Quaternion _RotationAngle;		//	Stores the approiate rotation
	private GameMaster _GM;					//	Reference to GameMaster object

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_GM = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		_speed = 3f;
		_RotationAngle = Quaternion.Euler (0, 0, 0);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	RotateHUD: Initalizes the _RotationAngle variable									*/
	/*			param:																		*/
	/*				float angle - How muhc to rotate the HUDimage by						*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void RotateHUD(float angle)
	{
		_RotationAngle = Quaternion.Euler (0, 0, angle);

	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (_GM.redIsActive) 
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (_GM.greenIsActive)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (_GM.blueIsActive)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (!_GM.redIsActive && !_GM.greenIsActive && !_GM.blueIsActive)
		{
			_RotationAngle = Quaternion.Euler (0, 0, 0);
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}
	}
}
