using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreKeep : MonoBehaviour {

	public float p1Score,p2Score = 0;
	public int p1snapCount = 0;
	public int p2snapCount = 0;
	public  GameObject [] coinArr;
	private GameManager gameManager;
	private const int pointsPerSnap=1;
	private Text totalScoreDisplayed;


	void Awake()
	{
		gameManager=GameManager.getInstance();
		totalScoreDisplayed = GameObject.Find("ScoreContainer").GetComponentInChildren<Text>();
	}
	// Use this for initialization
	void Start () {
		totalScoreDisplayed.text=gameManager.getTotalScore().ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
//		Debug.Log("p1 score: " + p1Score + "\n p2 score: " + p2Score);
	}

	public void updateScore(float score, int pIndex){
		gameManager.increaseScore(pointsPerSnap);//update total score
		totalScoreDisplayed.text=gameManager.getTotalScore().ToString();
		if(pIndex == 0){
			p1snapCount ++;//score per level
			p1Score += score;
		
		}
		else{
			p2snapCount ++;
			p2Score += score;


		}
	}




}
