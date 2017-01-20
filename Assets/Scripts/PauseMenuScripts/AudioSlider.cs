using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	AudioSLider: Connects Pause screen slider to game									*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class AudioSlider : MonoBehaviour 
{

	public Slider audioSlider;		//	Reference to UI slider

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		audioSlider = GetComponent<Slider> ();
	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		GameMaster.gm.audioLevel = audioSlider.value;
		GameData.gameData.storedAudioLevel = audioSlider.value;
	}
}
