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
	private AudioSource _PrismSong;			//	Audio Source that plays the song

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_PrismSong = GetComponentInParent<AudioSource> ();
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
		_PrismSong.volume = 1 * GameMaster.gm.audioLevel;
		
		if (GameMaster.gm.redIsActive) 
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (GameMaster.gm.greenIsActive)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (GameMaster.gm.blueIsActive)
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}

		if (!GameMaster.gm.redIsActive && !GameMaster.gm.greenIsActive && !GameMaster.gm.blueIsActive)
		{
			_RotationAngle = Quaternion.Euler (0, 0, 0);
			transform.rotation = Quaternion.Slerp (transform.rotation, _RotationAngle, Time.deltaTime * _speed);
		}
	}
}
