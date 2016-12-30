using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDTextManager : MonoBehaviour
{


	public Player player;

	//	Private Variables
	private Text _Text;
	private Color _TextOutlineColor;
	private Outline _Outline;
	private GameMaster _GM;

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
		_GM = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();

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
		else if (_GM.redIsActive)
		{
			_TextOutlineColor = new Color (0.8f, 0f, 0f, 0.9f);
			_Text.text = player.playerStats.playerRed.ToString();
		}
		else if (_GM.greenIsActive)
		{
			_TextOutlineColor = new Color (0f, 0.8f, 0f, 0.9f);
			_Text.text = player.playerStats.playerGreen.ToString();
		}
		else if (_GM.blueIsActive)
		{
			_TextOutlineColor = new Color (0f, 0f, 0.8f, 0.9f);
			_Text.text = player.playerStats.playerBlue.ToString();
		}
		else if (!_GM.redIsActive && !_GM.greenIsActive && !_GM.blueIsActive)
		{
			_TextOutlineColor = new Color (0f, 0f, 0f, 0.75f);

			_TextOutlineColor = new Color (0f, 0f, 0f, 0.75f);
			_Text.text = "0";
		}

		_Outline.effectColor = _TextOutlineColor;
	}
}
