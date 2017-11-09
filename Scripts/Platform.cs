using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	public GameObject right;
	public GameObject left;
	public float speed = 1f;
	public Vector3 Speedster = Vector3.up;
	public Vector3 Speedster1 = Vector3.down;
	float direction = 1;
	Vector3 velocityChange;
	bool goingRight = true;
	bool goingUp = true;
	public float smoothTime = 0.3F;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.gameObject.tag == "PlatformHoriz") {
			if (direction == 1) {
				velocityChange = transform.position;
				velocityChange.z += speed;
				transform.position = velocityChange;
			} else {
				velocityChange = transform.position;
				velocityChange.z += (-1f) * speed;
				transform.position = velocityChange;
			}

			if (transform.position.z > right.transform.position.z && goingRight) {
				direction *= -1f;
				goingRight = false;
			}
			if (transform.position.z < left.transform.position.z && !goingRight) {
				direction *= -1f;
				goingRight = true;
			}	
		}
		if (this.gameObject.tag == "PlatformVert") {
			if (direction == 1) {
				velocityChange = transform.position;
				velocityChange.y += speed;
				transform.position = velocityChange;
				Vector3 Target = right.transform.position;
				Target.z += 1;
				transform.position = Vector3.SmoothDamp(transform.position, Target, ref Speedster, smoothTime);
			//	StartCoroutine (smooth_move (Speedster, 0.1f));
			} else {
				velocityChange = transform.position;
				velocityChange.y += (-1f) * speed;
				transform.position = velocityChange;
				Vector3 Target = left.transform.position;
				Target.z -= 1;
				transform.position = Vector3.SmoothDamp(transform.position, Target, ref Speedster1, smoothTime);
			//	StartCoroutine (smooth_move (Speedster1, 0.1f));
			}

			if (transform.position.y > right.transform.position.y && goingUp) {
				direction *= -1f;
				goingUp = false;
			}
			if (transform.position.y < left.transform.position.y && !goingUp) {
				direction *= -1f;
				goingUp = true;
			}	
		}
	}

	IEnumerator smooth_move(Vector3 direction,float speed){
         float startime = Time.time;
         Vector3 start_pos = transform.position; //Starting position.
         Vector3 end_pos = transform.position + direction; //Ending position.
 
         while (start_pos != end_pos && ((Time.time - startime)*speed) < 1f) { 
			float move = Mathf.Lerp (0,(Time.time - startime)*speed, 0);
 
             transform.position += direction*move;
 
             yield return null;
         }
     }
}
