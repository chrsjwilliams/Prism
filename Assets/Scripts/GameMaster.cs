using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	GameMaster: Manages states of the game												*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			RespawnPlayer ()															*/
/*			KillPlayer ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class GameMaster : MonoBehaviour 
{
	//	Static variables
	public static GameMaster gm;			//	Reference to Game Master game object

	//	Public Variables
	public int spawnDelay = 2;				//	Spawen delay for player
	public Transform playerPrefab;			//	Reference to player prefab
	public Transform spawnPoint;			//	reference to level's spawn point

	//	Private Variables
	private static GameObject m_Player;		//	Reference to player Game Object

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	void Start ()
	{
		if (gm == null)
		{
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	RespawnPlayer: Creates a new instance of the player prefab							*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	public IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds (spawnDelay);

		m_Player = (GameObject)Instantiate (PrefabManager.Instance.PlayerPrefab, spawnPoint.position, spawnPoint.rotation);
		m_Player.transform.parent = GameObject.Find ("Player").transform;
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraFollow2D> ().target = m_Player.transform;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	KillPlayer: destroys player and runs RespawnPlayer()								*/
	/*		param:	Player player - the current player in the game							*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	public static void KillPlayer (Player player)
	{
		m_Player = player.gameObject;
		Destroy (player.gameObject);
		gm.StartCoroutine(gm.RespawnPlayer());
	}

}
