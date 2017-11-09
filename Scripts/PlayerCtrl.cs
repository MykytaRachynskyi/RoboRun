using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Animator))]

public class PlayerCtrl : MonoBehaviour {

//	public float inputDelay;
//	public float forwardSpeed;
//	public float turnSpeed;
//	public float jumpSpeed;
//	public float gravity;
//
//	private Vector3 moveDirection = Vector3.zero;
//	private float xPos;
//	CharacterController ctrler;
//
//	float inputForward;


	public float speed = 10.0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool grounded = false;

	//Animator anim;
	void Awake () 
	{
//		GetComponent<Rigidbody>().freezeRotation = true;
//		GetComponent<Rigidbody>().useGravity = false;
	}

	void Start () 
	{
		//anim = GetComponent<Animator>();
		//xPos = transform.position.x;
	}

	void Update () 
	{
		//move ();
	}

	void FixedUpdate()
	{
		//move ();
		//keepOnTrack (xPos);



		if (grounded) 
		{
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(0, 0, Input.GetAxis("Horizontal"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;

			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = 0;
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

			// Jump
			if (canJump && Input.GetButton("Jump")) 
			{
				GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			}
		}
	}

//	void move ()
//	{
//		CharacterController ctrler = GetComponent<CharacterController> ();
//		if (ctrler.isGrounded) 
//		{
//			inputForward = Input.GetAxis ("Horizontal");
//			if (inputForward > 0)
//				transform.localRotation = Quaternion.Euler (0, 0, 0);
//			else if (inputForward < 0)
//				transform.localRotation = Quaternion.Euler (0, 180, 0);
//			moveDirection = new Vector3 (0, 0, Mathf.Abs(inputForward));
//			moveDirection = transform.TransformDirection (moveDirection);
//			moveDirection *= forwardSpeed;
//
//			//anim.SetFloat ("SpeedForw", Mathf.Abs (inputForward));
//			if (Input.GetButton ("Jump"))
//				moveDirection.y = jumpSpeed;
//		}
//		moveDirection.y -= gravity * Time.fixedDeltaTime;
//		ctrler.Move(moveDirection * Time.fixedDeltaTime);
//	}

//	void keepOnTrack (float xPos)
//	{
//		if (xPos != transform.position.x) 
//			transform.position = new Vector3 (xPos, transform.position.y, transform.position.z);
//	}
	
	void OnCollisionStay () 
	{
		Debug.Log ("HIT THE GROUND!");
		grounded = true;    
	}

	float CalculateJumpVerticalSpeed () 
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	
}
