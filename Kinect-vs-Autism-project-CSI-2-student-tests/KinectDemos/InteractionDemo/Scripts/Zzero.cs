using UnityEngine;
using System.Collections;

public class Zzero : MonoBehaviour {

	float xPos, yPos = 0;
	Color color;
	Renderer rend;
	Vector3 pos;
	GrabDropScript grabScript;
	bool isGrabbed;

	public bool isSnapped;

	public Vector3 origin;

	public AudioClip snap;
	public AudioClip boing;
	public AudioSource source;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
		origin = this.transform.position;
		isSnapped = false;
	}

	// Update is called once per frame
	void Update () {
		isGrabbed = grabScript.isGrabbed;
		//if(!isGrabbed)
		//	transform.position = new Vector3(-4, 1, 0);

		pos = transform.position;
		pos.z = 0;
		transform.position = pos;
	}

	void OnTriggerEnter(Collider other) {

	}

	void playSound ()
	{
		source.PlayOneShot(boing);
	}
}
