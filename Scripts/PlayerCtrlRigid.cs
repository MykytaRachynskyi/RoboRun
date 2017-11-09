using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerCtrlRigid : MonoBehaviour {

	public GameObject feet;

	//Speed
	public float speed = 10.0f;
	public float maxVelocityChange = 10.0f;
	public float maxSpeed = 4.0f;

	//Jumping
	public bool jumpFlag = true;
	public float jumpHeight = 2.0f;
	public float floatHeight = 4.0f;
	public float inAirControl = 0.1f;
	public float maxJump = 10f;

	//Can Do
	public bool canRunSidestep = true;
	public bool canJump = true;
	public bool canRun = true;

	private Velocity velocity;
	private GroundedText groundedText;
	private bool facingForw = true;
	private bool grounded = false;
	private Vector3 groundVelocity;
	private CapsuleCollider capsule;	
	private float xPos;
	private float gravityMultiplier;
	private Vector3 startPosition;
	Animator anim;

	void Start () {
		//Speed'o'meter
		velocity = GameObject.Find("Speed").GetComponent<Velocity>();
		groundedText = GameObject.Find("GroundedText").GetComponent<GroundedText>();
		startPosition = transform.position;
		anim = GetComponent<Animator>();
		xPos = startPosition.x;
		capsule = GetComponent<CapsuleCollider>();
		GetComponent<Rigidbody>().freezeRotation = true;
		//GetComponent<Rigidbody>().useGravity = true;
	}

	void Update () {
		Speedometer();								 //Speedometer
		groundedText.UpdateText(grounded);

		if (grounded && GetComponent<Rigidbody> ().velocity != Vector3.zero)	    //If standing on the ground and the speed is non-zero
			anim.SetFloat ("SpeedForw", Mathf.Abs(Input.GetAxisRaw("Horizontal"))); //Start running animation

		if (Input.GetButtonDown("Jump")) 			//If space is pressed
			jumpFlag = true;			 			//Set jump trigger

		
	}

	void FixedUpdate ()
	{
		KeepOnTrack (xPos);
		gravityMultiplier = 1f;								
		//LimitSpeed();
		float inputDir = Input.GetAxis ("Horizontal"); //Non-raw for smooth acceleration; input (negative - left; positive - right) 
		Vector3 inputVector = new Vector3 (0, 0, inputDir);		//Convert the input direction into a directional vector
		if (facingForw && inputDir < 0) {						//There is no button press checking. If the input vector is (0,0,0), the
			Flip ();											//velocity gets multiplied by zero.
			facingForw = false;									//
		} else if (!facingForw && inputDir > 0) {				//Turning left and right
			Flip ();											//
			facingForw = true;									//
		}
		//Running---------------------------------------------------------------------
		if (grounded) { 
			Vector3 velocityChange = CalculateVelocityChange (inputVector);			     //Calculate velocity from the input vector
			GetComponent<Rigidbody> ().AddForce (velocityChange, ForceMode.VelocityChange);//Vel change because the acceleration is 
			//Jumping---------------------------------------------------------------------	 //provided by Non-raw input.GetAxis ln.63
			if (canJump && jumpFlag) {													 
				Vector3 jumpVel = new Vector3 (GetComponent<Rigidbody> ().velocity.x,   
					                  CalculateJumpVerticalSpeed (), 			//canJump variable only for future use to
					                  GetComponent<Rigidbody> ().velocity.z);   //restrict jumping, if necessary	 
				GetComponent<Rigidbody> ().velocity = jumpVel;
			}
			grounded = false;
		} else {
			Vector3 velocityChange = inputVector * inAirControl;                	      //Uses the input vector to affect the mid air 
			GetComponent<Rigidbody> ().AddForce (velocityChange, ForceMode.VelocityChange); //movement
		}
		//---------------------------------------------------------------------------
//		if (grounded) {
			if (GetComponent<Rigidbody> ().velocity.z > maxVelocityChange)
				GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, 
					GetComponent<Rigidbody> ().velocity.y, 
					maxVelocityChange);
			if (GetComponent<Rigidbody> ().velocity.z < -maxVelocityChange)
				GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, 
					GetComponent<Rigidbody> ().velocity.y, 
					-maxVelocityChange);
