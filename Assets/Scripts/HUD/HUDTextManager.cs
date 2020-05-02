using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	HUDTextManager: Changes text ouline color and text of HUD							*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class HUDTextManager : MonoBehaviour
{
	//	Public Variables
	public Player player;				//	Refernece to player

	//	Private Variables
	private Text _Text;					//	Displays the number of charge
	private Color _TextOutlineColor;	//	Color of outline of _Text
	private Outline _Outline;			//	Reference to outline of _Text

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_Text = GetComponent<Text> ();
		_TextOutlineColor = new Color (0f, 0f, 0f, 0.75f);
		_Outline = GetComponent<Outline> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		_Outline.effectColor = _TextOutlineColor;
	}
	
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if(player.playerStats.playerRed == player.playerStats.MAX_CHARGE &&
			player.playerStats.playerGreen == player.playerStats.MAX_CHARGE &&
			player.playerStats.playerBlue == player.playerStats.MAX_CHARGE) 
		{
			_TextOutlineColor = new Color (0.8f, 0.8f, 0.8f, 0.75f);
			_Text.text = "3";
		} 
		else if (GameMaster.gm.redIsActive)
		{
			_TextOutlineColor = new Color (0.8f, 0f, 0f, 0.9f);
			_Text.text = player.playerStats.playerRed.ToString();
		}
		else if (GameMaster.gm.greenIsActive)
		{
			_TextOutlineColor = new Color (0f, 0.8f, 0f, 0.9f);
			_Text.text = player.playerStats.playerGreen.ToString();
		}
		else if (GameMaster.gm.blueIsActive)
		{
			_TextOutlineColor = new Color (0f, 0f, 0.8f, 0.9f);
			_Text.text = player.playerStats.playerBlue.ToString();
		}
		else if (!GameMaster.gm.redIsActive && !GameMaster.gm.greenIsActive && !GameMaster.gm.blueIsActive)
		{
			_TextOutlineColor = new Color (0f, 0f, 0f, 0.75f);

			_TextOutlineColor = new Color (0f, 0f, 0f, 0.75f);
			_Text.text = "0";
		}

		_Outline.effectColor = _TextOutlineColor;
	}
}
