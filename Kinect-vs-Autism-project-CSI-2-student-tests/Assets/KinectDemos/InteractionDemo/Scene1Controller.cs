using UnityEngine;
using System.Collections;

public class Scene1Controller : MonoBehaviour {

	public bool gameOver;

	public GameObject obj1;
	public GameObject resultPanel;
	public GameObject scenePieces;

	GrabDropScript grabScript;

	// Use this for initialization
	void Start () {
		grabScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GrabDropScript>();

	}
	
	// Update is called once per frame
	void Update () {
		//Detect if Player grabbed headpiece to start gameover
		if(grabScript.draggedObject1 == obj1)
		{
			gameOver = true;
		}
		//Starts Fade when game is over
		if(gameOver == true) 
		{
			gameObject.GetComponent<Animator>().SetTrigger ("GameOver");
			//Turn Result Panel On
			StartCoroutine(Wait());
		}
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(2);
		gameObject.GetComponent<Animator>().ResetTrigger("GameOver");
		scenePieces.SetActive(false);
		//resultPanel.SetActive(true);
		Application.LoadLevel("Scene 2");
	}
}
