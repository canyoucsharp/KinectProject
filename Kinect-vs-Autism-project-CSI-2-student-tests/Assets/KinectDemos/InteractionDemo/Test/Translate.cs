using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {

	public Animation Fade;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(Vector3.up * Time.deltaTime * 50);
		gameObject.GetComponent<Animator>().SetBool("Fadeout", true);
		//gameObject.GetComponent<AnimationClip>().;
		//gameObject.GetComponent<Animator>().ResetTrigger ("Fadeout");

	}
}
