using UnityEngine;
using System.Collections;

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
	private Color _TempColor;					//	Stores temporary color
	private AudioSource _AudioSource;			//	Reference to Audio Source
	private ParticleSystem _ParticleSystem;		//	Reference to Particle System
	private Player _Player;						//	Reference to player

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void Start()
	{		
		_Player = GetComponent<Player> ();
		_ParticleSystem = GetComponent<ParticleSystem> ();
		_ParticleSystem.GetComponent<Renderer>().sortingLayerName = "Particles";
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
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f * GameMaster.gm.audioLevel);
				GameMaster.gm.TogglePlatforms (-1);
			}
			else
			{
				if (!GameMaster.gm.redIsActive && _Player.playerStats.playerRed > 0) 
				{
					_ParticleSystem.Play ();
					_TempColor = new Color (0.95f, 0.38f, 0.18f, 1.0f);
					_ParticleSystem.subEmitters.birth0.startColor = _TempColor;
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f * GameMaster.gm.audioLevel);
					GameMaster.gm.TogglePlatforms (0);
					_Player.playerStats.playerRed--;

					if (_Player.playerStats.playerRed < 0) 
					{
						_Player.playerStats.playerRed = 0;
					}
					GameData.gameData.storedPlayerRedCharge = _Player.playerStats.playerRed;
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.G))
		{
			if (_Player.playerStats.playerRed == 0 && _Player.playerStats.playerGreen == 0 && _Player.playerStats.playerBlue == 0)
			{
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f * GameMaster.gm.audioLevel);
				GameMaster.gm.TogglePlatforms (-1);
			}
			else
			{
				if (!GameMaster.gm.greenIsActive && _Player.playerStats.playerGreen > 0) 
				{
					_ParticleSystem.Play ();
					_TempColor = new Color (0.43f, 0.89f, 0.45f, 1.0f);
					_ParticleSystem.subEmitters.birth0.startColor = _TempColor;
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f * GameMaster.gm.audioLevel);
					GameMaster.gm.TogglePlatforms (1);
					_Player.playerStats.playerGreen--;

					if (_Player.playerStats.playerGreen < 0) 
					{
						_Player.playerStats.playerGreen = 0;
					}
					GameData.gameData.storedPlayerGreenCharge = _Player.playerStats.playerGreen;
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{
			if (_Player.playerStats.playerRed == 0 && _Player.playerStats.playerGreen == 0 && _Player.playerStats.playerBlue == 0)
			{
				_AudioSource.PlayOneShot (noChargeAudio, 0.5f * GameMaster.gm.audioLevel);
				GameMaster.gm.TogglePlatforms (-1);
			}
			else
			{
				if (!GameMaster.gm.blueIsActive && _Player.playerStats.playerBlue > 0) 
				{
					_ParticleSystem.Play ();
					_TempColor = new Color (0.17f, 0.54f, 0.88f, 1.0f);
					_ParticleSystem.subEmitters.birth0.startColor = _TempColor;
					_AudioSource.PlayOneShot (togglePlatformAudio, 0.5f * GameMaster.gm.audioLevel);
					GameMaster.gm.TogglePlatforms (2);
					_Player.playerStats.playerBlue--;

					if (_Player.playerStats.playerBlue < 0)
					{
						_Player.playerStats.playerBlue = 0;
					}
					GameData.gameData.storedPlayerBlueCharge = _Player.playerStats.playerBlue;
				}
			}
		}

		if (PlayerController.INSTANCE.isGrounded ()) 
		{
			PlayerController.INSTANCE.setAirControl (true);
		}
	}
}
