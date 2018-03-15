using UnityEngine;
using System.Collections;

public class PecMatch1 : MonoBehaviour {
	//Programmer: Gabriel Goldstein
	
	//This Script is attended to be attached to PecCards -> PecPlaceHolders -> P1
	//VARIABLES NEED TO BE ASSIGNED IN EDITOR:  AudioClip snap
	//                                          GameObject emptyObject
	
	// SUMMARY: Allows the player to snap in their choice of PecPart and adds it to an array to compare with the other players choice.
	// FUNCTION: 	- Turns the PecPlaceHolder green if tags match with the colliding PecPart, otherwise turns the PecPlaceHolder red
	//        		- When the player releases a PecPart in the BoxCollider of the attached object, 
	//                	It snaps the PecPart to the PecPlaceHolder & adds the PecPart to an array for error check with other players choice. */
	
	//NOTE FOR COMMENTS:    
	//PecPlaceHolder refers to Object attacted to this script
	//PecPart refers to the Object the player drags over and collides with the PecPlaceHolder
	//Not using "MatchingModel matchTransaction"
	//
	//			else if (other.GetComponent<Zzero>().isSnapped == false){
	//				//StartCoroutine(delayReset(other));
	//				other.gameObject.transform.position = other.GetComponent<Zzero>().origin;
	//				//other.GetComponent<AudioSource>().PlayOneShot (boing);
	//				Debug.Log("incorrect");
	//			}
	
	/* THINGS TO ADD: */


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
    //Script Varables (From MainCamera)
    GrabDropScript grabScript;
	PecCard pecScript;				

	Color color;					//Color varable to save the color
	Renderer rend;					//Renderer to change the Color of the object
	public Vector3 partOrigin;		//Origin of the PecPlaceHolder
	public GameObject emptyObject;	//Contains an EmptyObject (Assigned in the UnityEditor)
	Vector3 position; 				//Saves the position of the PecPlaceHolder

	public AudioClip snap; //Audio played when PecPart is Snapped into PecPlaceHolder (Assigned in the UnityEditor)

	//Script Varables for Player 1 & 2 (From MainCamera)
	public InteractionManager player1;
	public InteractionManager player2; 
	InteractionManager leftplayer;
	InteractionManager rightplayer;

	public log logScript; //Contains Log (From MainCamera)

	public bool arraySet = true; //If the array we assigned a PecPart to the array in the Script PecCards
	public bool occupied = false; //If current PecPlaceHOlder is Occupied with a PecPart

	MatchingModel matchTransaction;

	private Positionmanager _PositionManager;



	void Start () {
		//Associates the Components in the MainCamera to these variables
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		pecScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PecCard>();
		player1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0];
		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		logScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<log>();


