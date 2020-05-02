using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	Cursor: The indicator for the title screen											*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class Cursor : MonoBehaviour 
{
	private float _LowerPos;		//	Lower position for the marker
	private float _HigherPos;		//	Higher position for the marker
	private Vector3 _Position;		//	Temporarily hold the cursors position
	private Image _Cursor;			//	Image of the cursor. Used to get image's height

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_Position = transform.position;
		_Cursor = GetComponent<Image> ();
		_LowerPos = _Position.y - (_Cursor.sprite.rect.height * 2);
		_HigherPos = _Position.y;
	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			_Position.y = _LowerPos;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			_Position.y = _HigherPos;
		}

		if (_Position.y == _HigherPos && Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene ("Level_1", LoadSceneMode.Single);
		}

		if (_Position.y == _LowerPos && 
			(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
		{
			SceneManager.LoadScene ("ControlsMenu", LoadSceneMode.Single);
		}
			
		transform.position = _Position;
	}
}
