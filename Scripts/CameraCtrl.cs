using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

	public Transform target;
	public float lookSmooth = 0.01f;
	public Vector3 offsetFromTarget = new Vector3 (0, 6, -8);
	public float xTilit = 10f;

	Vector3 velocity = Vector3.zero;

	Vector3 destination = Vector3.zero;
	CharacterController charController;
	//float rotateVel = 0;

	void Start () {
		Quaternion lookAngle = Quaternion.Euler (xTilit, -90, 0);
		SetCamerTarget (target);
		transform.rotation = lookAngle;
	}

	void LateUpdate () {
		MoveToTarget ();
	}

	void SetCamerTarget (Transform t){
		target = t;

//		if (target != null) 
//		{
//			if (target.GetComponent<PlayerCtrl> ())
//				charController = target.GetComponent<PlayerCtrl> ();
//			else
//				Debug.LogError ("No character controller");
//		} else
//			Debug.LogError ("No target");
	}

	void MoveToTarget ()
	{
		destination = target.position + offsetFromTarget;
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, lookSmooth);
	}
}
