using UnityEngine;
using System.Collections;
using Pathfinding;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	BasicEnemyAI: Controller for basic enemy movements									*/
/*			Requires: Rigidbody2D, Seeker from A* Pathfinding Project					*/
/*																						*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			SearchForPlayer ()															*/
/*			OnPathComplete (Path p)														*/
/*			UpdatePath ()																*/
/*			CheckIfInLightPolygon ()													*/
/*			Update()																	*/
/*			FixedUpdate	()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class BasicEnemyAI : MonoBehaviour 
{
	//	Public Variables
	public bool isOff;							//	Turns off movement and colliders for enemy
	public bool followPlayer;					//	Should we follow player
	public bool canMove;						//	Can you move
	public bool inLight;						//	In light polygon
	public float updateRate = 2f;				//	How often Enemy AI seraches for target per second
	public float speed = 300f;					//	AI's speed per second
	public float moveSpeed = 50f;				//	Movement speed
	public float nextWayPointDistance = 3f;		//	The max distance from the AI to a way point for it to continue to the next way point
	public Transform target;					//	What the Enemy AI is moving towards
	public LayerMask enemyMask;					//	Reference to EnemyMask
	public TagMask sameTypeMask;				//	Mask of enemies of the same type
	public ForceMode2D fMode;					//	Chnages the way force is applied to the rigidbody.
	public Path path;							//	The calculated path for the AI

	//	Hidden in Inspector
	[HideInInspector]
	public bool pathIsEnded = false;			//	Let's us know when path is ended.

	//	Private Variables
	private bool _SerahcingForPlayer = false;	//	Let's us know when we should search for the player.
	private float _Width;						//	Reference to player's width
	private float _Height;						//	Refernece to player's height
	private int _CurrentWayPoint = 0;			//	The way point the AI is currenlty moving towards
	private Seeker _Seeker;						//	Reference to Seeker component
	private Rigidbody2D _Rigidbody2D;			//	Reference to out Rigidbody2D component
	private LightPolygon _LightPolygon;			//	Reference to the light polygon

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start () 
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer> ();

		_Width = spriteRenderer.bounds.extents.x;
		_Height = spriteRenderer.bounds.extents.y;

		_Seeker = GetComponent<Seeker> ();
		_Rigidbody2D = GetComponent<Rigidbody2D> ();

		isOff = false;
		followPlayer = false;

		if (gameObject.tag == "Enemy_YLLW")
		{
			_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
		}
		if (gameObject.tag == "Enemy_CYAN")
		{
			_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
		}
		if (gameObject.tag == "Enemy_MGNT")
		{
			_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		if (gameObject.tag == "Enemy_BLCK")
		{
			_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			_LightPolygon = GameObject.FindGameObjectWithTag ("LightSource").GetComponent<LightPolygon> ();
			followPlayer = true;
			inLight = true;
			StartCoroutine (CheckIfInLightPolygon ());
		}

		if (target == null)
		{
			if (!_SerahcingForPlayer)
			{
				_SerahcingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return;
		}
			
		//	Starts a new path to the target position, returns result to OnPathComplete method
		_Seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath());
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SearchForPlayer: Searches for player every second if target is null					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator SearchForPlayer()
	{
		GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
		if (searchResult == null)
		{
			yield return new WaitForSeconds (1.0f);
			StartCoroutine (SearchForPlayer ());
		}
		else
		{
			_SerahcingForPlayer = false;
			target = searchResult.transform;
			StartCoroutine (UpdatePath ());
			return false;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	OnPathComplete:	Makes sure the path does not generate any errors					*/
	/*			param:	Path p	-	A list of nodes connecting the AI to the target			*/
	/*																						*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			_CurrentWayPoint = 0;
		}
		else
		{
			Debug.Log ("Path Error: " + p.error);
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	UpdatePath: Updates path updateRate times per second								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator UpdatePath()
	{
		if (target == null)
		{
			if (!_SerahcingForPlayer)
			{
				_SerahcingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return false;
		}

		//	Starts a new path to the target position, returns result to OnPathComplete method
		_Seeker.StartPath (transform.position, target.position, OnPathComplete);

		yield return new WaitForSeconds (1f / updateRate);
		StartCoroutine (UpdatePath ());
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	CheckIfInLightPolygon: Only used in Black Enemy sprite								*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator CheckIfInLightPolygon()
	{
		bool searchResult = _LightPolygon.visibleTargets.Contains (transform);

		if (!searchResult)
		{
			inLight = false;
		}

		yield return new WaitForSeconds (0.5f / updateRate);
		StartCoroutine (CheckIfInLightPolygon ());
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update()
	{
		//	BLACK enemies do not care what color is active
		if (tag == "Enemy_YLLW" && GameMaster.gm.blueIsActive)
		{
			//	YELLOW enemies do not move when blue is active
			isOff = true;
		}
		else if (tag == "Enemy_CYAN" && GameMaster.gm.redIsActive)
		{
			//	CYAN enemies do not move when red is active
			isOff = true;
		}
		else if (tag == "Enemy_MGNT" && GameMaster.gm.greenIsActive)
		{
			//	MAGENTA enemies do not move when green is active
			isOff = true;
		}
		else
		{
			isOff = false;
		}
	}
		
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FixedUpdate: Runs every fixed interval. Interval can be changed in Unity.			*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void FixedUpdate () 
	{
		if (target == null)
		{
			if (!_SerahcingForPlayer)
			{
				_SerahcingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return;
		}

		if (path == null)
		{
			return;
		}

		if (_CurrentWayPoint >= path.vectorPath.Count)
		{
			if (pathIsEnded)
			{
				return;
			}
				
			pathIsEnded = true;
			return;
		}

		pathIsEnded = false;

		//	Only move when active
		if (!isOff) 
		{
			//	Movement when player is in sight or when not in the light
			if ((tag != "Enemy_BLCK" || !inLight) && followPlayer) 
			{
				//	Finds direction to next way point
				Vector3 direction = (path.vectorPath [_CurrentWayPoint] - transform.position).normalized;
				direction *= speed * Time.fixedDeltaTime;

				//	Moves the AI

				_Rigidbody2D.AddForce (direction, fMode);

				float distance = Vector3.Distance (transform.position, path.vectorPath [_CurrentWayPoint]);

				if (distance < nextWayPointDistance) 
				{
					_CurrentWayPoint++;
					return;
				}
			}

			//	Movement for YELLOW enemies (left and right)
			if (!followPlayer && tag == "Enemy_YLLW") 
			{
				Vector2 lineCastPosition = transform.position.toVector2 () - transform.right.toVector2 () * _Width + Vector2.up * _Height;

				bool isGrounded = Physics2D.Linecast (lineCastPosition, lineCastPosition + Vector2.down, enemyMask);
				bool isBlocked = Physics2D.Linecast (lineCastPosition, lineCastPosition - transform.right.toVector2 () * 0.05f, enemyMask);

				//	If not grounded or blocked then turn around
				if (!isGrounded || isBlocked) 
				{
					Vector3 currentRotation = transform.eulerAngles;
					currentRotation.y += 180;
					transform.eulerAngles = currentRotation;
				}

				//	Always move "forward" when not following player
				Vector2 newVelcotiy = _Rigidbody2D.velocity;
				newVelcotiy.x = -transform.right.x * moveSpeed * Time.fixedDeltaTime;

				_Rigidbody2D.velocity = newVelcotiy;
			}

			//	Movement for CYAN enemies (up and down)
			if (!followPlayer && tag == "Enemy_CYAN") 
			{
				Vector2 lineCastPosition = transform.position.toVector2 () - transform.right.toVector2 () * 0.0f * _Width + Vector2.up * _Height;

				bool isBlockedBottom = Physics2D.Linecast (lineCastPosition, lineCastPosition + Vector2.down * 0.9f, enemyMask);
				bool isBlockedTop = Physics2D.Linecast (lineCastPosition, lineCastPosition + Vector2.up * 0.05f, enemyMask);

				//	If not grounded or bloacked then turn around
				if (isBlockedBottom || isBlockedTop) 
				{
					moveSpeed *= -1;
				}

				//	Always move forward when not following player
				Vector2 newVelcotiy = _Rigidbody2D.velocity;
				newVelcotiy.y = -transform.up.y * moveSpeed * Time.fixedDeltaTime;

				_Rigidbody2D.velocity = newVelcotiy;
			}
		}
	}
}
