using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	ReturnTOMainMenu: Return player to Main Menu aat end game							*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class ReturnTOMainMenu : MonoBehaviour {

	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if(Input.anyKeyDown)
		{
			Destroy (GameObject.Find ("GameData"));
			SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
		}
	}
}
