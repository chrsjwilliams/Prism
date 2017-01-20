using UnityEngine;
using System.Collections;

/*
 * 
 *		TODO:	Enemy collision with player. Particle Effects
 *				Loading Screen between levels. maybe between Main menu and game? (probably not)
 * 			
 * 
 */

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameMaster: Manages states of the game												*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			RespawnPlayer ()															*/
/*			KillPlayer ()																*/
/*			HideAll ()																	*/
/*			TogglePlatforms(int i)														*/	
/*			TogglePauseScreen(bool showScreen)											*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameMaster : MonoBehaviour 
{
	//	Public Static variables
	public static GameMaster gm;				//	Reference to Game Master game object

	//	Public Variables
	public bool isPaused;						//	Toggles puase screen
	public bool redIsActive;					//	Tells us if red layer is active
	public bool greenIsActive;					//	Tells us if green layer is active
	public bool blueIsActive;					//	Tells us if blue layer is active
	public float audioLevel;					//	Controls the Audio Level of the game
	public float shakeIntensity;				//	How violent the camera shake is
	public float cameraShakeAmount = 0.2f;		//	How much to shake the camera by
	public float cameraShakeLength = 0.1f;		//	How long the camera shakes for
	public int spawnDelay = 2;					//	Spawen delay for player
	public Transform playerPrefab;				//	Reference to player prefab
	public Transform spawnPoint;				//	Reference to level's spawn point
	public AudioClip deadAudio;					//	Audio clip for death

	//	Private Static Variables
	private static GameObject _Player;			//	Reference to player Game Object
	private static GameObject _RedLayer;		//	Reference to red layer
	private static GameObject _GreenLayer;		//	Reference to green layer
	private static GameObject _BlueLayer;		//	Reference to blue layer

	//	Private Variables
	private Quaternion _RotationHide;			//	Rotate layer to hide it
	private Quaternion _RotationAppear;			//	Rotate layer to appear
	private HUD _HUD;							//	Reference to the HUD
	private HUDTextManager _HUDTextManager;		//	Reference to HUD text manager
	private AudioSource _AudioSource;			//	Reference to Audio Source
	private AstarPath _AIManager;				//	Manages collision detction and pathfinding for AI 
	private CameraShake _MainCamera;			//	Reference to game's camerashake script
	private GameObject _PauseScreen;			//	Reference to pause screen

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start ()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}

		audioLevel = 0.75f;
		shakeIntensity = 0.75f;

		GameData.gameData.storedAudioLevel = audioLevel;
		GameData.gameData.storedScreenShakeInetnsity = shakeIntensity;

		//	Game starts paused though
		isPaused = false;

		//	Initalizes spwan point
		spawnPoint = GameObject.FindGameObjectWithTag ("Respawn").transform;

		//	values to hide and show the active color
		_RotationHide = Quaternion.Euler (0, 90, 0);
		_RotationAppear = Quaternion.Euler (0, 0, 0);

		//	HUD values
		_HUD = GameObject.FindGameObjectWithTag ("HUD").GetComponent<HUD>();
		_HUDTextManager = GameObject.FindGameObjectWithTag ("HUDText").GetComponent<HUDTextManager> ();

		//	Audio
		_AudioSource = GetComponent<AudioSource> ();

		//	To draw updated collision boxes for AI
		_AIManager = GameObject.FindGameObjectWithTag ("AIManager").GetComponent<AstarPath> ();

		//	Main Camera for screen shake
		_MainCamera = GetComponent<CameraShake> ();

		//	Player reference
		_Player = GameObject.FindGameObjectWithTag ("Player");

		//	Reference to layers
		_RedLayer = GameObject.FindGameObjectWithTag ("Layer_RED");
		_GreenLayer = GameObject.FindGameObjectWithTag ("Layer_GRN");
		_BlueLayer = GameObject.FindGameObjectWithTag ("Layer_BLU");

		_PauseScreen = GameObject.FindGameObjectWithTag ("PauseScreen");
		TogglePauseScreen (false);
		//	Level starts with no layers active
		HideAll ();

		//	Updates AI collision boxes
		_AIManager.Scan ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	RespawnPlayer: Creates a new instance of the player prefab							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public IEnumerator RespawnPlayer()
	{
		_AudioSource.PlayOneShot (deadAudio, 1f * audioLevel);
		yield return new WaitForSeconds (spawnDelay);

		_Player = (GameObject)Instantiate (PrefabManager.Instance.PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
		_Player.transform.parent = GameObject.FindGameObjectWithTag ("Player").transform;
		_HUDTextManager.player = _Player.GetComponent<Player>();
		_MainCamera.player = _Player.GetComponent<Player> ();
		GameObject.FindGameObjectWithTag ("CameraShake").GetComponent<CameraFollow2D> ().target = _Player.transform;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	KillPlayer: destroys player and runs RespawnPlayer()								*/
	/*		param:	Player player - the current player in the game							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public static void KillPlayer (Player player)
	{
		_Player = player.gameObject;
		Destroy (player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	HideAll: Hides all platforms and deactives thier collision meshes					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void HideAll()
	{
		redIsActive = false;
		greenIsActive = false;
		blueIsActive = false;

		_RedLayer.transform.rotation = _RotationHide;
		_GreenLayer.transform.rotation = _RotationHide;
		_BlueLayer.transform.rotation = _RotationHide;

		_RedLayer.GetComponent<PolygonCollider2D>().enabled = false;
		_GreenLayer.GetComponent<PolygonCollider2D>().enabled = false;
		_BlueLayer.GetComponent<PolygonCollider2D>().enabled = false;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	TogglePLatforms: destroys player and runs RespawnPlayer()							*/
	/*		param:	Player player - the current player in the game							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void TogglePlatforms(int color)
	{
		GameData.gameData.storedActiveColor = color;
		_MainCamera.Shake (cameraShakeAmount * shakeIntensity, cameraShakeLength);
		HideAll ();
		switch (color)
		{
		case 0:
			if(!redIsActive)
			{
				//  Rotates HUD sprite to put activated color(RED) on top
				if(!blueIsActive && !redIsActive && !greenIsActive)
				{
					_HUD.RotateHUD(180);
				}
				if (blueIsActive)
				{
					_HUD.RotateHUD(180);
				}
				if (greenIsActive)
				{
					_HUD.RotateHUD(180);
				}

			}

			if (_Player.GetComponent<Player>().playerStats.playerRed > -1)
			{
				//  Sets RED color to active
				redIsActive = true;
			
				//  Displays Only RED and GRAY platforms
				_RedLayer.transform.rotation = _RotationAppear;

				//  Plays toggle sound effect
				//toggleFX.play("", 0, .5,false, false);

				//  Set collisions for RED layer to true
				_RedLayer.GetComponent<PolygonCollider2D>().enabled = true;

			}
				
			break;
		case 1:
			if(!greenIsActive)
			{
				//  Rotates HUD sprite to put activated color(RED) on top
				if(!blueIsActive && !redIsActive && !greenIsActive)
				{
					_HUD.RotateHUD(-60);
				}
				if (blueIsActive)
				{
					_HUD.RotateHUD(-60);
				}
				if (greenIsActive)
				{
					_HUD.RotateHUD(-60);
				}

			}

			if (_Player.GetComponent<Player>().playerStats.playerGreen > -1)
			{
				//  Sets GREEN color to active
				greenIsActive = true;
				//  Displays Only GREEN and GRAY platforms
				_GreenLayer.transform.rotation = _RotationAppear;

				//  Plays toggle sound effect
				//toggleFX.play("", 0, .5,false, false);

				//  Set collisions for GREEN layer to true
				_GreenLayer.GetComponent<PolygonCollider2D>().enabled = true;
			}

			break;
		case 2:
			if(!blueIsActive)
			{
				//  Rotates HUD sprite to put activated color(RED) on top
				if(!blueIsActive && !redIsActive && !greenIsActive)
				{
					_HUD.RotateHUD(60);
				}
				if (blueIsActive)
				{
					_HUD.RotateHUD(60);
				}
				if (greenIsActive)
				{
					_HUD.RotateHUD(60);
				}

			}

			if (_Player.GetComponent<Player>().playerStats.playerBlue > -1)
			{
				//  Sets BLUE color to active
				blueIsActive = true;
				//  Displays Only BLUE and GRAY platforms
				_BlueLayer.transform.rotation = _RotationAppear;

				//  Plays toggle sound effect
				//toggleFX.play("", 0, .5,false, false);

				//  Set collisions for BLUE layer to true
				_BlueLayer.GetComponent<PolygonCollider2D>().enabled = true;
			}
				
			break;
		default:
			break;
		}

		_AIManager.Scan ();
	}

	void TogglePauseScreen(bool showScreen)
	{
		_PauseScreen.SetActive (showScreen);
	}


	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			
			isPaused = !isPaused;
			TogglePauseScreen (isPaused);
			if (isPaused)
			{
				Time.timeScale = 0;

			}
			else
			{
				Time.timeScale = 1;	
			}


		}
	}
}
