using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenShakeSlider : MonoBehaviour {

	public Slider screenShakeSlider;

	// Use this for initialization
	void Start () 
	{
		screenShakeSlider = GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () 
	{
		GameMaster.gm.shakeIntensity = screenShakeSlider.value;
	}
}
