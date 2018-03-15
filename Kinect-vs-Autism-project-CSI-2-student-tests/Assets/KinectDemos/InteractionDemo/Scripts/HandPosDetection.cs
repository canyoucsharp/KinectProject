using UnityEngine;
using System.Collections;

public class HandPosDetection : MonoBehaviour {


	InteractionManager interaction1;
	GrabDropScript grabScript;

	public GameObject obj1; //Head
	public GameObject obj2; //Dotted line
	public GameObject obj3; //Screen Center
	public GameObject obj4;

	public bool active;
	public bool gameOver;

	// Use this for initialization
	void Start () {
		interaction1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractionManager>();
		grabScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GrabDropScript>();
		active = false;
		gameOver = false;
	}

	// Update is called once per frame
	void Update () {

		//Checks if hand is within the center of the screen
		//Right Hand Detection
		if (interaction1.GetRightHandScreenPos().x >= .46875 && interaction1.GetRightHandScreenPos().x <= .53125){
			if (interaction1.GetRightHandScreenPos().y >= .46875 && interaction1.GetRightHandScreenPos().y <= .53125){
				Debug.Log ("Hand is in the middle 1!!");
				if(active == false)
				{
					obj1.SetActive(true);
					obj2.SetActive (true);
					obj3.SetActive (false);
					obj4.SetActive (true);
					active = true;
				}
			}
		}

		//Left Hand Detection
		if (interaction1.GetLeftHandScreenPos().x >= .4375 && interaction1.GetLeftHandScreenPos().x <= .5625){
			if (interaction1.GetLeftHandScreenPos().y >= .4375 && interaction1.GetLeftHandScreenPos().y <= .5625){
				if (active == false)
				{
					obj1.SetActive(true);
					obj2.SetActive (true);
					obj3.SetActive (false);
					obj4.SetActive (true);
					active = true;
				}
			}
		}
		/*
		//Detect if Player grabbed headpiece to start gameover
		if(grabScript.draggedObject1 == obj1)
		{
			gameOver = true;
		}

		//Starts Fade when game is over
		if(gameOver == true) 
		{
			gameObject.GetComponent<Animator>().SetTrigger ("GameOver");
			Debug.Log("GAMEOVER!!!");
			StartCoroutine(Wait());
		}


		*/}

	IEnumerator Wait() {
		yield return new WaitForSeconds(2);
		//Application.LoadLevel("Avitar Test");
	}
}
