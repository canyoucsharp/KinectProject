using UnityEngine;
using System.Collections;

public class PecCard : MonoBehaviour {


	public int snapCounter = 0;
	public int numPieces = 0;
	public int arraySize = 0;
	public bool bodyMatchMode = true;

	public GameObject[] pec ;

	// Use this for initialization
	void Start () {
		//pec = new GameObject[arraySize];
			
	}
	
	// Update is called once per frame
	void Update () {
	

		if(snapCounter == numPieces){
			Debug.Log("win");
			bodyMatchMode = false;
			//p2Holder.SetActive(true);
			for (int i = 0; i < pec.Length; i++) {
				//pec.GetLength(arraySize)
				pec[i].SetActive(true);

			}

		}
	}

}
