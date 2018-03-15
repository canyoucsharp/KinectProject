using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using UnityEngine.SceneManagement;

public class PecCard : MonoBehaviour {
	//Programmer: Gabriel Goldstein

	//This Script is intended to be attached to Misc -> MainCamera
	//VARIABLES NEED TO BE ASSIGNED IN EDITOR:  int snapCounter = 0
	//											int numPieces = "# of pieces to complete"
	//											int arraySize = 2
	//											bool bodyMatchMode = true
	//											AudioClip boing
	//											AudioClip clapping
	//											GameObject smokeEffect
	//											GameObject avatar
	//											GameObject bodyParts
	//											GameObject PECCards
	//											GameObject resultPanel
	//											GameObject correctPec
	
	//SUMMARY: Handles the logic for the PecCard Portion of the game. 
	//			If Correct, handles winning animations, enabling and disabling associated GameObjects
	//			If Incorrect, handles reseting PecParts so players can try again
	//FUNCTIONALITY:
	//Checks if all BodyParts are SnappedIn, turns on visiblity of PecCards & turns off BodyParts
	//Checks if both Players SnappedIn their PecCard choice,
	//	If correct, then both PecPart's name match && Correct PecPart for the level, then handles Winning
	//	if incorrect, then resets position of the PecPart, resets bool in the PecPlaceHolder, places PecPart back into the draggableObjects array

	//NOTE FOR COMMENTS:
	//arraySize may not be needed
	//Pec Array may not be needed or used
	//Variables "Temp" & "Temp2" may not need to be public 
	//Variables "Temp" & "Temp2" need to be renamed to something better
	//PecStarted may not be needed
	//PecHolders not commented
	//Resets in PlaceHolder array can be done in a function to reduce lines of code
	//Can instead of use GameObjects to reference just reference the item needed. Like use the name or just the variable we need
	//		Idea only works for CorrectPec where we only check its name

	// bodyMatchMode and PecStarted - should only need one bool

	/*THINGS TO ADD:
	 	(Done)-You only win when both players select the same face AND its the PEC intended for that level
	 		Added "Public GameObject CorrectPec"
			Added to the if statement where it checks if faces match and add
				"(match[0].name==match[1].name&& match[1or2]==CorrectPec)"
	*/


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


	//Script Variables (from MainCamera)
	GrabDropScript grabScript;
	InteractionManager[] interactionManager;

	public int snapCounter; //Number of BodyParts currently SnappedIn
	public int numPieces; 	//Number of total BodyParts that need to be SnappedIn
	public int arraySize;
	private EngagementMeter engagementMeter;
	public GameObject[] match; //Array that holds each players choice of PecPart for is it correct checking
	public GameObject[] pec;
	public GameObject[] placeHolders; //Array that holds the PecPlaceHolders for resetting

	public AudioSource source;
	public AudioClip boing; //Audio Played when Incorrect
	public AudioClip clapping; //Audio Played when PecCards are finished

	public GameObject pause;
	public GameObject smokeEffect; //Contains SmokeEffect GameObject (Assigned in the UnityEditor)
	public GameObject avatar; //Contains the 3D Avatar Model (Assigned in the UnityEditor) - used as a reward feedback
	public GameObject bodyParts; //Parent Object containing all the BodyParts
	public GameObject PECCards; //Parent Object containing all the PecParts

	public GameObject resultPanel; //Associates to the Parent of the ResultPanel to activate when needed
	public log scriptLog;   
	public Timer result; //Varable used for the overall Timer

	//bodyMatchMode  true when the game is currently in Body Matching Mode (First Part of the Level - noncollaborative)
	//               false when complete and switched to PEC - collaborative
	public bool bodyMatchMode = true;	
	public bool pecStarted = false; //If the PecCard portion of the game has started 
	public bool firstTime = true;
	public bool temp2 = false; //Makes sure the if statement only happens once

	int attempt = 1; //Total # of attempts the Players made to complete the PecCard

	/*RECENT ADDITION (Await testing for adding)*/
	public GameObject correctPec; //Contains the Answer for the level (Assigned in the Unity Editor)
	public GameObject headOutline; //The HeadOutline before the PEC MODE starts
	PecMatch1 _Pecmatch1;
	private FacetrackingManager faceTrackingManager;
	private GameManager gameManager;
	private ScoreKeep scoreKeepBody;



	void Awake()
	{
		gameManager=GameManager.getInstance();
	}
	void Start () {
		//pec = new GameObject[arraySize];

		//Associate the variables with Components in the MainCamera
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();
		interactionManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>();
		result = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Timer>();
		scriptLog = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<log>();
		engagementMeter = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<EngagementMeter>();
		faceTrackingManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FacetrackingManager>();
		scoreKeepBody=GameObject.Find("BodyPlaceHolders").GetComponent<ScoreKeep>();
	   
	

		source = GetComponent<AudioSource>(); //Gets AudioSource from the attached Object
	}


