using UnityEngine;
using System.Collections;

public class PecMatch : MonoBehaviour {

	public GameObject P1, P2;
	bool p1isMatch;
	bool p2isMatch;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (P1.name == P2.name)
		{
			Debug.Log ("PEC CARD MATCH");
		}
	}

	void OnTriggerStay(Collider other) {

	}

}
