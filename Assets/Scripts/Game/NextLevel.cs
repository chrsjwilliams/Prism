using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	NextLevel: Handles inter level connection. 											*/
/*		Funtions:																		*/
/*			OnTriggerEnter2D (Collider other)											*/
/*																						*/
/*--------------------------------------------------------------------------------------*/

//	May use this script for loading screens
//	http://blog.teamtreehouse.com/make-loading-screen-unity

public class NextLevel : MonoBehaviour 
{

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnTriggerEnter2D:																	*/
	/*		param: Collider2D other - 														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnTriggerEnter2d(Collider2D other)
	{
		if (other.tag == "Player")
		{
			if (tag == "ToLevel2")
			{
				SceneManager.LoadScene ("Level_2", LoadSceneMode.Single);
			}
			else if (tag == "ToLevel3")
			{
				
			}
			else if (tag == "ToLevel4")
			{
				
			}
			else if (tag == "ToLevel5")
			{

			}
			else if (tag == "ToLevel6")
			{

			}
			else if (tag == "ToLevel7")
			{

			}
			else if (tag == "ToLevel8")
			{

			}
		}
	}
}