	void Update () {


		
		//If all BodyParts are SnappedIn
		if((snapCounter == numPieces)&&(temp2 == false)){
			Debug.Log("BEGIN PEC MODE");
			//scriptLog.file.WriteLine("\n"+System.DateTime.Now.ToString("hh:mm:ss")+"  Pec Mode starts");

			bodyMatchMode = false;	//Switchs mode
			PECCards.SetActive (true); //Turns on all PECCards
			pause.SetActive(true);
			headOutline.SetActive(false);//Turns HeadOutline off to not be in the way of the PECPLACEHOLDER
			temp2 = true; //To prevent loop
			pecStarted = true; //Log purpose in Zzero

			//string csvfile="C:\\Users\\user\\Desktop\\cloud.csv";
			//StringBuilder csvcontent=new StringBuilder();
			//csvcontent.AppendLine("\nPec Started\n");

			//File.AppendAllText(csvfile,csvcontent.ToString());
		}


		//If both objects in the Match array are filled
		if (match[0] != null && match[1] != null){ 
			//If both object in the Match array have the same name
			if ((match[0].name == match[1].name) && match[0].name == correctPec.name) {		/*ADD MATCHES THE CORRECT FACE FOR THE LEVEL*/
				//Starts PecCard part of the Game
				Debug.Log ("CONTINUE");

				if (firstTime){
					source.PlayOneShot(clapping); //Play Clapping Audio
					smokeEffect.SetActive(true); //Starts the SmokeEffect
					firstTime = false;
					placeHolders[0].gameObject.SetActive(false);
					placeHolders[1].gameObject.SetActive(false);
					faceTrackingManager.sendToCsv();
					StartCoroutine(wait()); //Wait 3 Secs to activate model and deactivate Parts,PEC Card;




				}
			}


			//When the Players choice for PecParts are incorrect, will reset pieces.
			else {
				Debug.Log ("RESTART");
				scriptLog.file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  Incorret PEC restarts, attempts: "+attempt); 

				attempt++; //Increases the # of attempts
				source.PlayOneShot (boing); //Plays SFX when incorrect

				//Sets PEC cards back to there original positions
				match[0].gameObject.transform.position = match[0].gameObject.GetComponent<Zzero>().origin;
				match[0].gameObject.GetComponent<Zzero>().IsSnapped = false;

				match[1].gameObject.transform.position = match[1].gameObject.GetComponent<Zzero>().origin;
				match[1].gameObject.GetComponent<Zzero>().IsSnapped = false;

				placeHolders[0].SetActive(true);
				placeHolders[1].SetActive(true);
				//Sets the Array set function to true so function can repeate multiple times.
				//Resets First Element in the PlaceHolders Array
				//if (placeHolders[0].gameObject.tag == "P1"){	/*Change to "placeHolders[0].gameObject.CompareTag("P1")*/
					placeHolders[0].gameObject.GetComponent<PecMatch1>().arraySet = true;
					placeHolders[0].gameObject.GetComponent<PecMatch1>().occupied = false;
			//	}
				//else if (placeHolders[0].gameObject.tag == "P2"){	/*Change to "placeHolders[0].gameObject.CompareTag("P2")*/
				//	placeHolders[0].gameObject.GetComponent<PecMatch1>().arraySet = true;
				//	placeHolders[0].gameObject.GetComponent<PecMatch1>().occupied = false;
			//	}

				//Resets Second Element in the PlaceHolders Array
				//if (placeHolders[1].gameObject.tag == "P1"){	/*Change to "placeHolders[0].gameObject.CompareTag("P1")*/
					placeHolders[1].gameObject.GetComponent<PecMatch1>().arraySet = true;
					placeHolders[1].gameObject.GetComponent<PecMatch1>().occupied = false;

				//}
			//	else if (placeHolders[1].gameObject.tag == "P2"){	/*Change to "placeHolders[0].gameObject.CompareTag("P1")*/
					//placeHolders[1].gameObject.GetComponent<PecMatch1>().arraySet = true;
				//	placeHolders[1].gameObject.GetComponent<PecMatch1>().occupied = false;
				//}

				//Loops throu the DraggableObjects array in the GrabScript and places the PecPart back into the DraggableObjects array
				for(int i = 0; i < grabScript.draggableObjects.Length; i++){
					/*Change if statement tag compare to "grabScript.draggableObjects[i].CompareTag("Junk")*/
					if(grabScript.draggableObjects[i].tag == "Junk") { //If draggableObjects[i] is an EmptyObject
						if (match[0] != null) { //If the First Element in the Match Array is not NULL
							grabScript.draggableObjects[i] = match[0]; //Places PecPart back into draggableObjects Array
							match[0] = null;
						}
						else if (match[1] != null){ //If the Second Element in the Match Array is not NULL
							grabScript.draggableObjects[i] = match[1]; //Places PecPart back into draggableObjects Array
							match[1] = null;
						}
					}
				}
			}
		}
	}

	//Called when PecCard Mode is correct
	//Handles Winning
	IEnumerator wait() {
		result.finish(); //Stops Timer
		disableCoins();
		yield return new WaitForSeconds(3);	//Wait 3 Secs
		avatar.SetActive (true);	//Turns on Avatar GameObject
		PECCards.SetActive(false);	//Turns off PecCards
		bodyParts.SetActive(false);	//Turns off BodyParts

		yield return new WaitForSeconds(1);	//Wait 7 Secs
		avatar.SetActive (false); //Disable Avitar
		resultPanel.SetActive(true); //Enable Results page

		//Disable both Players Hands 
		interactionManager[0].useHandCursor = false;
		interactionManager[1].useHandCursor = false;
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		gameManager.setCurrentSceneIndex(SceneManager.GetActiveScene().buildIndex + 1);

	}

	private void disableCoins()
	{
		for(int i=0;i<scoreKeepBody.coinArr.Length;i++)
		{
			scoreKeepBody.coinArr[i].SetActive(false);
		}
	}






}

