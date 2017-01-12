using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cursor : MonoBehaviour 
{

	private float _LowerPos;
	private float _HigherPos;
	private Vector3 _Position;
	private Image _Cursor;
	// Use this for initialization
	void Start () 
	{
		_Position = transform.position;
		_Cursor = GetComponent<Image> ();
		//	_Cursor.sprite.rect.height
		_LowerPos = _Position.y - (_Cursor.sprite.rect.height * 2);
		_HigherPos = _Position.y;
	}
	
	// Update is called once per frame
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

		Debug.Log (_Position);
		transform.position = _Position;
	}
}
