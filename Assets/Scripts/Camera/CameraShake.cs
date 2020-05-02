using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	CameraShake: Shakes the camera.														*/
/*			NOTE: Attached to GameMaster												*/
/*																						*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Shake (float amount, float lenght)											*/
/*			DoShake ()																	*/
/*			StopShake ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class CameraShake : MonoBehaviour {

	//	Public Variables
	public Camera mainCamera; 					//	Refernce to Main camera
	public Player player;						//	Reference to player
	public float shakeAmount = 0;				//	How violent the shake is

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start()
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
		}

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Shake: Starts the beginning of the shake											*/
	/*		param:	float amount - how violent the shake is									*/
	/*				float length - how many seconds the shake goes on for					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void Shake(float amount, float length)
	{
		shakeAmount = amount;
		InvokeRepeating ("DoShake", 0, 0.01f);
		Invoke ("StopShake", length);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DoShake: The actual shaking happens here											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void DoShake()
	{
		if (shakeAmount > 0)
		{
			Vector3 cameraPosition = mainCamera.transform.position;
			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			cameraPosition.x += offsetX;
			cameraPosition.y += offsetY;

			mainCamera.transform.position = cameraPosition;
		}
	}
		
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	StopShake: Stops the camera shaking													*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void StopShake()
	{
		CancelInvoke ("DoShake");
		mainCamera.transform.localPosition = new Vector3(0, 0, -10);

	}		
}
