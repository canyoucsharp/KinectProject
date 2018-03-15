using UnityEngine;
using System.Collections;
using System;
public class BodyPart : MonoBehaviour {

	Vector3 pos;
	GrabDropScript grabScript;
	bool isGrabbed;
	Color color;
	Renderer rend;
	Vector3 partOrigin;
	public GameObject emptyObject;

	public AudioClip snap;
	public AudioClip boing;

	public PecCard pec;
	public AudioSource source;

	bool isSnapped = false;
	
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		source = GetComponent<AudioSource>();
		pec = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();
	}
	
	// Update is called once per frame
	void Update () {

		pos = transform.position;
		pos.z = 0;
		transform.position = pos;
	}
	
	void OnTriggerEnter(Collider other) {
		
		color = rend.material.color;
		if(this.gameObject.tag == other.gameObject.tag)
		{
			if (grabScript.isGrabbed == true)
			{
				rend.material.color = Color.green;
			}
		} 
        else 
        {
			rend.material.color = Color.red;
		}
	}

	void OnTriggerStay(Collider other) {

		if (grabScript.isGrabbed == false)
		{
			if(other.gameObject.tag == gameObject.tag)
            { //if the body part matches
				other.GetComponent<Zzero>().isSnapped = true;
				pec.snapCounter ++;
				//other.isSnapped = true;
				rend.material.color = color;
				Vector3 position = gameObject.transform.position;
				other.gameObject.transform.position = position;
				Debug.Log ("Match");

				Debug.Log("game mode: " + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode);
				if(other.GetComponent<Zzero>().isSnapped && GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>().bodyMatchMode){
					other.GetComponent<AudioSource>().PlayOneShot(snap);
					gameObject.SetActive(false);
					Debug.Log("Snapped " + other.gameObject.tag);
					keepInPlace(other);
				}
				Debug.Log("Position " + position);
				//The line below delays a check for an incorrect piece
				//prevents multiple correct/incorrect piece placement

			} 
            else if (other.GetComponent<Zzero>().isSnapped == false)
            {
				//StartCoroutine(delayReset(other));
				//other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position,other.GetComponent<Zzero>().origin,Time.deltaTime*30);
				other.GetComponent<AudioSource>().PlayOneShot (boing);
				Debug.Log("incorrect");
			}
		}
	}

	//Explanation
	//http://forum.unity3d.com/threads/how-can-i-make-a-c-method-wait-a-number-of-seconds.61011/
	IEnumerator delayReset(Collider other){
		yield return new WaitForSeconds(0.5f);
		if (other.GetComponent<Zzero>().isSnapped == false){

//			other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
//			other.GetComponent<AudioSource>().PlayOneShot (boing);
			Debug.Log ("Both");
		}
	}

	void OnTriggerExit(Collider other){
		rend.material.color = color; //revert color to original
	}

	void keepInPlace(Collider other)
	{

		int i = 0;
		
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		//nulls the element in the array with the same name as current object that just snapped
		Debug.Log ("Sent " + other.gameObject.tag);

		for(i = 0; i < grabScript.draggableObjects.Length; i++){


			if(other.gameObject.tag == grabScript.draggableObjects[i].tag)
			{
				Debug.Log ("Current GameObject " + grabScript.draggableObjects[i].tag);
				grabScript.draggableObjects[i] = emptyObject;
			}

			//else Debug.Log ("Current GameObject outside " + grabScript.draggableObjects[i].tag);
		}
		


	}
}


























