using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {



	public GameObject levelSelectPanel;

	public GameObject scorePrefab;
	public GameObject stuff;
	public Vector3 stuffVector;
	private GameManager gameManager;

	/// <summary>
	/// /////////////////////////////
	/// </summary>
	void Awake()
	{
		gameManager=GameManager.getInstance();

	}
	public void startGameButton(){
		Application.LoadLevel ("Full Game");
	}
	public void startTutorialButton() {
		Application.LoadLevel ("Scene 1");
	}
	public void startPaintButton() {
		Application.LoadLevel ("Paint");
	}
	public void startFindItButton() {
		Application.LoadLevel ("Find_It");
	}

	//Goes to the Next Level
	public void nextLevel () {
		//Application.LoadLevel (Application.loadedLevel + 1);
		SceneManager.LoadScene(gameManager.getCurrentSceneIndex());//changed 02/03/18

	}

	//Level Switch between 
	public void toLevel1(){
		Application.LoadLevel ("Scene 1");
	}
	public void toLevel2() {
		Application.LoadLevel ("Scene 2");
	}
	public void toLevel3(){
		Application.LoadLevel ("Scene 3");
	}
	public void toLevel4(){
		Application.LoadLevel ("Scene 4");
	}

	//Full Game Level Loads
	public void fullGameLevel1() {
		Application.LoadLevel ("Full Game");
	}
	public void fullGameLevel2() {
		Application.LoadLevel ("Full Game_LVL2");
	}

	public void miniGameButton(){
		Debug.Log ("Last Loaded Level: " + Application.loadedLevel);
		PlayerPrefs.SetInt("Level", Application.loadedLevel );
		Application.LoadLevel ("Eraser Mini Game");
	}


	public void levelSelectButton() {
		//Disable Main Menu Panel
		gameObject.SetActive (false);
		//Activate LevelSelectPanel
		levelSelectPanel.SetActive(true);
	}
	public void leveltoMain(){
		//Level Select window to Main Menu
		levelSelectPanel.SetActive (false);
		gameObject.SetActive(true);
	}





	public void hitNumber() {
		stuffVector = new Vector3 (0,25,0);
		stuff.transform.position = stuffVector;

		GameObject tmp = Instantiate(scorePrefab)as GameObject;
		RectTransform tmpRect = tmp.GetComponent<RectTransform>();
		//GameObject.FindGameObjectWithTag("MainCamera").GetComponent<socket>();
		//tmp.transform.SetParent(gameObject.transform);
		tmp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>());
		//tmpRect.transform.localPosition = scorePrefab.transform.localPosition;
		tmpRect.transform.localPosition = stuff.transform.localPosition;
		tmpRect.transform.localScale = scorePrefab.transform.localScale;
		Destroy (tmp, 2.0f);


	}










}
