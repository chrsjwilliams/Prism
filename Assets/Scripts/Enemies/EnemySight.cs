using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	EnemySight: What the enemy charcters can see										*/
/*																						*/
/*		Functions:																		*/
/*			Start()																		*/
/*			OnTriggerEnter2D (Collider2D other)											*/
/*			OnTriggerExit2D (Collider2D other)											*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class EnemySight : MonoBehaviour 
{
	//	Public Variables
	BasicEnemyAI enemy;					//	Reference to enemy

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		enemy = GetComponentInChildren<BasicEnemyAI> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnTriggerEnter2D:																	*/
	/*		param: Collider2D other - 														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !enemy.isOff)
		{
			enemy.followPlayer = true;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnTriggerExit2D:																	*/
	/*		param: Collider2D other - 														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			enemy.followPlayer = false;
		}
	}
}
