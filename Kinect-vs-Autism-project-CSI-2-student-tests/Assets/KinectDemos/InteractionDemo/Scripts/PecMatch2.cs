using UnityEngine;
using System.Collections;

public class PecMatch2 : MonoBehaviour {
	//Programmer: Gabriel Goldstein
	
	//This Script is intended to be attached to PecCards -> PecPlaceHolders -> P2
	//VARIABLES NEED TO BE ASSIGNED IN EDITOR:  AudioClip snap
	//                                          GameObject emptyObject (for example: Junk)
	
	// SUMMARY: Allows the player to snap in their choice of Pec card and adds it to an array to compare with the other players choice.
	// FUNCTION:	- Turns the PecPlaceHolder green if tags match with the colliding PecPart, otherwise turns the PecPlaceHolder red
	//            	- When the player releases a PecPart in the BoxCollider of the attached object, 
	//               	It snaps the PecPart to the PecPlaceHolder & adds the PecPart to an array for error check with other players choice. 
	
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
	
	
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
		
    //Script Varables (From MainCamera)
	GrabDropScript grabScript;
	PecCard pecScript;
	
	Color color;                    //Color varable to save the color
    Renderer rend;                  //Renderer to change the Color of the object
    public Vector3 partOrigin;      //Origin of the PecPlaceHolder
    public GameObject emptyObject;  //Contains an EmptyObject (Assigned in the UnityEditor)
    Vector3 position;               //Saves the position of the PecPlaceHolder

    public AudioClip snap; //Audio played when PecPart is Snapped into PecPlaceHolder (Assigned in the UnityEditor)

    //Script Varables for Player 1 & 2 (From MainCamera)
    public InteractionManager player1;
	public InteractionManager player2;

    public log logScript; //Contains Log (From MainCamera)

    public bool arraySet = true; //If the array we assigned a PecPart to the array in the Script PecCards
    public bool occupied = false; //If current PecPlaceHOlder is Occupied with a PecPart

    //MatchingModel matchTransaction;
	
	
	void Start () {
        //Associates the Components in the MainCamera to these variables
        grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		pecScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<PecCard>();
		//assign player 1 and player 2 correctly
		player1 = (GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0]);//.playerIndex == 0) ? 
	       // GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0] : 
			//	GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		logScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<log>();

        rend = GetComponent<Renderer>(); //Gets the Renderer from the object attached to this script
        partOrigin = gameObject.transform.position; //Saves the position of the attached object
        color = rend.material.color; //Saves the color of the attached object
    }
	

	void Update () {
		//determine a snap, send to MatchingModel.snap

		Debug.Log("player 1: " + player1.isRight + "/nplayer 2: "+ player2.isRight);
	}
	
	void OnTriggerExit(Collider other) {
        //Returns Color to Object when Object leaves BoxCollider
        rend.material.color = color;
	}


    //PecPart is Colliding with the BoxCollider of the PecPlaceHolder
    void OnTriggerStay(Collider other) {
        //If PecPart tag matchs PecPlaceHolder Tag (Both for Player 1)
		if (other.gameObject.tag == gameObject.tag)
			rend.material.color = Color.green; //Turns PecPart holder Green 
        else
			rend.material.color = Color.red; //Turns PecPart holder Red
        Debug.Log(string.Format("obj1 {0}, obj2 {1}", gameObject.tag, other.gameObject.tag));

        //Player 2 Releases PecPart while colliding with PecPlaceHolder
		if (false) /*(player2.PrimaryHandEvent ==  InteractionManager.HandEventType.Release) &&*/ //( player2.isRight==true ) ) //&& other.gameObject==grabScript.draggedObject2) // && player2.playerIndex == 1) || (player1.PrimaryHandEvent == InteractionManager.HandEventType.Release && player1.playerIndex == 1)  ) //Player 2 Hand is Released
        {
			
            //If PecPart and PecPlaceHolder tag match (Both belong to Player1) AND PecPart is not snapped AND PecPlaceHolder is not occupied
            if (other.gameObject.tag == gameObject.tag && !other.GetComponent<Zzero>().IsSnapped && !occupied) {
				logScript.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  player 1 releases "+ other.gameObject.name+" in a slot");

                var obj = other.GetComponent<Zzero>();
                obj.IsSnapped = true;   //Lets PecPart know its Snapped
                occupied = true;                                //Lets PecPlaceHolder know its Occupied
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
                for (int i = 0; i < grabScript.draggableObjects.Length; i++){
                    //If PecPart Tag and Name match
                    if (other.gameObject.tag == grabScript.draggableObjects[i].tag 
                        && other.gameObject.name == grabScript.draggableObjects[i].name) {
						grabScript.draggableObjects[i] = emptyObject; //Replace with EmptyObject 
                        
                        //matchTransaction.setSnapped(other.gameObject, i);
                    }


				}

				Debug.Log("Position " + position);				
			}
		}	
	}
}
