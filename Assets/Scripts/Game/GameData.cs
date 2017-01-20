using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameData: Stores player's preferences and current game state between levels			*/
/*		Functions:																		*/
/*			Start ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameData : MonoBehaviour {


	public static GameData gameData;

	public bool preferenceStored = false;		//	Has preferences been sotred before
	public float storedAudioLevel;				//	Stored Audio Level. Set in AudioSlider.cs									Func: Update ()
	public float storedScreenShakeInetnsity;	//	Stored ScreenShakeIntensity. Set in ScreenShakeSlider.cs					Func: Update ()
	public float storedLightDirection;			//	Stores the direction the light is moveing. Set in LightPolygon.cs			Func: Update ()
	public float storedPlayerRedCharge;			//	Stores amount of Red charge. Set in PlayerControls.cs and Player.cs			Func: Update () & ChargeUp(bool red, bool green, bool blue)
	public float storedPlayerGreenCharge;		//	Stores amount of Green charge. Set in PlayerControls.cs and Player.cs		Func: Update ()	& ChargeUp(bool red, bool green, bool blue)
	public float storedPlayerBlueCharge;		//	Stores amount of Blue charge. Set in PlayerControls.cs and Player.cs		Func: Update ()	& ChargeUp(bool red, bool green, bool blue)
	public int storedActiveColor = -1;			//	Stores the active color. Set in GameMaster.cs								Func: TogglePlatforms (int color)

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Awake: Runs once at the begining of the game before Start(). Initalizes variables.	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Awake () 
	{
		if (gameData == null)
		{
			gameData = GameObject.FindGameObjectWithTag ("GameData").GetComponent<GameData> ();
		}

		DontDestroyOnLoad (this);
	}
}
