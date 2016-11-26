using UnityEngine;
using System.Collections;

/*
 * 
 * 		TODO: Changing the active layer
 * 			  Implement light polygon
 * 
 */ 

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayerControls: Handles user input for player										*/
/*		Depends on Player.cs script														*/
/*																						*/
/*		Functions:																		*/
/*			FixedUpdate ()																*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
[RequireComponent(typeof (Player))]
public class PlayerControls : MonoBehaviour
{
	//	Private Variables
	private bool m_Jump;			//	Lets us know if we are jumping

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FixedUpdate: Called once per fixed amount of frames									*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void FixedUpdate()
	{
		// Read the inputs
		float h = Input.GetAxis ("Horizontal");
		// Pass all parameters to the Player class
		PlayerController.INSTANCE.Move(h, m_Jump);
		m_Jump = false;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Update ()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed
			m_Jump = Input.GetKeyDown(KeyCode.Space);
		}

		if (PlayerController.INSTANCE.isGrounded ()) 
		{
			PlayerController.INSTANCE.setAirControl (true);
		}
	}
}
