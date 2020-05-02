using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	LightPolygon: Creates LightPolygon in Prism											*/
/*																						*/
/*		Structs:																		*/
/*			LightPolygonInfo															*/
/*			EdgeInfo																	*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			ViewLightPolygon (float globalAngle)										*/
/*			FindEdge (LightPolygonInfo min, LightPolygonInfo max)						*/
/*			FindPlayerWithDelay (float delay)											*/
/*			DirectionFromAngle (float angleInDegrees, bool angleIsGlobal)				*/
/*			FindVisiblePlayer ()														*/
/*			DrawLightPolygon ()															*/
/*			Update ()																	*/
/*			LateUpdate ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class LightPolygon : MonoBehaviour 
{
	//	Public Variables
	public float viewRadius;											//	How far the light projects out
	[Range(0, 360)]														//	0 degrees to 360 degrees
	public float viewAngle;												//	In how many degrees the light projects out
	public float lightSpeed;											//	How fast the light source moves across the screen
	public float meshResolution;										//	How many rays are cast out
	public float edgeDistanceThreshold;									//	Threshold to determine if two edges are seprate objects
	public float _RightLimit;											//	How far right the light source can go
	public float _LeftLimit;											//	How far left the lgiht source can go
	public int edgeResolveIterations;									//	...
	public LayerMask playerMask;										//	Layer Mask for the player
	public LayerMask platformMask;										//	Layer Mask for the platforms/ground
	public MeshFilter viewMeshFilter;									//	The mesh drawn to represent the light
	public Collider2D[] playerInViewRadius;								//	List of colliders in view radius (currently holds only player)
	public List<Transform> visibleTargets = new List<Transform>();		//	List of objects in the light (currently holds only player)

	//	Private Variables
	private Vector3 _Position;											//	Position of the light source
	private Mesh _ViewMesh;												//	Reference to the visible mesh

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		_ViewMesh = new Mesh ();
		_ViewMesh.name = "View Mesh";
		viewMeshFilter.mesh = _ViewMesh;
		StartCoroutine (FindPlayerWithDelay (0.2f));
		_Position = transform.position;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	[STRUCT]																			*/
	/*	LightPolygonInfo: Holds info for Light Polygon										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public struct LightPolygonInfo
	{
		public bool hit;				//	Has the RayCast2D collided with something
		public Vector2 point;			//	Point of collision for RayCast2D
		public float distance;			//	How long the RayCast2D is
		public float angle;				//	Angle of RayCast2D

		/*--------------------------------------------------------------------------------------*/
		/*																						*/
		/*	[CONSTRUCTOR]																		*/
		/*	LightPolygonInfo: Stores Raycast info												*/
		/*				param:																	*/
		/*					bool _hit -	did the RayCast2D hit									*/
		/*					Vector2 _point - Point where RayCast2D hit							*/
		/*					float _distance	- How long the RayCast2D is							*/
		/*					float _angle - Angle of RayCast2D									*/
		/*																						*/
		/*--------------------------------------------------------------------------------------*/
		public LightPolygonInfo(bool _hit, Vector2 _point, float _distance, float _angle)
		{
			hit = _hit;
			point = _point;
			distance = _distance;
			angle = _angle;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	[STRUCT]																			*/
	/*	EdgeInfo: Optimizes LightPolygon drawing											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public struct EdgeInfo
	{
		public Vector3 pointA;		//	PointA...
		public Vector3 pointB;		//	PointB...

		/*--------------------------------------------------------------------------------------*/
		/*																						*/
		/*	[CONSTRUCTOR]																		*/
		/*	EdgeInfo: Casts a ray on the egde													*/
		/*				param:																	*/
		/*					Vector3 _PointA	-													*/
		/*					Vector3 _PointB	-													*/
		/*																						*/
		/*--------------------------------------------------------------------------------------*/
		public EdgeInfo (Vector3 _PointA, Vector3 _PointB)
		{
			pointA = _PointA;
			pointB = _PointB;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	ViewLightPolygon: Gathers info to draw LightPolygon									*/
	/*		param: 																			*/
	/*			float globalAngle - The global angle for RayCast2D							*/
	/*																						*/
	/*		returns:																		*/
	/*			LightPolygonInfo - hit points and distacen to draw LightPolygon				*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	LightPolygonInfo ViewLightPolygon(float globalAngle)
	{
		Vector3 direction = DirectionFromAngle (globalAngle, true);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, viewRadius, platformMask); 

		if (hit)
		{
			//	Creates a new Ray with the appropriate info
			return new LightPolygonInfo (true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			//	Creates a new Ray that extends to the viewRadius
			return new LightPolygonInfo (false, transform.position + direction * viewRadius, viewRadius, globalAngle);
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FindEdge: Performs optimization for drawing LightPolygon over adjacent edges		*/
	/*			  by drawing a ray on the egde always										*/
	/*		param: 																			*/
	/*			LightPolygonInfo min - The global angle for light Polygon					*/
	/*			LightPolygonInfo max - The global angle for light Polygon					*/
	/*																						*/
	/*		returns:																		*/
	/*			EdgeInfo - with hit points and distacen to draw LightPolygon				*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	EdgeInfo FindEdge(LightPolygonInfo min, LightPolygonInfo max)
	{
		float minAngle = min.angle;
		float maxAngle = max.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for(int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			LightPolygonInfo newLightPolygon = ViewLightPolygon (angle);

			bool edgeDistanceThresholdExceeded = Mathf.Abs(min.distance - newLightPolygon.distance) > edgeDistanceThreshold;
			if (newLightPolygon.hit == min.hit && !edgeDistanceThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newLightPolygon.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newLightPolygon.point;
			}
		}

		return new EdgeInfo (minPoint, maxPoint);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FindPlayerWithDelay: Finds the player after delay seconds							*/
	/*		param: 																			*/
	/*			float delay - how many seconds until next search							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator FindPlayerWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds (delay);
			FindVisiblePlayer ();
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DirectionFromAngle: Takes in angle and outputs direction of angle					*/
	/*		param: 																			*/
	/*			float angleInDegrees - angle in degrees										*/
	/*			bool angleIsGlobal - is angle global										*/
	/*																						*/
	/*		returns:																		*/
	/*			Vector3 - direction with angle in degrees									*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		//	Makes angle global if not global already
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angleInDegrees * Mathf.Deg2Rad), Mathf.Cos (angleInDegrees * Mathf.Deg2Rad), 0);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FindVisiblePlayer: Finds the player if not obstucted								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void FindVisiblePlayer()
	{
		//	So we do not have dupilcates in the list
		visibleTargets.Clear ();

		//	Collects all colliders in view radius
		playerInViewRadius = Physics2D.OverlapCircleAll (transform.position, viewRadius, playerMask);

		for (int i = 0; i < playerInViewRadius.Length; i++)
		{
			Transform target = playerInViewRadius [i].transform;
			Vector3 directionToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, directionToTarget) < viewAngle / 2)
			{
				float distanceToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics2D.Raycast (transform.position, directionToTarget, distanceToTarget, platformMask)) 
				{
					visibleTargets.Add (target);
					if (target.tag == "Player" && !GameMaster.playerIsDead) 
					{
						target.gameObject.GetComponent<Player> ().ChargeUp (true, true, true);
					}
					if (target.tag == "Enemy_BLCK" && SceneManager.GetActiveScene ().name != "ControlsMenu")
					{
						target.GetComponent<BasicEnemyAI> ().inLight = true;
					}

				}
			}
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DrawLightPolygon: Draws the LightPolygonMesh in gameview							*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void DrawLightPolygon()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		//	Determines how many degrees are in each step
		float stepAngleSize = viewAngle / stepCount;

		//	List of all the points the RayCast2D hit
		List<Vector3> viewPoints = new List<Vector3> ();

		LightPolygonInfo oldLightPolygon = new LightPolygonInfo ();
		for (int i = 0; i <= stepCount; i++)
		{
			//	Roates clockwise until we reach thr right most ray.
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			LightPolygonInfo newLightPolygon = ViewLightPolygon(angle);

			if (i > 0)
			{
				bool edgeDistanceThresholdExceeded = Mathf.Abs(oldLightPolygon.distance - newLightPolygon.distance) > edgeDistanceThreshold;
				if (oldLightPolygon.hit != newLightPolygon.hit || (oldLightPolygon.hit && newLightPolygon.hit && edgeDistanceThresholdExceeded))
				{
					EdgeInfo edge = FindEdge (oldLightPolygon, newLightPolygon);
					if (edge.pointA != Vector3.zero)
					{
						viewPoints.Add (edge.pointA);
					}
					if (edge.pointB != Vector3.zero)
					{
						viewPoints.Add (edge.pointB);
					}
				}
			}

			viewPoints.Add (newLightPolygon.point);
			oldLightPolygon = newLightPolygon;
		}

		//	Actually draws the viewMesh
		int vertextCount = viewPoints.Count + 1;
		Vector3 [] verticies = new Vector3[vertextCount];
		int [] triangles = new int[(vertextCount - 2) * 3];
		verticies [0] = Vector2.zero;
		for (int i = 0; i < vertextCount - 2; i++)
		{
			//	Makes vertices local space instead of global space
			verticies [i + 1] = transform.InverseTransformPoint(viewPoints [i]);

			//	Creates the viewMesh from the GameObject's origin point
			if (i < vertextCount - 2) 
			{
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		_ViewMesh.Clear ();
		_ViewMesh.vertices = verticies;
		_ViewMesh.triangles = triangles;
		_ViewMesh.RecalculateNormals ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update()
	{
		if (!NextLevel.loadingLevel.isLoading || SceneManager.GetActiveScene ().name == "ControlsMenu") 
		{
			//	Lightsource
			_Position.x += lightSpeed; 

			if (_Position.x > _RightLimit || _Position.x < _LeftLimit) 
			{
				lightSpeed *= -1;
			}

			if (SceneManager.GetActiveScene ().name != "ControlsMenu") 
			{
				if (lightSpeed > 0) {
					GameData.gameData.storedLightDirection = 1;
				} else {
					GameData.gameData.storedLightDirection = -1;
				}
			}
				transform.position = _Position;
			
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	LateUpdate: Called after Upate()													*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void LateUpdate () 
	{
		//	Draws LightPolygon after rotations to stop any strange visual glitches 
		DrawLightPolygon ();
	}
}