		rend = GetComponent<Renderer>(); //Gets the Renderer from the object attached to this script
		partOrigin = gameObject.transform.position; //Saves the position of the attached object
		color = rend.material.color; //Saves the color of the attached object
		_PositionManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Positionmanager>();
	}


	void assignplayertoside()
	{
		if(player1.isRight)
		{
			leftplayer=player2;
			rightplayer=player1;

		}
		else 
		leftplayer=player1;
		rightplayer=player2;
			

	}


	bool leftplayerholdingcorrectobj()
	{
		//player1 is releasing an object&& hes on the left side&& the objects hes grabbing has the left side tag
		if(grabScript.isGrabbed1==false&&player1.isRight==false && grabScript.pecObj1.tag=="P1")
		{
			return true;
		}
		else if(grabScript.isGrabbed2==false&&player2.isRight==false && grabScript.pecObj2.tag=="P1")
		{
			return true;
		}
		else 
			return false;


	}

	bool rightplayerholdingcorrectobj()
	{
		if(grabScript.isGrabbed1==false&&player1.isRight==true && grabScript.pecObj1.tag=="P2")
		{
			return true;
		}
		else if(grabScript.isGrabbed2==false&&player2.isRight==true && grabScript.pecObj2.tag=="P2")
		{
			return true;
		}
		else 
			return false;

	}

	void snapp(Collider other)
	{
		if(other.gameObject.tag == gameObject.tag && !other.GetComponent<Zzero>().IsSnapped && !occupied) 
		{
			//if(player1.PrimaryHandEvent==InteractionManager.HandEventType.Release||player2.PrimaryHandEvent==InteractionManager.HandEventType.Release )
			//{
			//logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player 0 releases "+ other.gameObject.name+" in a slot");

			var obj = other.GetComponent<Zzero>();
			obj.IsSnapped = true; 	//Lets PecPart know its Snapped
			occupied = true;								//Lets PecPlaceHolder know its Occupied
			obj.Snappedbject = gameObject;//set snapped object
			gameObject.SetActive(false);


			//Assigns PecPart to the Match Array in PecScript
			if (arraySet) {
				other.GetComponent<AudioSource>().PlayOneShot(snap); //Plays Snap Audio

				//If the first value in PecScript Match array is null then assign PecPart to it
				if (pecScript.match[0] == null)
					pecScript.match[0] = other.gameObject;
				//If the second value in PecScript Match array is null then assign PecPart to it
				else if (pecScript.match[1] == null)
					pecScript.match[1] = other.gameObject;

				arraySet = false;
			}

			//Loops throu GrabScript's DraggableObjects array to find the PecPart to replace it with an EmptyObject
			for(int i = 0; i < grabScript.draggableObjects.Length; i++){
				//If PecPart Tag and Name match
				if(other.gameObject.tag == grabScript.draggableObjects[i].tag 
					&& other.gameObject.name == grabScript.draggableObjects[i].name){
					grabScript.draggableObjects[i] = emptyObject; //Replace with EmptyObject

					//matchTransaction.setSnapped(other.gameObject, i);
				}


			}


	}
	}
	void snappingconditions(Collider other)
	{
	if(other.tag=="P1")
		{
			Debug.Log("grabScript.isGrabbed1"+grabScript.isGrabbed1+"other.tag"+other.tag);

			if (leftplayerholdingcorrectobj())
			{	
				snapp(other);
			}


		}
	 if(other.tag=="P2")
		{
			if (rightplayerholdingcorrectobj())
			{	
			snapp(other);
			}

		}
		}

				




		
	IEnumerator wait() {
		yield return new WaitForSeconds(1);



	
	}

	



	void Update () {
		//determine a snap, send to MatchingModel.snap
		assignplayertoside();
		//Debug.Log("leftplayer.PrimaryHandEvent"+leftplayer.PrimaryHandEvent);
			
	
	}

	void OnTriggerExit(Collider other) {
		//Returns Color to Object when Object leaves BoxCollider
		rend.material.color = color; 
	}


	//PecPart is Colliding with the BoxCollider of the PecPlaceHolder
	void OnTriggerStay(Collider other) {
		//If PecPart tag matchs PecPlaceHolder Tag (Both for Player 1)
		if (other.gameObject.tag == gameObject.tag)
			rend.material.color = Color.green; //Turns PecPart Green 
		else
			rend.material.color = Color.red; //Turns PecPart Red

		snappingconditions(other);
	}


		//Player 1 Releases PecPart while colliding with PecPlaceHolder
	/*
		if ( (leftplayerholdingcorrectobj()||rightplayerholdingcorrectobj()))
			{// && (player1.isRight == false ))
			//&& other.gameObject==grabScript.draggedObject1 ) //&& player1.playerIndex == 0) || (player2.PrimaryHandEvent == InteractionManager.HandEventType.Release && player2.playerIndex == 0)) //Player 1 Hand is Released

				

		    //If PecPart and PecPlaceHolder tag match (Both belong to Player1) AND PecPart is not snapped AND PecPlaceHolder is not occupied
			if(other.gameObject.tag == gameObject.tag && !other.GetComponent<Zzero>().IsSnapped && !occupied  ) 
                {
				//if(player1.PrimaryHandEvent==InteractionManager.HandEventType.Release||player2.PrimaryHandEvent==InteractionManager.HandEventType.Release )
				//{
				logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player 0 releases "+ other.gameObject.name+" in a slot");

                    var obj = other.GetComponent<Zzero>();
                    obj.IsSnapped = true; 	//Lets PecPart know its Snapped
				    occupied = true;								//Lets PecPlaceHolder know its Occupied
                    obj.Snappedbject = gameObject;//set snapped object
					gameObject.SetActive(false);


				    //Assigns PecPart to the Match Array in PecScript
					if (arraySet) {
					other.GetComponent<AudioSource>().PlayOneShot(snap); //Plays Snap Audio

					    //If the first value in PecScript Match array is null then assign PecPart to it
						if (pecScript.match[0] == null)
							pecScript.match[0] = other.gameObject;
					    //If the second value in PecScript Match array is null then assign PecPart to it
						else if (pecScript.match[1] == null)
							pecScript.match[1] = other.gameObject;

						arraySet = false;
					}
					
				//Loops throu GrabScript's DraggableObjects array to find the PecPart to replace it with an EmptyObject
					for(int i = 0; i < grabScript.draggableObjects.Length; i++){
					//If PecPart Tag and Name match
						if(other.gameObject.tag == grabScript.draggableObjects[i].tag 
					   && other.gameObject.name == grabScript.draggableObjects[i].name){
							grabScript.draggableObjects[i] = emptyObject; //Replace with EmptyObject

							//matchTransaction.setSnapped(other.gameObject, i);
						}


					}


				Debug.Log("Position " + position);				
			}
		//}
		}
	}
}

*/
}

