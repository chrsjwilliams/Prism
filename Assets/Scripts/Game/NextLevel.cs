using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	NextLevel: Handles inter level connection. 											*/
/*		Funtions:																		*/
/*			OnTriggerEnter2D (Collider other)											*/
/*																						*/
/*--------------------------------------------------------------------------------------*/

//	May use this script for loading screens
//	http://blog.teamtreehouse.com/make-loading-screen-unity

public class NextLevel : MonoBehaviour 
{
	public static NextLevel loadingLevel;

	public bool isLoading;					//	Are we loading.
	private float _Alpha;					//	Alpha of LoadScreen
	private float _Boundary;				//	How far the image travels from its origin
	private float _Speed;					//	How fast the image moves
	private float _Top;						//	Refernece to top boundary
	private float _Bottom;					//	Reference to bottom boundary
	private Vector3 _Position;				//	temporarily hold image's position
	private Image _LoadingScreen;			//	Reference to Loading Screen image
	private Image _LoadingIcon;				//	Reference to the floating image on Loading Screen.
	private Text _LoadingText;				//	Reference to the "Loading..." text on Loading Screen

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start()
	{
		if (loadingLevel == null)
		{
			loadingLevel = GameObject.Find("ToNextLevel").GetComponent<NextLevel>();
		}
		if (SceneManager.GetActiveScene ().name != "ControlsMenu")
		{
		_LoadingScreen = GameObject.FindGameObjectWithTag ("LoadingScreen").GetComponent<Image> ();
		_LoadingIcon = GameObject.FindGameObjectWithTag ("LoadingScreenIcon").GetComponent<Image> ();
		_LoadingText = _LoadingScreen.GetComponentInChildren<Text> ();

		_Boundary = 5.0f;
		_Speed = 0.2f;

		_Position = _LoadingIcon.transform.position;
		_Top = _LoadingIcon.transform.position.y + _Boundary;
		_Bottom = _LoadingIcon.transform.position.y - _Boundary;

		_LoadingScreen.color = new Color (0.0f, 0.0f, 0.0f, 0.0f);
		_LoadingText.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		_LoadingIcon.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		}
		isLoading = false;
		//_LoadingScreen.gameObject.SetActive (false);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	LoadNewScene: Runs once at the begining of the game. Initalizes variables.			*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator LoadNewScene(string level)
	{
		while (_LoadingScreen.color.a < 1.0f)
		{
			_Alpha += 0.01f;

			_LoadingScreen.color = new Color (_LoadingScreen.color.r, _LoadingScreen.color.g, _LoadingScreen.color.b, _Alpha);
		}

		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (level, LoadSceneMode.Single);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update ()
	{
		if (isLoading)
		{
			_LoadingText.color = new Color(_LoadingText.color.r, _LoadingText.color.g, _LoadingText.color.b, Mathf.PingPong(Time.time, 1));
			_LoadingIcon.color = new Color (_LoadingIcon.color.r, _LoadingIcon.color.g, _LoadingIcon.color.b, Mathf.PingPong(Time.time, 1) + 0.2f);

			_Position.y+= _Speed; 

			if (_Position.y > _Top || _Position.y < _Bottom)
			{
				_Speed *= -1;
			}

			_LoadingIcon.transform.position = _Position;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnTriggerEnter2D:																	*/
	/*		param: Collider2D other - 														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			isLoading = true;
			//_LoadingScreen.gameObject.SetActive (true);
			if (tag == "ToLevel2")
			{
				StartCoroutine (LoadNewScene ("Level_2"));
			}
			else if (tag == "ToLevel3")
			{
				StartCoroutine (LoadNewScene ("Level_3"));
			}
			else if (tag == "ToLevel4")
			{
				StartCoroutine (LoadNewScene ("Level_4"));
			}
			else if (tag == "ToLevel5")
			{
				StartCoroutine (LoadNewScene ("Level_5"));
			}
			else if (tag == "ToLevel6")
			{
				StartCoroutine (LoadNewScene ("Level_6"));
			}
			else if (tag == "ToLevel7")
			{
				StartCoroutine (LoadNewScene ("Level_7"));
			}
			else if (tag == "ToLevel8")
			{
				StartCoroutine (LoadNewScene ("Level_8"));
			}
			else if (tag == "End")
			{
				StartCoroutine (LoadNewScene ("End"));
			}
		}
	}
}
