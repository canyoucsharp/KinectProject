using UnityEngine;
using System.Collections;

public class MatchingModel {

	public bool BothSnapped {
		get { return bothSnapped; }
		set {}
	}

	private bool bothSnapped;
	private bool pec1Snapped, pec2Snapped;
	private bool pec1Waiting, pec2SWaiting;

	private int pec1Place, pec2Place;
	private GameObject[] pecs;
	private GameObject p1Placeholder, p2Placeholder;

	public MatchingModel() {
		pecs = new GameObject[2];
		p1Placeholder = GameObject.FindGameObjectWithTag("P1");
		p2Placeholder = GameObject.FindGameObjectWithTag("P2");

	}

	/// <summary>
	/// Keeps track of a two-part PEC card snap.
	/// </summary>
	/// <param name="obj">The PEC game object</param>
	/// <param name="dropScriptPlace">Place in GrabDropScript array, the PEC will be deactivated.</param>
	public void setSnapped(GameObject obj, int dropScriptPlace){
		if(pecs[0] == null) {
			pecs[0] = obj;
			pec1Place = dropScriptPlace;
			Debug.Log ("SNAPPED 1 PIECE");
			pec1Snapped = true;
			//BodyPart.keepInPlace(obj.GetComponent<Collider>());
		} else if(pecs[1] == null && pec1Snapped) {
			pecs[1] = obj;
			pec2Place = dropScriptPlace;
			Debug.Log ("SNAPPED 2nd PIECE");
			pec2Snapped = true;
			isMatch(); //check for a match, if they dont match reset the PEC Cards
		}
	}

	private void isMatch(){
		if(pec1Snapped && pec2Snapped){
			if (pecs[0].name == pecs[1].name)
			{
				Debug.Log ("PEC CARD MATCH. GAME WON");

			} else {
				//null the objects AND
				pecs[0].transform.position = pecs[0].GetComponent<Zzero>().origin;
				pecs[1].transform.position = pecs[1].GetComponent<Zzero>().origin;
				Debug.Log ("PEC RESET");
				pecs[0] = null;
				pecs[1] = null;
				//...reset the pieces
				bothSnapped = false;
			}
		}
	}
}
