using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioSlider : MonoBehaviour {

	public Slider audioSlider;

	// Use this for initialization
	void Start () 
	{
		audioSlider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameMaster.gm.audioLevel = audioSlider.value;
	}
}
