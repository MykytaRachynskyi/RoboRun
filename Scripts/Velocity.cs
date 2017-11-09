using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Velocity : MonoBehaviour {

	private Text myText;
	private float speed;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		//UpdateSpeed(newSpeed);
	}

	public void UpdateSpeed (Vector3 newSpeed)
	{
		speed = newSpeed.z;
		myText.text = speed.ToString();
	}
}
