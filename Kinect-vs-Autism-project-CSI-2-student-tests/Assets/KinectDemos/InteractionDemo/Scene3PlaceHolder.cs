using UnityEngine;
using System.Collections;

public class Scene3PlaceHolder : MonoBehaviour {
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
	void Start()
	{
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
	
	void OnTriggerEnter(Collider other)
	{
		color = rend.material.color;
		var obj = other.GetComponent<Zzero>();
		obj.triggeredObjects.Add(this.gameObject);

		if (this.gameObject.tag == other.gameObject.tag)
		{
			obj.CorrectPlaced = true;
			rend.material.color = Color.green;
		}
		else
		{
			rend.material.color = Color.red;
		}
	}
	
	void OnTriggerStay(Collider other) {
		if ((this.gameObject.tag == other.gameObject.tag) && (!grabScript.isGrabbed1))
		{
			other.gameObject.transform.position = gameObject.transform.position;
			other.gameObject.GetComponent<Zzero>().IsSnapped = true;
			gameObject.GetComponent<MeshRenderer>().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f); 
			GameObject.FindGameObjectWithTag("ScenePeices").GetComponent<Animator>().SetTrigger ("GameOver");
			StartCoroutine(Wait ());
			
			
			//gameObject.renderer.material.color.a = 1
			
			
			
		}
	}
	
	
	/*void OnTriggerExit(Collider other)
	{        
		var obj = other.GetComponent<Zzero>();
		if (this.gameObject.tag == other.gameObject.tag )
		{
			obj.CorrectPlaced = false;
		}
		obj.triggeredObjects.Remove(this.gameObject);
		rend.material.color = color; //revert color to original
	}*/
	
	/*public void keepInPlace(Collider other)
	{
		int i = 0;
		
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		//nulls the element in the array with the same name as current object that just snapped
		Debug.Log ("Sent " + other.gameObject.tag);
		
		for(i = 0; i < grabScript.draggableObjects.Length; i++) {
			
			
			if(other.gameObject.tag == grabScript.draggableObjects[i].tag)
			{
				Debug.Log ("Current GameObject " + grabScript.draggableObjects[i].tag);
				grabScript.draggableObjects[i] = emptyObject;
			}
			
			//else Debug.Log ("Current GameObject outside " + grabScript.draggableObjects[i].tag);
		}

	}*/
	IEnumerator Wait() {
		yield return new WaitForSeconds(2);
		Application.LoadLevel("Scene 4");
	}
}












