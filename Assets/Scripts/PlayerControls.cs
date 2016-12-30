using UnityEngine;
using System.Collections;

/*
 * 
 * 		TODO: 
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
	//	Public Variables
	public AudioClip togglePlatformAudio;		//	Audio for toggling platforms
	public AudioClip noChargeAudio;				//	Audio for no charge

	//	Private Variables
	private bool _Jump;							//	Lets us know if we are jumping
	private AudioSource _AudioSource;			//	Reference to Audio Source
	private Player _Player;						//	Reference to player
	private GameMaster _GM;						//	Reference to Game Master

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	private void Start()
	{
		_Player = GetComponent<Player> ();
		if (_GM == null)
		{
			_GM = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}

		_AudioSource = GetComponent<AudioSource> ();
	}

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
		PlayerController.INSTANCE.Move(h, _Jump);
		_Jump = false;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Update ()
	{
		if (!_Jump)
		{
			// Read the jump input in Update so button presses aren't missed
			_Jump = Input.GetKeyDown(KeyCode.Space);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (_Player.playerStats.playerRed == 0 && _Player.playerStats.playerGreen == 0 && _Player.playerStats.playerBlue == 0)
			{
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f);
				_GM.TogglePlatforms (-1);
			}
			else
			{
				if (!_GM.redIsActive && _Player.playerStats.playerRed > 0) 
				{
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f);
					_GM.TogglePlatforms (0);
					_Player.playerStats.playerRed--;
					if (_Player.playerStats.playerRed < 0) 
					{
						_Player.playerStats.playerRed = 0;
					}
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			if (_Player.playerStats.playerRed == 0 && _Player.playerStats.playerGreen == 0 && _Player.playerStats.playerBlue == 0)
			{
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f);
				_GM.TogglePlatforms (-1);
			}
			else
			{
				if (!_GM.greenIsActive && _Player.playerStats.playerGreen > 0) 
				{
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f);
					_GM.TogglePlatforms (1);
					_Player.playerStats.playerGreen--;
					if (_Player.playerStats.playerGreen < 0) 
					{
						_Player.playerStats.playerGreen = 0;
					}
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			if (_Player.playerStats.playerRed == 0 && _Player.playerStats.playerGreen == 0 && _Player.playerStats.playerBlue == 0)
			{
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f);
				_GM.TogglePlatforms (-1);
			}
			else
			{
				if (!_GM.blueIsActive && _Player.playerStats.playerBlue > 0) 
				{
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f);
					_GM.TogglePlatforms (2);
					_Player.playerStats.playerBlue--;
					if (_Player.playerStats.playerBlue < 0)
					{
						_Player.playerStats.playerBlue = 0;
					}
				}
			}
		}

		if (PlayerController.INSTANCE.isGrounded ()) 
		{
			PlayerController.INSTANCE.setAirControl (true);
		}
	}
}
