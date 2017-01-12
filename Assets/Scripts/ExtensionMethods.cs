using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	ExtensionMethods: Convenience methods												*/
/*																						*/
/*		Functions:																		*/
/*			toVector2(this Vector3 vec3)												*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public static class ExtensionMethods
{
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	toVector2: turns a Vector3 into a Vecotr2											*/
	/*		param: Vector3 vec3																*/
	/*		returns: the same Vector 3 without the z value in a Vector2						*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public static Vector2 toVector2(this Vector3 vec3)
	{
		return new Vector2(vec3.x, vec3.y);
	}
}
