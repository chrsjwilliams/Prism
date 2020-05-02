using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	ScreenShakeSlider: Connects Pause screen slider to game								*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class ScreenShakeSlider : MonoBehaviour 
{

	public Slider screenShakeSlider;		//	Reference to UI slider

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		screenShakeSlider = GetComponent<Slider> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		GameMaster.gm.shakeIntensity = screenShakeSlider.value;
		GameData.gameData.storedScreenShakeInetnsity = screenShakeSlider.value;
	}
}
