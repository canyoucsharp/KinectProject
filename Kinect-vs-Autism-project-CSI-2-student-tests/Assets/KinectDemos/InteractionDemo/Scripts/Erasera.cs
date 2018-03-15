using UnityEngine;
using System.Collections;

public class Erasera : MonoBehaviour {

	//Assigned in the GUI
	//The player ID (0=player1, 1=player2)
	public int playernum;
	AudioSource fx;

	void Start () {
		fx = GetComponent<AudioSource>();
	}


	void OnTriggerEnter(Collider other) {
		//Checks if Eraser and overlay belong to the same player
		int blocknum=-1;
		if(other.gameObject.GetComponent<CoverBlocks>()!=null)
			 blocknum=other.gameObject.GetComponent<CoverBlocks>().playerDestroyed;
		{
		
			 if (playernum==blocknum){
        		other.gameObject.GetComponent<CoverBlocks>().shrinking = true;
				fx.Play();
			}
		}
    }


}
