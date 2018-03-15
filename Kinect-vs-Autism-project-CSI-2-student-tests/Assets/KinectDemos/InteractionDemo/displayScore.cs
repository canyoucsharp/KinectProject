using UnityEngine;
using System.Collections;
//using System.Timers.Timer;

public class displayScore : MonoBehaviour {

	public int secondsToSpinFor = 3;
	private float timer = 0;
	//private System.Timers.Timer timer;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Animator>().SetBool("Fadeout", true);


	}
	
	// Update is called once per frame
	void Update () {
	
		gameObject.transform.Translate(Vector3.up * Time.deltaTime * 2.0f );
		//gameObject.GetComponent<Animator>().SetBool("Fadeout", true);
		//timer += Time.deltaTime / secondsToSpinFor;
		//if (timer > 1)
		//	gameObject.GetComponent<Animator>().SetBool("Fadeout", false);
	}
}