//		} else {
//			if (GetComponent<Rigidbody> ().velocity.z > maxVelocityChange * 2f)
//				GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, 
//					GetComponent<Rigidbody> ().velocity.y, 
//					maxVelocityChange);
//			if (GetComponent<Rigidbody> ().velocity.z < -maxVelocityChange * 2f)
//				GetComponent<Rigidbody> ().velocity = new Vector3 (GetComponent<Rigidbody> ().velocity.x, 
//					GetComponent<Rigidbody> ().velocity.y, 
//					-maxVelocityChange);
//		}
					
		jumpFlag = false;


		if (GetComponent<Rigidbody>().velocity.y < 0)
			gravityMultiplier = 3f;
		else if (GetComponent<Rigidbody>().velocity.y > 0 && !Input.GetButton("Jump")) 
			gravityMultiplier = 2f;
		
		GetComponent<Rigidbody>().AddForce(new Vector3(0, Physics.gravity.y * gravityMultiplier * Time.deltaTime, 0), ForceMode.Impulse);
	}

	void Speedometer () {
		Vector3 TestVel = new Vector3();
		TestVel = GetComponent<Rigidbody>().velocity;
		velocity.UpdateSpeed(TestVel);
	}

	void KeepOnTrack (float xPos) {
		if (xPos != transform.position.x) 
			transform.position = new Vector3 (xPos, transform.position.y, transform.position.z);
	}

	void Flip() {
		transform.Rotate (Vector3.up * 180);
	}

	//Collision detection & parenting
	void OnCollisionExit(Collision collision) {
		if (collision.transform == transform.parent)
			transform.parent = null;
	}

	void OnCollisionStay(Collision col) {
		TrackGrounded(col);
	}

	void OnCollisionEnter(Collision col) {
		TrackGrounded(col);
	}

	//Speed Calculations
	private Vector3 CalculateVelocityChange (Vector3 inputVector) {
		// Calculate how fast we should be moving
		//transform.TransformDirection(
		Vector3 relativeVelocity = inputVector;

		relativeVelocity.z *= (canRun) ? speed : 1f;

		// Calcualte the delta velocity
		Vector3 currRelativeVelocity = GetComponent<Rigidbody>().velocity - groundVelocity;
		Vector3 velocityChange = relativeVelocity - currRelativeVelocity;
		velocityChange.x = 0;
		velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;

		return velocityChange;
	}

	private float CalculateJumpVerticalSpeed() {
		return Mathf.Sqrt (2f * jumpHeight); // * Mathf.Abs(Physics.gravity.y));
	}


	//Check if grounded via colliders
	private void TrackGrounded (Collision collision)
	{
		float maxHeight = capsule.bounds.min.y + capsule.radius * .9f;
		foreach (ContactPoint contact in collision.contacts) {
			if (contact.point.y < maxHeight) {
				if (enteredDeath (collision)) {
					transform.position = startPosition;
				}
				else if (isKinematic(collision)) {
					// Get the ground velocity and we parent to it
					groundVelocity = collision.rigidbody.velocity;
					transform.parent = collision.transform;
				}
				else if (isStatic(collision)) {
					// Just parent to it since it's static
					transform.parent = collision.transform;
				}
				else {
					// We are standing over a dynamic object,
					// set the groundVelocity to Zero to avoid jiggers and extreme accelerations
					groundVelocity = Vector3.zero;
				}
				grounded = true;
			}
			break;
		}
	}

	private bool isKinematic(Collision collision) {
		if (collision.gameObject.tag == "PlatformVert" || collision.gameObject.tag == "PlatformHoriz" )
			return true;
		else
			return false;
	}

	private bool isKinematic(Transform transform) {
		return transform.GetComponent<Rigidbody>() && transform.GetComponent<Rigidbody>().isKinematic;
//		if (collision.gameObject && collision.gameObject.tag == "Platform")
//			return true;
//		else
//			return false;
	}

	private bool isStatic(Collision collision) {
		if (collision.gameObject.tag != "Platform")
			return true;
		else
			return false;
	}

	private bool isStatic(Transform transform) {
		return transform.gameObject.isStatic;
	}

	private bool enteredDeath (Collision collision)
	{
		if (collision.gameObject.tag == "DeathZone")
			return true;
		else
			return false;
	}
}

