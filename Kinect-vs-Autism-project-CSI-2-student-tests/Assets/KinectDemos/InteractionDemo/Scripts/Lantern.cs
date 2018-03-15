using UnityEngine;
using System.Collections;

public class Lantern : MonoBehaviour {
	public int player;
    float zLoc;
    // Use this for initialization
    void Start () {
          zLoc = transform.position.z; 
    }
	
	// Update is called once per frame
	void Update () {
		/*if (player == 1) {
			Vector3 temp = Input.mousePosition;
			temp.z = zLoc + 10;
			this.transform.position = Camera.main.ScreenToWorldPoint (temp);
		} else {
			if (Input.GetKey (KeyCode.W))
				transform.Translate(Vector3.up * 2 * Time.deltaTime);


			if (Input.GetKey (KeyCode.A))
				transform.Translate(Vector3.left * 2 * Time.deltaTime);

			

			
			if (Input.GetKey (KeyCode.S))
				transform.Translate(Vector3.down * 2 * Time.deltaTime);
			
			if (Input.GetKey (KeyCode.D))
				transform.Translate(Vector3.right * 2 * Time.deltaTime);
				
		}*/
    }
}
