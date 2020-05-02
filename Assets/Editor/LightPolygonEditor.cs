using UnityEngine;
using System.Collections;
using UnityEditor;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LightPolygonEditor: Inspector Editor for LightPolygon								*/
/*																						*/
/*		Functions:																		*/
/*			OnSceneGUI ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
[CustomEditor (typeof (LightPolygon))]
public class LightPolygonEditor : Editor 
{

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnSceneGUI: Draws GUI on the scene view												*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void OnSceneGUI()
	{
		//	Reference to LightPolyogn script
		LightPolygon lightpolygon = (LightPolygon)target;
		Handles.color = Color.white;
		//	Draws viewRadius
		Handles.DrawWireArc (lightpolygon.transform.position, Vector3.forward, Vector3.up, 360, lightpolygon.viewRadius);

		//	Draws viewAngle
		Vector3 viewAngleA = lightpolygon.DirectionFromAngle (-lightpolygon.viewAngle / 2, false);
		Vector3 viewAngleB = lightpolygon.DirectionFromAngle (lightpolygon.viewAngle / 2, false);
		Handles.DrawLine (lightpolygon.transform.position, lightpolygon.transform.position + viewAngleA * lightpolygon.viewRadius);
		Handles.DrawLine (lightpolygon.transform.position, lightpolygon.transform.position + viewAngleB * lightpolygon.viewRadius);

		Handles.color = Color.red;

		//	Draws line if visibleTargets are in the light
		foreach (Transform visibleTargets in lightpolygon.visibleTargets)
		{
			Handles.DrawLine (lightpolygon.transform.position, visibleTargets.position);
		}
	}
}
