using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GroundedText : MonoBehaviour {

	private Text myText;
	private string grounded = "Grounded";
	private string notGrounded = "Not Grounded";
	
	// Use this for initialization
	void Start () {
		myText = GetComponent<Text> ();
		if (myText == null) {
			return;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		

	}

	public void UpdateText (bool flag)
	{
		if (flag == true) 
			myText.text = grounded;
		else
			myText.text = notGrounded;
	}
}
