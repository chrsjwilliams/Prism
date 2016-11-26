using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	PlayerController: Handles all movement and manages all states of the player			*/
/*		Functions:																		*/
/*			Awake ()																	*/
/*			Start ()																	*/
/*			Move (float move, bool jump)												*/
/*			Flip ()																		*/
/*			FlinchTimer (float flinchTimer)												*/
/*			TriggerDamage (float flinchTimer)											*/
/*			FixedUpdate ()																*/
/*			Update ()																	*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class PlayerController : MonoBehaviour 
{
	//	Insatnces
	public static PlayerController INSTANCE;							//	Stores an instacen of our player

	//	Public Variables
	public float defaultGravityScale = 10f;								//	Default gravity scale for player

	//	Hidden From Inspector
	[HideInInspector]
	public Animator m_Animator;											//	Reference to player's Animator
	[HideInInspector]
	private Collider2D[] m_Colliders;									//	Array of player's colliders

	//	Serializable Fields
	[SerializeField] private bool m_AirControl; 						// Whether or not player can steer while jumping
	[SerializeField] private float m_MaxSpeed = 10f;					//	Fastest player can travel on X-axis
	[SerializeField] private float m_JumpForce = 50f;					//	Amount of force added when player jumps
	[SerializeField] private LayerMask m_WhatIsGround; 					//	A mask determining what is ground to character

	//	Private Variables
	private bool m_FacingLeft;											//	Let's us know if our player is facing left
	private bool m_Grounded;											//	Let's us know if player is grounded
	private float m_InputDirection;										//	X value of m_MoveVector
	private float m_VerticalVelocity;									//	Y value of m_MoveVector
	private Vector2 m_MoveVector;										//	Vector that temporarily stores player movement
	private Transform m_GroundCheck;									//	Transform for GroundCheck GameObject
	private Rigidbody2D m_Rigidbody2D;									//	Reference to player's rigidbody2D

	//	Constants
	const float k_GroundedRadius = 0.01f;								//	Radius of GroundCheck GameObject
	const float k_CeilingRadius = 0.01f;								//	Radius of CeilingCheck GameObject

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Awake: Runs once when the object is created											*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	private void Awake()
	{
		INSTANCE = this;
		m_AirControl = true;
		m_GroundCheck = transform.Find ("GroundCheck");
		m_Animator = GetComponent<Animator> ();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_Colliders = this.GetComponents<Collider2D> ();
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/	
	void Start () 
	{
	
	}
		
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Move: Handles player movement														*/
	/*		param: float move - power of player movement									*/
	/*			   bool jump - checks if we should jump										*/
	/*																						*/
	///*--------------------------------------------------------------------------------------*/
	public void Move(float move, bool jump)
	{
		// Only control player if grounded or if airControl is on
		if (m_Grounded || m_AirControl)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal inpput
			m_Animator.SetFloat("Speed", Mathf.Abs(move));

			// Move character
			m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

			if (move > 0 && !m_FacingLeft)
			{
				// Flips player
				Flip ();
			}
			// Otherwise player should be facing left
			else if (move < 0 && m_FacingLeft)
			{
				// Flips player
				Flip ();
			}
		}

		// If the player should jump
		if (m_Grounded && jump && m_Animator.GetBool("Ground"))
		{
			// Adds vertical force to player
			m_Grounded = false;
			m_Animator.SetBool ("Ground", false);
			m_Rigidbody2D.AddForce (new Vector2 (0f, m_JumpForce));
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Flip: FLips player sprite right or left												*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void Flip ()
	{
		// Switch the way the player is labeled as facing
		m_FacingLeft = !m_FacingLeft;

		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FlinchTimer: Handles player flinching												*/
	/*		param: flinchTimer - how long the player should flinch for						*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	IEnumerator FlinchTimer(float flinchTimer)
	{
		// Ignores collisions with Enemies
		int enemyLayer = LayerMask.NameToLayer ("Enemies");
		int playerLayer = LayerMask.NameToLayer ("Player");

		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer);
		foreach (Collider2D collider in m_Colliders)
		{
			collider.enabled = false;
			collider.enabled = true;
		}
		// Start Flinching Animation
		m_Animator.SetLayerWeight(1,1);

		// Wait for flinch timer to expire
		yield return new WaitForSeconds (flinchTimer);

		// Stops flinching animation and re-enables collision
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer, false);
		m_Animator.SetLayerWeight (1, 0);
	}
		
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	TriggerDamage: Starts the FlinchTimer Coroutine										*/
	/*		param: flinchTimer - how long the player should flinch for						*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void TriggerDamage(float flinchTimer)
	{
		StartCoroutine (FlinchTimer (flinchTimer));
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	FixedUpdate: Called once per fixed amount of frames									*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	private void FixedUpdate()
	{
		//	We always assume we are not grounded
		m_Grounded = false;

		//	The player is grounded if a circle cast to the ground position hits anything labeld as ground
		//	This can be done with layers
		Collider2D[] groundColliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders [i].gameObject != gameObject)
				m_Grounded = true;
		}
			
		//	Sets the Idle or Running animation
		m_Animator.SetBool ("Ground", m_Grounded);

		//	Set the vertical animation
		m_Animator.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Update: Called once per frame														*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Update () 
	{
	
	}


	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetAirControl: Sets whether character can be controlled in the air					*/
	/*		param: bool b																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public bool setAirControl (bool b)
	{
		return m_AirControl = b;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	isFacingLeft: tells us if character is facing left									*/
	/*		returns: True if character is facing left										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public bool isFacingLeft()
	{
		return m_FacingLeft;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetGrounded: Sets whether character is grounded										*/
	/*		param: bool b																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setGrounded(bool b)
	{
		m_Grounded = b;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	isGrounded: tells us if character is grounded										*/
	/*		returns: True if character is grounded											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public bool isGrounded()
	{
		return m_Grounded;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	getJumpForce: tells us what is the characer's jumpforce								*/
	/*		returns: the float value for m_JumpForce										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public float getJumpForce()
	{
		return m_JumpForce;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetJumpForce: Sets characetr's jump force											*/
	/*		param: float f																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setJumpForce(float f)
	{
		m_JumpForce = f;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	getMaxSpeed: tells us what is the characetr's max speed								*/
	/*		returns: the float value for m_MaxSpeed											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public float getMaxSpeed()
	{
		return m_MaxSpeed;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetMaxSpeed: Sets characetr's max speed												*/
	/*		param: float f																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setMaxSpeed(float f)
	{
		m_MaxSpeed = f;
	}
}
