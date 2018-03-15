using UnityEngine;
using System.Collections;

public class Zero : MonoBehaviour {

	Vector3 pos;
	GameObject gameObj;
	GrabDropScript grabScript;
	bool isGrabbed;
	Color color;
	Renderer rend;


	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
		pos = transform.position;
		pos.z = 0;
		transform.position = pos;
	}

	void OnTriggerEnter(Collider other) {

		color = rend.material.color;
		if(this.gameObject.tag == other.gameObject.tag){
			rend.material.color = Color.green;
		} else {
			rend.material.color = Color.red;
		}
	}

	void OnTriggerExit(Collider other){
		rend.material.color = color;
	}
}
