using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	CameraCollow2D: Camera movement script												*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			FindPlayer ()																*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class CameraFollow2D : MonoBehaviour
{
	//	Public Variables
	public Transform target;						//	What the camera is fixed on
	public float damping = 1.0f;					//	Damping factor for camera
	public float lookAheadFactor = 3.0f;			//	how far ahead the camera can be
	public float lookAheadReturnSpeed = 0.5f;		//	How fast the camera snaps back to the target
	public float lookAheadMoveThreshold = 0.1f;		//	How far ahead the camera goes ahead of the target
	public float yPosBoundary = -5.0f;				//	The lowest the camera can go in the y direction
	public float xPosBoundary = 353.5f;				//	The furthest the camera can go in the x direction
	public float xNegBoundary = 0.0f;				//	The lowest the camera can go in the x direction
	public float nextTimeToSearch = 0;				//	How long unitl the camera searches for the target again

	//	Private Variabels
	private float m_OffsetZ;						//	...
	private Vector3 m_LastTargetPosition;			//	Where the camera's target was last frame
	private Vector3 m_CurrentVelocity;				//	Velocity of the camera
	private Vector3 m_LookAheadPos;					//	Where the camera is set to when it's looking ahead

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		m_LastTargetPosition = target.position;
		m_OffsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FindPlayer: Seraches for player incase they are ever set to null					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void FindPlayer()
	{
		if (nextTimeToSearch <= Time.time)
		{
			GameObject result = GameObject.FindGameObjectWithTag ("Player");
			if (result != null)
				target = result.transform;

			nextTimeToSearch = Time.time + 2.0f;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
		if (target == null)
		{
			FindPlayer ();
			return;
		}

		float xMoveDelta = (target.position - m_LastTargetPosition).x;

		bool updateLookAheadTarget = Mathf.Abs (xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget)
		{
			m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign (xMoveDelta);
		}
		else
		{
			m_LookAheadPos = Vector3.MoveTowards (m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
		}

		Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp (transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

		newPos = new Vector3 (newPos.x, Mathf.Clamp (newPos.y, yPosBoundary, Mathf.Infinity), newPos.z);

		if (newPos.x > xPosBoundary) 
		{
			newPos.x = xPosBoundary;
		}

		if (newPos.x < xNegBoundary) 
		{
			newPos.x = xNegBoundary;
		}

		transform.position = newPos;

		m_LastTargetPosition = target.position;
	}
}
