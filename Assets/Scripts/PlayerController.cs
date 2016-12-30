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
	public AudioClip jumpAudio;											//	Audio clip for jumping
	public AudioClip landedAudio;										//	Audio clip for landing

	//	Hidden From Inspector
	[HideInInspector]
	public Animator _Animator;											//	Reference to player's Animator
	[HideInInspector]
	private Collider2D[] _Colliders;									//	Array of player's colliders

	//	Serializable Fields
	[SerializeField] private bool _AirControl; 							// Whether or not player can steer while jumping
	[SerializeField] private float _MaxSpeed = 3f;						//	Fastest player can travel on X-axis
	[SerializeField] private float _JumpForce = 300f;					//	Amount of force added when player jumps
	[SerializeField] private LayerMask _WhatIsGround; 					//	A mask determining what is ground to character

	//	Private Variables
	private bool _FacingLeft;											//	Let's us know if our player is facing left
	private bool _Grounded;												//	Let's us know if player is grounded
	private bool _PlayLandedAudio;										//	LEt's us know if we should play landedAudio
	private float _InputDirection;										//	X value of _MoveVector
	private float _VerticalVelocity;									//	Y value of _MoveVector
	private Vector2 _MoveVector;										//	Vector that temporarily stores player movement
	private Transform _GroundCheck;										//	Transform for GroundCheck GameObject
	private Rigidbody2D _Rigidbody2D;									//	Reference to player's rigidbody2D
	private AudioSource _AudioSource;									//	Reference to Audio Source

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
		_AirControl = true;
		_GroundCheck = transform.Find ("GroundCheck");
		_Animator = GetComponent<Animator> ();
		_Rigidbody2D = GetComponent<Rigidbody2D>();
		_Colliders = this.GetComponents<Collider2D> ();
		_AudioSource = GetComponent<AudioSource> ();
		_PlayLandedAudio = true;
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
		if (_Grounded || _AirControl)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal inpput
			_Animator.SetFloat("Speed", Mathf.Abs(move));

			// Move character
			_Rigidbody2D.velocity = new Vector2(move * _MaxSpeed, _Rigidbody2D.velocity.y);

			if (move > 0 && !_FacingLeft)
			{
				// Flips player
				Flip ();
			}
			// Otherwise player should be facing left
			else if (move < 0 && _FacingLeft)
			{
				// Flips player
				Flip ();
			}
		}

		// If the player should jump
		if (_Grounded && jump && _Animator.GetBool("Ground"))
		{
			// Adds vertical force to player
			_Grounded = false;
			_Animator.SetBool ("Ground", false);
			_PlayLandedAudio = true;
			_Rigidbody2D.AddForce (new Vector2 (0f, _JumpForce));
			_AudioSource.PlayOneShot (jumpAudio, 0.5f);
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
		_FacingLeft = !_FacingLeft;

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
		foreach (Collider2D collider in _Colliders)
		{
			collider.enabled = false;
			collider.enabled = true;
		}
		// Start Flinching Animation
		_Animator.SetLayerWeight(1,1);

		// Wait for flinch timer to expire
		yield return new WaitForSeconds (flinchTimer);

		// Stops flinching animation and re-enables collision
		Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer, false);
		_Animator.SetLayerWeight (1, 0);
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
		_Grounded = false;

		//	The player is grounded if a circle cast to the ground position hits anything labeld as ground
		//	This can be done with layers
		Collider2D[] groundColliders = Physics2D.OverlapCircleAll (_GroundCheck.position, k_GroundedRadius, _WhatIsGround);
		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders [i].gameObject != gameObject) 
			{
				_Grounded = true;
			}
			if (_PlayLandedAudio)
			{
				_AudioSource.PlayOneShot (landedAudio, 1f);
				_PlayLandedAudio = false;
			}
		}
			
		//	Sets the Idle or Running animation
		_Animator.SetBool ("Ground", _Grounded);

		//	Set the vertical animation
		_Animator.SetFloat ("vSpeed", _Rigidbody2D.velocity.y);
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
		return _AirControl = b;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	isFacingLeft: tells us if character is facing left									*/
	/*		returns: True if character is facing left										*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public bool isFacingLeft()
	{
		return _FacingLeft;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetGrounded: Sets whether character is grounded										*/
	/*		param: bool b																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setGrounded(bool b)
	{
		_Grounded = b;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	isGrounded: tells us if character is grounded										*/
	/*		returns: True if character is grounded											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public bool isGrounded()
	{
		return _Grounded;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	getJumpForce: tells us what is the characer's jumpforce								*/
	/*		returns: the float value for _JumpForce											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public float getJumpForce()
	{
		return _JumpForce;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetJumpForce: Sets characetr's jump force											*/
	/*		param: float f																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setJumpForce(float f)
	{
		_JumpForce = f;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	getMaxSpeed: tells us what is the characetr's max speed								*/
	/*		returns: the float value for _MaxSpeed											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public float getMaxSpeed()
	{
		return _MaxSpeed;
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	SetMaxSpeed: Sets characetr's max speed												*/
	/*		param: float f																	*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void setMaxSpeed(float f)
	{
		_MaxSpeed = f;
	}
}
