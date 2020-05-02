using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackToMainMenu : MonoBehaviour {

	
	// Update is called once per frame
	void Update () 
	{
		if (SceneManager.GetActiveScene().name == "ControlsMenu" && Input.GetKeyDown(KeyCode.P))
		{
			Destroy (GameObject.Find ("GameData"));
			SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
		}
	}
}
